#pragma once

#include "stdafx.h"

class ECS;

namespace EventECS
{
	class ECSMap
	{
		friend int w__broadcastevent(lua_State*);

	private:
		ECSMap();
		ECSMap(const char* initializerScript, size_t size);
		ECSMap(const char* initializerScript, size_t size, const char* executablePath, const char* identity);

		static ECSMap* instance;

		lua_State * L;

		bool disposed;

		bool loveInitialized;

		std::map<std::string, ECS> map;

		static void(*logHandler)(const char* log);
		static int lua_logFunction(lua_State* L);
		static int lua_broadcastEvent(lua_State* L);

		void SetDefaults();

		void LoadInitializerCode(const char* initializerCode, size_t size);

		bool InitializeLove(const char* executablePath, const char* identity);

		void Require(const char* moduleName, const char* globalName);

		void Quit();

	public:
		virtual ~ECSMap();

		ECSMap(const ECSMap&) = delete;
		ECSMap& operator=(const ECSMap&) = delete;

		void Reset();

		ECS& operator[](const char* name);
		const ECS& operator[](const char* name) const;

		void Add(const char* name);
		size_t Remove(const char* name);

		int BroadcastEvent(const char* eventName);

		bool UpdateLove();

		lua_Number DrawLove(bool skipSleep = false);

		std::list<std::string> Serialize() const;

		static ECSMap* CreateInstance();
		static ECSMap* CreateInstance(const char* initializerScript, size_t size);
		static ECSMap* CreateInstance(const char* initializerScript, size_t size, const char* executablePath, const char* identity);
		static void DeleteInstance();

		static void SetLogHandler(void(*) (const char*));
	};
}