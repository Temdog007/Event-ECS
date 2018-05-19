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

		static ECSMap* instance;

		lua_State * L;

		bool disposed;

		bool loveInitialized;

		std::map<std::string, ECS> map;

		static void(*logHandler)(const char* log);
		static int lua_logFunction(lua_State* L);

		void Require(const char* moduleName, const char* globalName);

		void Quit();

	public:
		virtual ~ECSMap();

		ECSMap(const ECSMap&) = delete;
		ECSMap& operator=(const ECSMap&) = delete;

		ECS& operator[](const char* name);
		const ECS& operator[](const char* name) const;

		void Add(const char* name);
		size_t Remove(const char* name);

		bool InitializeLove(const char* executablePath, const char* identity);

		int BroadcastEvent(const char* eventName);

		void UpdateLove();

		lua_Number DrawLove();

		std::list<std::string> Serialize() const;

		static ECSMap* CreateInstance();
		static void DeleteInstance();

		static void SetLogHandler(void(*) (const char*));
	};
}