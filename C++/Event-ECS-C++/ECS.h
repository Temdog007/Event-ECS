#pragma once

#include "stdafx.h"

namespace EventECS 
{
	class ECS
	{
	private:
		lua_State * L;

		const int idx;

		void SetSystem() const;

		void SetFunction(const char* funcName) const;

		void SetEntityFunction(int entityID, const char* funcName) const;

		void SetComponentFunction(int entityID, int componentID, const char* funcName) const;

		void FindEntity(int entityID) const;

		void FindComponent(int entityID, int componentID) const;

	public:
		ECS();
		ECS(lua_State* L);
		ECS(lua_State* L, int pIdx);
		virtual ~ECS();
		
		ECS(const ECS&) = delete;
		ECS(ECS&& ecs) = delete;
		ECS& operator=(const ECS&) = delete;
		ECS& operator=(ECS&&) = delete;

		int GetIDX() const 
		{
			return idx;
		}

		std::string AddEntity();
		bool RemoveEntity(int entityID);

		int DispatchEvent(const char* eventName);
		int DispatchEvent(const char* eventName, int argRef);

		int DispatchEvent(int entityID, const char* eventName);
		int DispatchEvent(int entityID, const char* eventName, int argRef);

		void AddComponent(int entityID, const char* componentName);
		void AddComponents(int entityID, const std::list<std::string>& componentNames);

		bool RemoveComponent(int entityID, int componentID);

		bool IsSystemEnabled() const;
		void SetSystemEnabled(bool value);

		bool IsEntityEnabled(int entityID) const;
		void SetEntityEnabled(int entityID, bool value);

		bool IsComponentEnabled(int entityID, int componentID) const;
		void SetComponentEnabled(int entityID, int componentID, bool value);

		void SetSystemBool(const char* key, bool value);
		void SetSystemNumber(const char* key, lua_Number value);
		void SetSystemString(const char* key, const char* value);

		void SetEntityBool(int entityID, const char* key, bool value);
		void SetEntityNumber(int entityID, const char* key, lua_Number value);
		void SetEntityString(int entityID, const char* key, const char* value);

		void SetComponentBool(int entityID, int componentID, const char* key, bool value);
		void SetComponentNumber(int entityID, int componentID, const char* key, lua_Number value);
		void SetComponentNumber(int entityID, int componentID, int key, lua_Number value);
		void SetComponentString(int entityID, int componentID, const char* key, const char* value);

		bool GetSystemBool(const char* key) const;
		lua_Number GetSystemNumber(const char* key) const;
		std::string GetSystemString(const char* key) const;

		bool GetEntityBool(int entityID, const char* key) const;
		lua_Number GetEntityNumber(int entityID, const char* key) const;
		std::string GetEntityString(int entityID, const char* key) const;

		bool GetComponentBool(int entityID, int componentID, const char* key) const;
		lua_Number GetComponentNumber(int entityID, int componentID, const char* key) const;
		lua_Number GetComponentNumber(int entityID, int componentID, int key) const;
		std::string GetComponentString(int entityID, int componentID, const char* key) const;

		std::string Serialize() const;
		std::string SerializeEntity(int entityID) const;
		std::string SerializeComponent(int entityID, int componentID) const;
	};
}