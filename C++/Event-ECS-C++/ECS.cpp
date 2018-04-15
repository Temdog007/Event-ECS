#include "stdafx.h"

const char System[] = "System";
const char mySystem[] = "mySystem";
const char love[] = "love";

namespace EventECS
{
	std::queue<std::string> EventECS::ECS::logs;

	int luaopen_logFunction(lua_State* L)
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
				ECS::logs.emplace(log);
			}
		}

		return 0;
	}

	ECS::ECS(ECSType type) : L(luaL_newstate()), initialized(false), type(type)
	{

	}

	ECS::~ECS()
	{
		Quit();
		initialized = false;

		lua_close(L);
	}

	void ECS::Quit()
	{
		CheckInitialized();

		if (type == ECSType::LOVE)
		{
			lua_getglobal(L, love);
			lua_getfield(L, -1, "event");
			lua_getfield(L, -1, "quit");
			if (lua_pcall(L, 0, 0, 0) != 0)
			{
				while (this->LoveUpdate());
			}
		}
	}

	bool ECS::Initialize(const char* executablePath, const char* identity)
	{
		if (!initialized)
		{
			while (!logs.empty())
			{
				logs.pop();
			}

			luaL_openlibs(L);
			initialized = true;

			Require("eventecs", nullptr);

			switch (type)
			{
			case ECSType::LOVE:
				Require(love, love);
				Require("loveSystem", System);
				break;
			default:
				Require("system", System);
				break;
			}

			lua_getglobal(L, System);
			lua_pushstring(L, identity);
			lua_pushstring(L, executablePath);
			if (lua_pcall(L, 2, 1, 0) == 0)
			{
				lua_setglobal(L, mySystem);
				lua_pop(L, 1);
				initialized = true;

				lua_pushcfunction(L, luaopen_logFunction);
				lua_setglobal(L, "Log");
			}
			else
			{
				initialized = false;
				throw std::exception(lua_tostring(L, -1));
			}
		}
		return initialized;
	}

	void ECS::CheckInitialized()
	{
		if (!initialized)
		{
			throw std::exception("Called function when ECS was not intialized.");
		}
	}

	void ECS::Require(const char* modName, const char* globalName)
	{
		CheckInitialized();

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

	void ECS::SetFunction(const char* funcName)
	{
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, funcName);
		lua_pushvalue(L, -2);
	}

	void ECS::FindEntity(int entityID)
	{
		SetFunction("findEntity");
		lua_pushnumber(L, entityID);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::FindComponent(int entityID, int componentID)
	{
		FindEntity(entityID);
		lua_getfield(L, -1, "findComponent");
		lua_pushvalue(L, -2);
		lua_pushnumber(L, componentID);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::RegisterComponent(const char* modName, bool replace)
	{
		CheckInitialized();

		lua_getglobal(L, "require");
		lua_pushstring(L, modName);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			lua_setglobal(L, "Component");

			lua_getglobal(L, mySystem);
			lua_getfield(L, -1, replace ? "replaceComponent" : "registerComponent");
			lua_pushnumber(L, -2);
			lua_getglobal(L, "Component");
			if (lua_pcall(L, 2, 0, 0) == 0)
			{
				lua_pushnil(L);
				lua_setglobal(L, "Component");
				return;
			}
		}
		throw std::exception(lua_tostring(L, -1));
	}

	std::string ECS::AddEntity()
	{
		CheckInitialized();

		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, "createEntity");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			lua_getfield(L, -1, "serialize");
			lua_pushvalue(L, -2);
			if (lua_pcall(L, 1, 1, 0) == 0)
			{
				std::string rval(lua_tostring(L, -1));
				lua_pop(L, 1);
				return rval;
			}
		}
		throw std::exception(lua_tostring(L, -1));
	}

	int ECS::RemoveEntity(int entityID)
	{
		CheckInitialized();

		SetFunction("removeEntity");
		lua_pushnumber(L, entityID);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			int entitesRemoved = static_cast<int>(lua_tonumber(L, -1));
			lua_pop(L, 1);
			return entitesRemoved;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	int ECS::DispatchEvent(const char* eventName)
	{
		CheckInitialized();

		SetFunction("dispatchEvent");
		lua_pushstring(L, eventName);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			int handles = static_cast<int>(lua_tonumber(L, -1));
			lua_pop(L, 1);
			return handles;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	void ECS::AddComponent(int entityID, const char* componentName)
	{
		CheckInitialized();

		FindEntity(entityID);
		lua_getfield(L, -1, "addComponent");
		lua_pushvalue(L, -2);
		lua_pushstring(L, componentName);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
		lua_pop(L, 1);
	}

	void ECS::AddComponents(int entityID, std::list<std::string> componentNames)
	{
		CheckInitialized();

		FindEntity(entityID);
		lua_getfield(L, -1, "addComponents");
		lua_pushvalue(L, -2);
		for (auto componentName : componentNames)
		{
			lua_pushstring(L, componentName.c_str());
		}

		const int size = componentNames.size();
		if (lua_pcall(L, size + 1, size, 0) == 0)
		{
			while(lua_gettop(L))
			{
				lua_pop(L, 1);
			}
			return;
		}

		throw std::exception(lua_tostring(L, -1));
	}

	bool ECS::RemoveComponent(int entityID, int componentID)
	{
		CheckInitialized();

		FindComponent(entityID, componentID);
		lua_getfield(L, -1, "remove");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			bool rval = static_cast<bool>(lua_toboolean(L, -1));
			lua_pop(L, 1);
			return rval;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	std::string ECS::Serialize()
	{
		CheckInitialized();

		SetFunction("serialize");
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			std::string rval(lua_tostring(L, -1));
			lua_pop(L, 1);
			return rval;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	std::string ECS::SerializeEntity(int entityID)
	{
		CheckInitialized();

		FindEntity(entityID);
		lua_getfield(L, -1, "serialize");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			std::string rval(lua_tostring(L, -1));
			lua_pop(L, 1);
			return rval;
		}

		throw std::exception(lua_tostring(L, -1));
	}

	std::string ECS::SerializeComponent(int entityID, int componentID)
	{
		CheckInitialized();

		FindComponent(entityID, componentID);
		lua_getfield(L, -1, "serilize");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			std::string rval(lua_tostring(L, -1));
			lua_pop(L, 1);
			return rval;
		}

		throw std::exception(lua_tostring(L, -1));
	}

	bool ECS::DoLoveUpdate(bool throwException)
	{
		SetFunction("run");
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			bool rval = static_cast<bool>(lua_toboolean(L, -1));
			lua_pop(L, 1);
			return rval;
		}
		if (throwException)
		{
			throw std::exception(lua_tostring(L, -1));
		}
		return false;
	}

	bool ECS::LoveUpdate()
	{
		CheckInitialized();

		if (type != ECSType::LOVE)
		{
			throw std::exception("Not Love System. Can't call update");
		}
		return DoLoveUpdate(true);
	}

	bool ECS::GetLog(std::string& log)
	{
		if (!logs.empty())
		{
			log = logs.front();
			logs.pop();
			return true;
		}
		return false;
	}

	void ECS::SetFrameRate(int fps)
	{
		lua_getglobal(L, mySystem);
		lua_pushstring(L, "frameRate");
		lua_pushnumber(L, fps);
		lua_settable(L, -3);
	}

	int ECS::GetFrameRate() const 
	{
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, "frameRate");
		auto value = luaL_optnumber(L, -1, 0);
		lua_pop(L, 1);
		return static_cast<int>(value);
	}
}