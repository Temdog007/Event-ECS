#include "stdafx.h"

ECS::ECS() : L(luaL_newstate())
{
	luaL_openlibs(L);
}

ECS::~ECS()
{
	lua_close(L);
}

void ECS::Require(const char* modName, const char* globalName)
{
	lua_getglobal(L, "require");
	lua_pushstring(L, modName);
	if (lua_pcall(L, 1, 1, 0) == 0)
	{
		if (globalName != nullptr)
		{
			lua_setglobal(L, globalName);
		}
		else
		{
			lua_pop(L, 1);
		}
	}
	else 
	{
		throw std::exception(lua_tostring(L, -1));
	}
}

bool ECS::DoString(const char* code)
{
	return luaL_dostring(L, code);
}