#pragma once

#include "stdafx.h"

namespace EventECS 
{
	enum ECSType
	{
		NORMAL,
		LOVE
	};

	class ECS
	{
	private:
		friend int luaopen_logFunction(lua_State* L);

		lua_State * L;

		bool initialized;

		bool autoUpdate;

		const ECSType type;

		static bool canAddLog;
		static std::queue<std::string> logs;

		void Require(const char* moduleName, const char* globalName);

		void SetFunction(const char* funcName);

		void FindEntity(int entityID);

		void FindComponent(int entityID, int componentID);

		void CheckInitialized();

		bool DoLoveUpdate(bool throwException);

		void Quit();

	public:
		ECS(ECSType type = ECSType::NORMAL);
		virtual ~ECS();

		ECS() = delete;
		ECS(const ECS&) = delete;
		ECS& operator=(const ECS&) = delete;

		bool Initialize(const char* executablePath, const char* identity);

		std::string AddEntity();
		bool RemoveEntity(int entityID);

		int DispatchEvent(const char* eventName);
		void RegisterComponent(const char* moduleName, bool replace = false);

		void AddComponent(int entityID, const char* componentName);
		void AddComponents(int entityID, std::list<std::string> componentNames);

		bool RemoveComponent(int entityID, int componentID);

		void SetFrameRate(int fps);
		int GetFrameRate() const;

		std::string Serialize();
		std::string SerializeEntity(int entityID);
		std::string SerializeComponent(int entityID, int componentID);

		bool LoveUpdate();

		static bool GetLog(std::string& log);

		inline ECSType getType() const { return type; }
	};
}