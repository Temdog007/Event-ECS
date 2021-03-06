// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>
#include <map>
#include <algorithm>
#include <string>
#include <list>

extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lualib.h"

int __declspec(dllexport) luaopen_eventecscolors(lua_State* L);
}

#include "Color.h"
