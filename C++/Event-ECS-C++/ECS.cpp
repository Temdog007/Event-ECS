#include "stdafx.h"

const char System[] = "System";
const char LoveSystem[] = "LoveSystem";
const char mySystem[] = "mySystem";
const char love[] = "love";

namespace EventECS
{
	void(*ECS::logHandler)(const char* log);

	ECS::ECS() : L(luaL_newstate()), loveInitialized(false), disposed(false)
	{
		luaL_openlibs(L);

		Require("eventecs", nullptr);
		Require("system", System);

		lua_getglobal(L, System);
		lua_call(L, 0, 1);
		lua_setglobal(L, mySystem);

		lua_getglobal(L, mySystem);
		if (lua_isnil(L, -1))
		{
			throw std::exception("Failed to create global Entity Component System");
		}
		lua_pop(L, 1);

		lua_pushcfunction(L, luaopen_logFunction);
		lua_setglobal(L, "Log");
	}

	ECS::~ECS()
	{
		if (!disposed) 
		{
			Quit();

			lua_close(L);

			disposed = true;
		}
	}

	void ECS::Quit()
	{
		if (loveInitialized)
		{
			lua_settop(L, 0);
			lua_getglobal(L, love);
			lua_getfield(L, -1, "event");
			lua_getfield(L, -1, "quit");
			if (lua_pcall(L, 0, 0, 0) != 0)
			{
				while (this->LoveUpdate(false));
			}
			loveInitialized = false;
		}
	}

	bool ECS::InitializeLove(const char* executablePath, const char* identity)
	{
		Require(love, love);
		Require("loveSystem", LoveSystem);

		lua_getglobal(L, LoveSystem);
		lua_pushstring(L, identity);
		lua_pushstring(L, executablePath);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			lua_setglobal(L, mySystem);
		}
		else
		{
			throw std::exception(lua_tostring(L, -1));
		}
		loveInitialized = true;

		return loveInitialized;
	}

	void ECS::Require(const char* modName, const char* globalName)
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

	void ECS::SetFunction(const char* funcName) const
	{
		if (disposed)
		{
			throw std::exception("Lua state has been disposed");
		}

		lua_settop(L, 0);
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, funcName);
		lua_pushvalue(L, -2);
	}

	void ECS::FindEntity(int entityID) const
	{
		SetFunction("findEntity");
		lua_pushnumber(L, entityID);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::FindComponent(int entityID, int componentID) const
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
		lua_settop(L, 0);
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
		SetFunction("createEntity");
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

	bool ECS::RemoveEntity(int entityID)
	{
		FindEntity(entityID);
		lua_getfield(L, -1, "remove");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			bool entitesRemoved = static_cast<bool>(lua_toboolean(L, -1));
			lua_pop(L, 1);
			return entitesRemoved;
		}
		throw std::exception(lua_tostring(L, -1));
	}

	int ECS::DispatchEvent(const char* eventName)
	{
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

	void ECS::AddComponents(int entityID, const std::list<std::string>& componentNames)
	{
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

	#pragma region Getter/Setters

	#pragma region System
	void ECS::SetSystemBool(const char* key, bool value)
	{
		lua_getglobal(L, mySystem);
		lua_pushstring(L, key);
		lua_pushboolean(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetSystemNumber(const char* key, lua_Number value)
	{
		lua_getglobal(L, mySystem);
		lua_pushstring(L, key);
		lua_pushnumber(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetSystemString(const char* key, const char* value)
	{
		lua_getglobal(L, mySystem);
		lua_pushstring(L, key);
		lua_pushstring(L, value);
		lua_settable(L, -3);
	}

	bool ECS::GetSystemBool(const char* key) const
	{
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, key);
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	lua_Number ECS::GetSystemNumber(const char* key) const
	{
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, key);
		return lua_tonumber(L, -1);
	}

	const char* ECS::GetSystemString(const char* key) const
	{
		lua_getglobal(L, mySystem);
		lua_getfield(L, -1, key);
		return lua_tostring(L, -1);
	}
	#pragma endregion

	#pragma region Entity
	void ECS::SetEntityBool(int entityID, const char* key, bool value)
	{
		FindEntity(entityID);
		lua_pushstring(L, key);
		lua_pushboolean(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetEntityNumber(int entityID, const char* key, lua_Number value)
	{
		FindEntity(entityID);
		lua_pushstring(L, key);
		lua_pushnumber(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetEntityString(int entityID, const char* key, const char* value)
	{
		FindEntity(entityID);
		lua_pushstring(L, key);
		lua_pushstring(L, value);
		lua_settable(L, -3);
	}

	bool ECS::GetEntityBool(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	lua_Number ECS::GetEntityNumber(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		return lua_tonumber(L, -1);
	}

	const char* ECS::GetEntityString(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		return lua_tostring(L, -1);
	}
	
	#pragma endregion

	#pragma region Component

	void ECS::SetEnabled(int entityID, int componentID, bool enabled)
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, "setEnabled");
		lua_pushvalue(L, -2);
		lua_pushboolean(L, static_cast<int>(enabled));
		if (lua_pcall(L, 2, 0, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::SetComponentBool(int entityID, int componentID, const char* key, bool value)
	{
		FindComponent(entityID, componentID);
		lua_pushstring(L, key);
		lua_pushboolean(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetComponentNumber(int entityID, int componentID, const char* key, lua_Number value)
	{
		FindComponent(entityID, componentID);
		lua_pushstring(L, key);
		lua_pushnumber(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetComponentString(int entityID, int componentID, const char* key, const char* value)
	{
		FindComponent(entityID, componentID);
		lua_pushstring(L, key);
		lua_pushstring(L, value);
		lua_settable(L, -3);
	}

	bool ECS::IsEnabled(int entityID, int componentID) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, "isEnabled");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	bool ECS::GetComponentBool(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	lua_Number ECS::GetComponentNumber(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		return lua_tonumber(L, -1);
	}

	const char* ECS::GetComponentString(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		return lua_tostring(L, -1);
	}
	#pragma endregion

	#pragma endregion

	#pragma region Serialization
	std::string ECS::Serialize()
	{
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
	#pragma endregion

	#pragma region LOVE
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

	bool ECS::LoveUpdate(bool throwException)
	{
		if (!loveInitialized)
		{
			throw std::exception("LOVE not initialized. Can't call LOVE update");
		}
		return DoLoveUpdate(throwException);
	}

	void ECS::SetLogHandler(void(*func)(const char*))
	{
		ECS::logHandler = func;
	}

	int ECS::luaopen_logFunction(lua_State* L)
	{
		auto logHandler = ECS::logHandler;
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
	#pragma endregion
}