#include "stdafx.h"

namespace EventECS
{
	ECS::ECS() : L(nullptr), idx(LUA_REFNIL) {}

	ECS::ECS(lua_State* pL) : L(pL), idx(luaL_ref(L, -1))
	{

	}

	ECS::ECS(lua_State* pL, int pIdx) : L(pL), idx(pIdx)
	{
		
	}

	ECS::ECS(const ECS& ecs) : L(ecs.L), idx(ecs.idx)
	{

	}

	ECS::~ECS()
	{
		if (idx != LUA_REFNIL)
		{
			luaL_unref(L, LUA_REGISTRYINDEX, idx);
		}
	}

	void ECS::SetSystem() const 
	{
		lua_rawgeti(L, LUA_REGISTRYINDEX, idx);
	}

	void ECS::SetFunction(const char* funcName) const
	{
		SetSystem();
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
		SetSystem();
		lua_pushstring(L, key);
		lua_pushboolean(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetSystemNumber(const char* key, lua_Number value)
	{
		SetSystem();
		lua_pushstring(L, key);
		lua_pushnumber(L, value);
		lua_settable(L, -3);
	}

	void ECS::SetSystemString(const char* key, const char* value)
	{
		SetSystem();
		lua_pushstring(L, key);
		lua_pushstring(L, value);
		lua_settable(L, -3);
	}

	bool ECS::GetSystemBool(const char* key) const
	{
		SetSystem();
		lua_getfield(L, -1, key);
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	lua_Number ECS::GetSystemNumber(const char* key) const
	{
		SetSystem();
		lua_getfield(L, -1, key);
		return lua_tonumber(L, -1);
	}

	const char* ECS::GetSystemString(const char* key) const
	{
		SetSystem();
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

	void ECS::SetSystemEnabled(bool enabled)
	{
		SetSystem();
		lua_getfield(L, -1, "setEnabled");
		lua_pushvalue(L, -2);
		lua_pushboolean(L, static_cast<int>(enabled));
		if (lua_pcall(L, 2, 0, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::SetEntityEnabled(int entityID, bool enabled)
	{
		FindEntity(entityID);
		lua_getfield(L, -1, "setEnabled");
		lua_pushvalue(L, -2);
		lua_pushboolean(L, static_cast<int>(enabled));
		if (lua_pcall(L, 2, 0, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}

	void ECS::SetComponentEnabled(int entityID, int componentID, bool enabled)
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

	bool ECS::IsSystemEnabled() const
	{
		SetSystem();
		lua_getfield(L, -1, "isEnabled");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	bool ECS::IsEntityEnabled(int entityID) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, "isEnabled");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 2, 1, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
		return static_cast<bool>(lua_toboolean(L, -1));
	}

	bool ECS::IsComponentEnabled(int entityID, int componentID) const
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
	std::string ECS::Serialize() const
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

	std::string ECS::SerializeEntity(int entityID) const
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

	std::string ECS::SerializeComponent(int entityID, int componentID) const
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
}