#include "stdafx.h"

ECS::ECS() : L(luaL_newstate()), initialized(false)
{
}

ECS::~ECS()
{
	lua_close(L);
}

void ECS::Init()
{
	if (initialized)
	{
		return;
	}

	luaL_openlibs(L);

	loadModule("love");
	lua_pop(L, 1);

	loadModule("eventecs");
	lua_setglobal(L, "System");

	initialized = true;
}

void ECS::loadModule(const char* name)
{
	lua_getglobal(L, "require");
	lua_pushstring(L, name);
	safeCall(1,1);
}

void ECS::safeCall(int nargs, int rvals)
{
	if (lua_pcall(L, nargs, rvals, 0) != 0)
	{
		const char * err = lua_tostring(L, -1);
		lua_pop(L, 1);
		throw std::exception(err);
	}
}

void ECS::dispatchEvent(const char* name, int& handled)
{
	if (!initialized)
	{
		throw std::exception("ECS not initialized");
		return;
	}

	lua_getglobal(L, "System");
	lua_getfield(L, -1, "dispatchEvent");
	lua_setglobal(L, "System");
	lua_pushstring(L, name);

	safeCall(2, 1);

	handled = static_cast<int>(lua_tonumber(L, -1));
	lua_pop(L, 1);
}

void ECS::getState(const char*& data)
{
	if (!initialized)
	{
		throw std::exception("ECS not initialized");
		return;
	}

	lua_getglobal(L, "System");
	lua_pushstring(L, "encode");
	lua_setglobal(L, "System");

	safeCall(1, 1);
	
	data = lua_tostring(L, -1);
	lua_pop(L, 1);
}