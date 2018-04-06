#include "stdafx.h"

ECS::ECS()
{
	L = luaL_newstate();
	luaL_openlibs(L);

	loadModule("love");
	lua_pop(L, 1);

	loadModule("eventecs");
	lua_setglobal(L, "System");
}

ECS::~ECS()
{
	lua_close(L);
}

void ECS::loadModule(const char* name)
{
	lua_getglobal(L, "require");
	lua_pushstring(L, name);
	if (lua_pcall(L, 1, 1, 0) != 0)
	{
		throw new std::exception(lua_tostring(L, -1));
	}
}

void ECS::dispatchEvent(const char* name, int& handled)
{
	lua_getglobal(L, "System");
	lua_getfield(L, -1, "dispatchEvent");
	lua_setglobal(L, "System");
	lua_pushstring(L, name);
	if (lua_pcall(L, 2, 1, 0) != 0)
	{
		throw new std::exception(lua_tostring(L, -1));
	}
	handled = static_cast<int>(lua_tonumber(L, -1));
	lua_pop(L, 1);
}

void ECS::getState(const char*& data) const
{
	lua_getglobal(L, "System");
	lua_pushstring(L, "encode");
	lua_setglobal(L, "System");
	if (lua_pcall(L, 1, 1, 0) != 0)
	{
		throw new std::exception(lua_tostring(L, -1));
	}
	data = lua_tostring(L, -1);
	lua_pop(L, 1);
}