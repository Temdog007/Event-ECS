#include "stdafx.h"

namespace EventECS
{
	ECS::ECS() : L(nullptr), idx(LUA_REFNIL) {}

	ECS::ECS(lua_State* pL) : L(pL), idx(luaL_ref(L, LUA_REGISTRYINDEX))
	{

	}

	ECS::ECS(lua_State* pL, int pIdx) : L(pL), idx(pIdx)
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
		lua_settop(L, 0);
		lua_rawgeti(L, LUA_REGISTRYINDEX, idx); // push system
	}

	void ECS::SetFunction(const char* funcName) const
	{
		SetSystem(); // stack has system
		lua_getfield(L, -1, funcName); // push system function
		lua_pushvalue(L, -2); // push system (system function system)
		lua_remove(L, -3); // remove the first system (function system)
	}

	void ECS::SetEntityFunction(int entityID, const char* funcName) const
	{
		FindEntity(entityID); // stack has entity
		lua_getfield(L, -1, funcName); // push entity function (entity function)
		lua_pushvalue(L, -2); // push entity (entity function entity)
		lua_remove(L, -3); // remove first entity (function entity)
	}

	void ECS::SetComponentFunction(int entityID, int componentID, const char* funcName) const
	{
		FindComponent(entityID, componentID); // stack has component
		lua_getfield(L, -1, funcName); // push component function (component function)
		lua_pushvalue(L, -2); // push component (component function component)
		lua_remove(L, -3); // remove first component (function component)
	}

	void ECS::FindEntity(int entityID) const
	{
		SetFunction("findEntity"); // stack has function
		lua_pushnumber(L, entityID); // push number
		luax_call(L, 2, 1); // call function (entity)
	}

	void ECS::FindComponent(int entityID, int componentID) const
	{
		FindEntity(entityID); // stack has entity
		lua_getfield(L, -1, "findComponent"); // push entity function
		lua_pushvalue(L, -2); // push entity (entity function entity)
		lua_remove(L, -3); // remove first entity (function entity)
		lua_pushnumber(L, componentID); // push number
		luax_call(L, 2, 1); // call function component
	}

	std::string ECS::AddEntity()
	{
		SetFunction("createEntity");
		luax_call(L, 1, 1);

		lua_getfield(L, -1, "serialize");
		lua_pushvalue(L, -2);
		lua_remove(L, -3);
		luax_call(L, 1, 1);

		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}

	bool ECS::RemoveEntity(int entityID)
	{
		SetEntityFunction(entityID, "remove");
		luax_call(L, 1, 1);
		bool entitesRemoved = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return entitesRemoved;
	}

	int ECS::DispatchEvent(const char* eventName)
	{
		SetFunction("dispatchEvent");
		lua_pushstring(L, eventName);
		luax_call(L, 2, 1);
		int handles = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return handles;
	}

	int ECS::DispatchEvent(const char* eventName, int argRef)
	{
		SetFunction("dispatchEvent");
		lua_pushstring(L, eventName);
		lua_rawgeti(L, LUA_REGISTRYINDEX, argRef);
		luax_call(L, 3, 1);
		int handles = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return handles;
	}

	int ECS::DispatchEvent(int entityID, const char* eventName)
	{
		SetEntityFunction(entityID, "dispatchEvent");
		lua_pushstring(L, eventName);
		luax_call(L, 2, 1);
		int handles = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return handles;
	}

	int ECS::DispatchEvent(int entityID, const char* eventName, int argRef)
	{
		SetEntityFunction(entityID, "dispatchEvent");
		lua_pushstring(L, eventName);
		lua_rawgeti(L, LUA_REGISTRYINDEX, argRef);
		luax_call(L, 3, 1);
		int handles = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return handles;
	}

	void ECS::AddComponent(int entityID, const char* componentName)
	{
		SetEntityFunction(entityID, "addComponent");
		lua_pushstring(L, componentName);
		luax_call(L, 2, 0);
	}

	void ECS::AddComponents(int entityID, const std::list<std::string>& componentNames)
	{
		SetEntityFunction(entityID, "addComponents");
		for (auto componentName : componentNames)
		{
			lua_pushstring(L, componentName.c_str());
		}

		const int size = componentNames.size();
		luax_call(L, size + 1, 0);
	}

	bool ECS::RemoveComponent(int entityID, int componentID)
	{
		SetComponentFunction(entityID, componentID, "remove");
		luax_call(L, 1, 1);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	#pragma region Getter/Setters

	#pragma region System
	void ECS::SetSystemBool(const char* key, bool value)
	{
		SetSystem();
		luax_pushboolean(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetSystemNumber(const char* key, lua_Number value)
	{
		SetSystem();
		lua_pushnumber(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetSystemString(const char* key, const char* value)
	{
		SetSystem();
		lua_pushstring(L, value);
		lua_setfield(L, -2, key);
	}

	bool ECS::GetSystemBool(const char* key) const
	{
		SetSystem();
		lua_getfield(L, -1, key);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	lua_Number ECS::GetSystemNumber(const char* key) const
	{
		SetSystem();
		lua_getfield(L, -1, key);
		lua_Number rval = lua_tonumber(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	std::string ECS::GetSystemString(const char* key) const
	{
		SetSystem();
		lua_getfield(L, -1, key);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	#pragma endregion

	#pragma region Entity
	void ECS::SetEntityBool(int entityID, const char* key, bool value)
	{
		FindEntity(entityID);
		luax_pushboolean(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetEntityNumber(int entityID, const char* key, lua_Number value)
	{
		FindEntity(entityID);
		lua_pushnumber(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetEntityString(int entityID, const char* key, const char* value)
	{
		FindEntity(entityID);
		lua_pushstring(L, value);
		lua_setfield(L, -2, key);
	}

	bool ECS::GetEntityBool(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	lua_Number ECS::GetEntityNumber(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		lua_Number rval = lua_tonumber(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	std::string ECS::GetEntityString(int entityID, const char* key) const
	{
		FindEntity(entityID);
		lua_getfield(L, -1, key);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	
	#pragma endregion

	#pragma region Component

	void ECS::SetSystemEnabled(bool enabled)
	{
		SetFunction("setEnabled");
		luax_pushboolean(L, enabled);
		luax_call(L, 2, 0);
	}

	void ECS::SetEntityEnabled(int entityID, bool enabled)
	{
		SetEntityFunction(entityID, "setEnabled");
		luax_pushboolean(L, enabled);
		luax_call(L, 2, 0);
	}

	void ECS::SetComponentEnabled(int entityID, int componentID, bool enabled)
	{
		SetComponentFunction(entityID, componentID, "setEnabled");
		luax_pushboolean(L, enabled);
		luax_call(L, 2, 0);
	}

	void ECS::SetComponentBool(int entityID, int componentID, const char* key, bool value)
	{
		FindComponent(entityID, componentID);
		luax_pushboolean(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetComponentNumber(int entityID, int componentID, const char* key, lua_Number value)
	{
		FindComponent(entityID, componentID);
		lua_pushnumber(L, value);
		lua_setfield(L, -2, key);
	}

	void ECS::SetComponentString(int entityID, int componentID, const char* key, const char* value)
	{
		FindComponent(entityID, componentID);
		lua_pushstring(L, value);
		lua_setfield(L, -2, key);
	}

	bool ECS::IsSystemEnabled() const
	{
		SetFunction("isEnabled");
		luax_call(L, 1, 1);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	bool ECS::IsEntityEnabled(int entityID) const
	{
		SetEntityFunction(entityID, "isEnabled");
		luax_call(L, 1, 1);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	bool ECS::IsComponentEnabled(int entityID, int componentID) const
	{
		SetComponentFunction(entityID, componentID, "isEnabled");
		luax_call(L, 1, 1);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	bool ECS::GetComponentBool(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		bool rval = luax_toboolean(L, -1);
		lua_pop(L, 1);
		return rval;
	}

	lua_Number ECS::GetComponentNumber(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		return lua_tonumber(L, -1);
	}

	std::string ECS::GetComponentString(int entityID, int componentID, const char* key) const
	{
		FindComponent(entityID, componentID);
		lua_getfield(L, -1, key);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	#pragma endregion

	#pragma endregion

	#pragma region Serialization
	std::string ECS::Serialize() const
	{
		SetFunction("serialize");
		luax_call(L, 1, 1);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}

	std::string ECS::SerializeEntity(int entityID) const
	{
		SetEntityFunction(entityID, "serialize");
		luax_call(L, 1, 1);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}

	std::string ECS::SerializeComponent(int entityID, int componentID) const
	{
		SetComponentFunction(entityID, componentID, "serialize");
		luax_call(L, 1, 1);
		std::string rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	#pragma endregion
}