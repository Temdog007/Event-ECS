// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers

#include <exception>
#include <string>
#include <list>
#include <map>
#include <queue>
#include <set>

extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"
}

#include "ECS.h"
#include "ECSMap.h"

namespace EventECS
{
	extern void luax_call(lua_State* L, int nargs, int nresults);

	extern bool luax_toboolean(lua_State* L, int idx);

	extern void luax_pushboolean(lua_State* L, bool b);

	extern void stackDump(lua_State* L);
}