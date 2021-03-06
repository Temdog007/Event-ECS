// eventecsserver.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include "compiledCode.h"

int preload(lua_State* L, lua_CFunction f, const char* name)
{
	lua_getglobal(L, "package");
	lua_getfield(L, -1, "preload");
	lua_pushcfunction(L, f);
	lua_setfield(L, -2, name);
	lua_pop(L, 2);
	return 0;
}

int luaopen_eventecsserver(lua_State* L)
{
	for (auto pair : funcMap)
	{
		preload(L, pair.second, pair.first);
	}
	return 0;
}