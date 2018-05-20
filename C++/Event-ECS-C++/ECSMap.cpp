#include "stdafx.h"

namespace EventECS
{
	void(*ECSMap::logHandler)(const char* log);

	ECSMap* ECSMap::instance = nullptr;

	ECSMap::ECSMap() : L(luaL_newstate()), loveInitialized(false), disposed(false)
	{
		SetDefaults();
	}

	ECSMap::ECSMap(const char* initializerCode, size_t size) : L(luaL_newstate()), loveInitialized(false), disposed(false)
	{
		SetDefaults();

		LoadInitializerCode(initializerCode, size);

		Reset();
	}

	ECSMap::ECSMap(const char* initializerCode, size_t size, const char* executablePath, const char* identity) : L(luaL_newstate()), loveInitialized(false), disposed(false)
	{
		SetDefaults();

		if (!InitializeLove(executablePath, identity))
		{
			throw std::exception("Failed to initialize LOVE");
		}

		LoadInitializerCode(initializerCode, size);

		Reset();
	}

	ECSMap::~ECSMap()
	{
		Quit();

		map.clear();

		lua_close(L);
	}

	void ECSMap::SetDefaults()
	{
		luaL_openlibs(L);

		Require("eventecs", nullptr);
		Require("system", "System");

		lua_pushcfunction(L, lua_logFunction);
		lua_setglobal(L, "Log");
	}

	void ECSMap::LoadInitializerCode(const char* initializerCode, size_t size)
	{
		if (luaL_loadbuffer(L, initializerCode, size, "initliaze.lua") != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}

		int bufferRef = luaL_ref(L, LUA_REGISTRYINDEX);
		lua_pop(L, 1);

		lua_getglobal(L, "package");
		lua_getfield(L, -1, "preload");
		lua_rawgeti(L, LUA_REGISTRYINDEX, bufferRef);
		lua_setfield(L, -2, "initializer");
		lua_pop(L, 2);

		luaL_unref(L, LUA_REGISTRYINDEX, bufferRef);
	}

	void ECSMap::Reset()
	{
		map.clear();

		lua_getglobal(L, "require");
		lua_pushstring(L, "initializer");
		if (lua_pcall(L, 1, 1, 0) != 0 || // get initializer function
			lua_pcall(L, 0, LUA_MULTRET, 0) != 0) // call initializer function
		{
			throw std::exception(lua_tostring(L, -1));
		}

		std::list<ECS> list;
		while (lua_gettop(L)) // get all systems
		{
			list.push_back(ECS(L));
			lua_pop(L, 1);
		}

		for (auto ecs : list)
		{
			map.emplace(ecs.GetSystemString("name"), ecs);
		}
	}

	ECSMap* ECSMap::CreateInstance()
	{
		DeleteInstance();
		return (instance = new ECSMap());
	}

	ECSMap* ECSMap::CreateInstance(const char* initializerCode, size_t size)
	{
		DeleteInstance();
		return (instance = new ECSMap(initializerCode, size));
	}

	ECSMap* ECSMap::CreateInstance(const char* initializerCode, size_t size, const char* executablePath, const char* identity)
	{
		DeleteInstance();
		return (instance = new ECSMap(initializerCode, size, executablePath, identity));
	}

	void ECSMap::DeleteInstance()
	{
		if (instance != nullptr)
		{
			delete instance;
			instance = nullptr;
		}
	}

	void ECSMap::Add(const char* name)
	{
		if (map.find(name) != map.end())
		{
			char buffer[100];
			sprintf_s(buffer, "'%s' already exists in the map", name);
			throw std::exception(buffer);
		}

		lua_getglobal(L, "System");
		lua_pushstring(L, name);

		if (lua_pcall(L, 1, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}

		int idx = luaL_ref(L, LUA_REGISTRYINDEX);

		map.emplace(std::piecewise_construct, std::make_tuple(name), std::make_tuple(L, idx));
	}

	size_t ECSMap::Remove(const char* name)
	{
		return map.erase(name);
	}

	ECS& ECSMap::operator[](const char* name)
	{
		return map.at(name);
	}

	const ECS& ECSMap::operator[](const char* name) const
	{
		return map.at(name);
	}

	void ECSMap::Quit()
	{
		if (loveInitialized)
		{
			lua_settop(L, 0);
			lua_getglobal(L, "love");
			lua_getfield(L, -1, "event");
			lua_getfield(L, -1, "quit");
			if (lua_pcall(L, 0, 0, 0) == 0)
			{
				while (this->UpdateLove());
			}
			loveInitialized = false;
		}
	}

	bool ECSMap::InitializeLove(const char* executablePath, const char* identity)
	{
		Require("love", "love");
		Require("loveBoot", "loveInitializerFunctions");

		lua_settop(L, 0);
		lua_getglobal(L, "loveInitializerFunctions");
		lua_getfield(L, -1, "bootLove");
		lua_pushcfunction(L, lua_broadcastEvent);
		lua_pushstring(L, identity);
		lua_pushstring(L, executablePath);
		if(lua_pcall(L, 3, 0, 0) == 0)
		{
			if (ECSMap::logHandler != nullptr)
			{
				ECSMap::logHandler("Created a LOVE2D Entity Component System");
			}
		}
		else
		{
			throw std::exception(lua_tostring(L, -1));
		}
		loveInitialized = true;

		return loveInitialized;
	}

	void ECSMap::Require(const char* modName, const char* globalName)
	{
		lua_settop(L, 0);
		lua_getglobal(L, "require");
		lua_pushstring(L, modName);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			if (globalName != nullptr)
			{
				lua_setglobal(L, globalName);
			}
			else
			{
				lua_pop(L, 1);
			}
		}
		else
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	int ECSMap::BroadcastEvent(const char * eventName)
	{
		int handlers = 0;
		for (auto iter = map.begin(); iter != map.end(); ++iter)
		{
			handlers += iter->second.DispatchEvent(eventName);
		}
		return handlers;
	}

	bool ECSMap::UpdateLove()
	{
		lua_settop(L, 0);
		lua_getglobal(L, "loveInitializerFunctions");
		lua_getfield(L, -1, "updateLove");
		if (lua_pcall(L, 0, 0, 0) == 0)
		{
			bool rval = static_cast<bool>(lua_toboolean(L, -1));
			lua_pop(L, 1);
			return rval;
		}

		throw std::exception(lua_tostring(L, -1));
	}

	lua_Number ECSMap::DrawLove(bool skipSleep)
	{
		lua_settop(L, 0);
		lua_getglobal(L, "loveInitializerFunctions");
		lua_getfield(L, -1, "drawLove");
		lua_pushboolean(L, static_cast<int>(skipSleep));
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			lua_Number sleep = lua_tonumber(L, -1);
			lua_pop(L, 1);
			return sleep;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	std::list<std::string> ECSMap::Serialize() const
	{
		std::list<std::string> list;
		for(auto iter = map.begin(); iter != map.end(); ++iter)
		{
			list.push_back(iter->second.Serialize());
		}
		return list;
	}

	void ECSMap::SetLogHandler(void(*func)(const char*))
	{
		ECSMap::logHandler = func;
	}

	int ECSMap::lua_logFunction(lua_State* L)
	{
		auto logHandler = ECSMap::logHandler;
		if (logHandler != nullptr)
		{
			for (int i = 1; i <= lua_gettop(L); ++i)
			{
				const char* log = luaL_checkstring(L, i);
				if (log == nullptr)
				{
					continue;
				}
				else
				{
					logHandler(log);
				}
			}
		}

		return 0;
	}

	int ECSMap::lua_broadcastEvent(lua_State* L)
	{
		if (ECSMap::instance == nullptr)
		{
			return 0;
		}
		const char* log = luaL_checkstring(L, 1);
		if (log == nullptr)
		{
			return 0;
		}
		int handles = ECSMap::instance->BroadcastEvent(log);
		lua_pushnumber(L, handles);
		return 1;
	}
}