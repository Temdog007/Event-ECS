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

		lua_State *const L;

		int updateRefs[2];

		bool disposed;

		bool dispatchingEvent;

		bool logEvents;

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

		void EmptyEventQueue();

		static void Log(const char* log, ...);

		struct Event
		{
			lua_State*const L;
			int ref;
			const char* name;

			Event(lua_State* pL, const char* pName, int pRef);
			~Event();
		};
		std::queue<Event> queue;

		std::set<std::string> eventsToIgnore;

	public:
		virtual ~ECSMap();

		ECSMap(const ECSMap&) = delete;
		ECSMap& operator=(const ECSMap&) = delete;

		int UpdateLove();

		void Reset();

		void Execute(const char* code);
		void Execute(const char* code, const char* systemName);

		ECS& operator[](const char* name);
		const ECS& operator[](const char* name) const;

		void SetLogEvents(bool value);
		bool IsLoggingEvents() const;

		void SetEventsToIgnore(const std::set<std::string>& events);
		bool IsIgnored(const std::string& ev) const;

		void Unregister(const char* modName);

		void BroadcastEventWithArgs(const char* eventName);
		void BroadcastEvent(const char* eventName, int argRef = LUA_REFNIL);

		std::string GetClassname(const char* systemName) const;

		bool HasSystem(const char * name) const;

		std::list<std::string> Serialize() const;

		static ECSMap* CreateInstance();
		static ECSMap* CreateInstance(const char* initializerScript, size_t size);
		static ECSMap* CreateInstance(const char* initializerScript, size_t size, const char* executablePath, const char* identity);
		static void DeleteInstance();

		static void SetLogHandler(void(*) (const char*));
	};
}