// Event-ECS-Lib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

int luaopen_eventecs(lua_State* L)
{
	lua_newtable(L);

	#include "compiledCode.h"

	return 1;
}
