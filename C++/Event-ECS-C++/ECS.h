#pragma once

#include "stdafx.h"

namespace EventECS 
{
	class ECS
	{
	private:
		lua_State * L;

		bool loveInitialized;

		bool autoUpdate;

		static void(*logHandler)(const char* log);
		static int luaopen_logFunction(lua_State* L);

		void Require(const char* moduleName, const char* globalName);

		void SetFunction(const char* funcName) const;

		void FindEntity(int entityID) const;

		void FindComponent(int entityID, int componentID) const;

		bool DoLoveUpdate(bool throwException);

		void Quit();

	public:
		ECS();
		virtual ~ECS();

		ECS(const ECS&) = delete;
		ECS& operator=(const ECS&) = delete;

		bool InitializeLove(const char* executablePath, const char* identity);

		std::string AddEntity();
		bool RemoveEntity(int entityID);

		int DispatchEvent(const char* eventName);
		void RegisterComponent(const char* moduleName, bool replace = false);

		void AddComponent(int entityID, const char* componentName);
		void AddComponents(int entityID, std::list<std::string> componentNames);

		bool RemoveComponent(int entityID, int componentID);

		void SetSystemBool(const char* key, bool value);
		void SetSystemNumber(const char* key, lua_Number value);
		void SetSystemString(const char* key, const char* value);

		void SetEntityBool(int entityID, const char* key, bool value);
		void SetEntityNumber(int entityID, const char* key, lua_Number value);
		void SetEntityString(int entityID, const char* key, const char* value);

		void SetComponentBool(int entityID, int componentID, const char* key, bool value);
		void SetComponentNumber(int entityID, int componentID, const char* key, lua_Number value);
		void SetComponentString(int entityID, int componentID, const char* key, const char* value);

		bool GetSystemBool(const char* key) const;
		lua_Number GetSystemNumber(const char* key) const;
		const char* GetSystemString(const char* key) const;

		bool GetEntityBool(int entityID, const char* key) const;
		lua_Number GetEntityNumber(int entityID, const char* key) const;
		const char* GetEntityString(int entityID, const char* key) const;

		bool GetComponentBool(int entityID, int componentID, const char* key) const;
		lua_Number GetComponentNumber(int entityID, int componentID, const char* key) const;
		const char* GetComponentString(int entityID, int componentID, const char* key) const;

		std::string Serialize();
		std::string SerializeEntity(int entityID);
		std::string SerializeComponent(int entityID, int componentID);

		bool LoveUpdate();

		static void SetLogHandler(void(*) (const char*));
	};
}