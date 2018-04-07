#include "stdafx.h"

ECS::ECS() : L(nullptr), initialized(false)
{
	L = luaL_newstate();
	luaL_openlibs(L);
}

ECS::~ECS()
{
	lua_close(L);
}

void ECS::Initialize()
{
	if (initialized)
	{
		return;
	}

	loadModule("love", 1); // Loading  love automatically creates a global called 'love'
	lua_pop(L, 1); //Pop 'love' from stack

	loadModule("eventecs", 0); // Load all of the ECS modules

	loadModule("system", 1); // Get System class table
	safeCall(0, 1); // Call System class table to create an instance
	lua_setglobal(L, "System"); // Set System instance as a global

	initialized = true;
}

void ECS::loadModule(const char* name, int rvals)
{
	lua_getglobal(L, "require");
	lua_pushstring(L, name);
	safeCall(1, rvals);
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