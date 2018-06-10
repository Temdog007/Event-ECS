#include "stdafx.h"

namespace EventECS
{
	void(*ECSMap::logHandler)(const char* log);

	ECSMap* ECSMap::instance = nullptr;

	ECSMap::ECSMap() 
		: L(luaL_newstate()), loveInitialized(false), disposed(false), dispatchingEvent(false), logEvents(false)
	{
		SetDefaults();
	}

	ECSMap::ECSMap(const char* initializerCode, size_t size) 
		: L(luaL_newstate()), loveInitialized(false), disposed(false), dispatchingEvent(false), logEvents(false)
	{
		SetDefaults();

		LoadInitializerCode(initializerCode, size);

		Reset();
	}

	ECSMap::ECSMap(const char* initializerCode, size_t size, const char* executablePath, const char* identity) 
		: L(luaL_newstate()), loveInitialized(false), disposed(false), dispatchingEvent(false), logEvents(false)
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

		lua_pushcfunction(L, lua_logFunction);
		lua_setglobal(L, "Log");
	}

	void ECSMap::LoadInitializerCode(const char* initializerCode, size_t size)
	{
		if (luaL_loadbuffer(L, initializerCode, size, "initliaze.lua") != 0)//load function and put on stack
		{
			throw std::exception(lua_tostring(L, -1));
		}

		int bufferRef = luaL_ref(L, LUA_REGISTRYINDEX); // store function in registry
		lua_pop(L, 1); // remove function from stack

		lua_getglobal(L, "package");
		lua_getfield(L, -1, "preload");
		lua_rawgeti(L, LUA_REGISTRYINDEX, bufferRef); // put function in initializer in preload table
		lua_setfield(L, -2, "initializer");
		lua_pop(L, 2);

		luaL_unref(L, LUA_REGISTRYINDEX, bufferRef); // remove from registry
	}

	void ECSMap::Execute(const char* code)
	{
		if (luaL_loadstring(L, code) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}

		luax_call(L, 0, 0);
	}

	void ECSMap::Execute(const char* code, const char* systemName)
	{
		if (luaL_loadstring(L, code) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}

		lua_rawgeti(L, LUA_REGISTRYINDEX, map[systemName].GetIDX());
		luax_call(L, 1, 0);
	}

	void ECSMap::Reset()
	{
		map.clear();

		lua_getglobal(L, "require");
		lua_pushstring(L, "initializer");

		luax_call(L, 1, 1); // get initializer function

#ifdef NDEBUG
		luax_pushboolean(L, false); // push false to signal not debug mode
#else
		luax_pushboolean(L, true); // push true to signal debug mode
#endif
		luax_call(L, 1, LUA_MULTRET); // call initializer function

		Log("%d system(s) in initalizer script", lua_gettop(L));

		std::list<int> refs;
		while (lua_gettop(L) > 0) // get all systems
		{
			refs.push_back(luaL_ref(L, LUA_REGISTRYINDEX));
		}

		for (int ref : refs)
		{
			lua_rawgeti(L, LUA_REGISTRYINDEX, ref);
			lua_getfield(L, -1, "getName");
			lua_pushvalue(L, -2);
			luax_call(L, 1, 1);
			const char* name = lua_tostring(L, -1);
			map.emplace(std::piecewise_construct, std::make_tuple(name), std::make_tuple(L, ref));
			lua_pop(L, 1);
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

	bool ECSMap::HasSystem(const char* name) const 
	{
		return map.find(name) != map.end();
	}

	ECS& ECSMap::operator[](const char* name)
	{
		if (!HasSystem(name))
		{
			char buffer[100];
			sprintf_s(buffer, "System '%s' not found", name);
			throw std::exception(buffer);
		}
		return map[name];
	}

	const ECS& ECSMap::operator[](const char* name) const
	{
		if (!HasSystem(name))
		{
			char buffer[100];
			sprintf_s(buffer, "System '%s' not found", name);
			throw std::exception(buffer);
		}
		return map.at(name);
	}

	std::string ECSMap::GetClassname(const char* name) const
	{
		if (map.find(name) != map.end())
		{
			lua_settop(L, 0);
			lua_getglobal(L, "classname");
			lua_rawgeti(L, LUA_REGISTRYINDEX, (*this)[name].GetIDX());
			if (lua_pcall(L, 1, 1, 0) == 0)
			{
				return std::string(lua_tostring(L, -1));
			}
			else
			{
				throw std::exception(lua_tostring(L, -1));
			}
		}
		return "";
	}

	void ECSMap::Quit()
	{
		if (loveInitialized)
		{
			try 
			{
				lua_settop(L, 0);
				lua_getglobal(L, "love");
				lua_getfield(L, -1, "event");
				lua_getfield(L, -1, "quit");
				loveInitialized = false;
			}
			catch (const std::exception& e)
			{
				Log(e.what());
			}
			catch (...) 
			{
				Log("Unknown error occurred while quitting Love");
			}
		}
	}

	bool ECSMap::InitializeLove(const char* executablePath, const char* identity)
	{
		if (!loveInitialized)
		{
			Require("love", "love");
			Require("loveBoot", "loveInitializerFunctions");

			lua_settop(L, 0);
			lua_getglobal(L, "loveInitializerFunctions");
			lua_getfield(L, -1, "bootLove");
			lua_pushcfunction(L, lua_broadcastEvent);
			lua_pushstring(L, identity);
			lua_pushstring(L, executablePath);
			luax_call(L, 3, 0);
			Log("Created a LOVE2D Entity Component System");
			loveInitialized = true;
		}
		return loveInitialized;
	}

	void ECSMap::Require(const char* modName, const char* globalName)
	{
		lua_settop(L, 0);
		lua_getglobal(L, "require");
		lua_pushstring(L, modName);
		luax_call(L, 1, 1);
		
		if (globalName != nullptr)
		{
			lua_setglobal(L, globalName);
		}
		else
		{
			lua_pop(L, 1);
		}
	}

	void ECSMap::Unregister(const char* modName)
	{
		lua_settop(L, 0);
		lua_getglobal(L, "package");
		lua_getfield(L, -1, "loaded");
		lua_pushstring(L, modName);
		lua_pushnil(L);
		lua_settable(L, -3);
		lua_settop(L, 0);
	}

	void ECSMap::BroadcastEventWithArgs(const char* eventName)
	{
		lua_newtable(L);
		int ref = luaL_ref(L, LUA_REGISTRYINDEX);
		BroadcastEvent(eventName, ref);
		lua_pop(L, 1);
	}

	void ECSMap::BroadcastEvent(const char * eventName, int ref)
	{
		queue.emplace(L, eventName, ref);
		if (!dispatchingEvent)
		{
			EmptyEventQueue();
		}
	}

	void ECSMap::EmptyEventQueue()
	{
		dispatchingEvent = true;
		while (!queue.empty())
		{
			const Event& ev = queue.front();
			int handlers = 0;
			for (auto iter = map.begin(); iter != map.end(); ++iter)
			{
				if (ev.ref == LUA_REFNIL)
				{
					handlers += iter->second.DispatchEvent(ev.name);
				}
				else 
				{
					if (logEvents && !IsIgnored(ev.name))
					{
						Log("Event '%s' will be called with ref '%d.'", ev.name, ev.ref);
					}
					handlers += iter->second.DispatchEvent(ev.name, ev.ref);
				}
			}
			if (logEvents && !IsIgnored(ev.name))
			{
				Log("Event '%s' was handled by '%d' components", ev.name, handlers);
			}
			queue.pop();
		}
		dispatchingEvent = false;
	}

	bool ECSMap::UpdateLove()
	{
		lua_settop(L, 0);
		lua_getglobal(L, "loveInitializerFunctions");
		lua_getfield(L, -1, "updateLove");
		luax_call(L, 0, 2);
		bool rval = luax_toboolean(L, -1);
		if (!rval)
		{
			if (lua_isstring(L, -2))
			{
				const char* message = lua_tostring(L, -2);
				Log(message);
			}
		}
		lua_settop(L, 0);
		return rval;
	}

	lua_Number ECSMap::DrawLove(bool skipSleep)
	{
		lua_settop(L, 0);
		lua_getglobal(L, "loveInitializerFunctions");
		lua_getfield(L, -1, "drawLove");
		luax_pushboolean(L, skipSleep);
		luax_call(L, 1, 2);

		lua_Number sleep = lua_tonumber(L, -1);
		if (lua_isstring(L, -2))
		{
			const char* message = lua_tostring(L, -2);
			Log(message);
		}
		lua_settop(L, 0);
		return sleep;
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

	bool ECSMap::IsLoggingEvents() const 
	{
		return logEvents;
	}

	void ECSMap::SetLogEvents(bool value)
	{
		logEvents = value;
	}

	void ECSMap::SetLogHandler(void(*func)(const char*))
	{
		ECSMap::logHandler = func;
	}

	std::string tolower(const std::string& s)
	{
		std::string rval = s;
		std::transform(rval.begin(), rval.end(), rval.begin(), ::tolower);
		return rval;
	}

	void ECSMap::SetEventsToIgnore(const std::set<std::string>& events)
	{
		eventsToIgnore.clear();
		for (auto iter = events.begin(); iter != events.end(); ++iter)
		{
			eventsToIgnore.emplace(tolower(*iter));
		}
	}

	bool ECSMap::IsIgnored(const std::string& ev) const 
	{
		return eventsToIgnore.find(tolower(ev)) != eventsToIgnore.end();
	}

	void ECSMap::Log(const char* format, ...)
	{
		if (logHandler != nullptr && format != nullptr)
		{
			va_list args;
			va_start(args, format);
			char log[1000];
			vsnprintf(log, sizeof(log), format, args);
			logHandler(log);
			va_end(args);
		}
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
		if (ECSMap::instance != nullptr)
		{
			const char* log = luaL_checkstring(L, 1);
			if (log != nullptr) 
			{
				if (lua_isnil(L, 2))
				{
					ECSMap::instance->BroadcastEvent(log);
				}
				else
				{
					lua_pushvalue(L, 2);
					int reg = luaL_ref(L, LUA_REGISTRYINDEX);
					ECSMap::instance->BroadcastEvent(log, reg);
				}
			}
		}
		return 0;
	}

	ECSMap::Event::Event(lua_State*const pL, const char* pName, int pRef) : L(pL), name(pName), ref(pRef) {}

	ECSMap::Event::~Event()
	{
		if (ref != LUA_REFNIL)
		{
			luaL_unref(L, LUA_REGISTRYINDEX, ref);
		}
	}
}