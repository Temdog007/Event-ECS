// Event-ECS-Lib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

#include "compiledCode.h"

int preload(lua_State* L, lua_CFunction f, const char* name);
int preloadClassFactory(lua_State* L);
int preloadComponent(lua_State* L);
int preloadEntity(lua_State* L);
int preloadSystem(lua_State* L);
int loadBuffer(lua_State* L, const unsigned char* buffer, const size_t size, const char* name);

int luaopen_eventecs(lua_State* L)
{
	preload(L, preloadClassFactory, "classFactory");
	preload(L, preloadComponent, "component");
	preload(L, preloadEntity, "entity");
	preload(L, preloadSystem, "system");
	return 0;
}

int preload(lua_State* L, lua_CFunction f, const char* name)
{
	lua_getglobal(L, "package");
	lua_getfield(L, -1, "preload");
	lua_pushcfunction(L, f);
	lua_setfield(L, -2, name);
	lua_pop(L, 2);
	return 0;
}

int preloadClassFactory(lua_State* L)
{
	loadBuffer(L, CLASSFACTORY, sizeof(CLASSFACTORY), "classFactory.lua");
	return 1;
}

int preloadComponent(lua_State* L)
{
	loadBuffer(L, COMPONENT, sizeof(COMPONENT), "component.lua");
	return 1;
}

int preloadEntity(lua_State* L) 
{
	loadBuffer(L, ENTITY, sizeof(ENTITY), "entity.lua");
	return 1;
}

int preloadSystem(lua_State* L) 
{
	loadBuffer(L, SYSTEM, sizeof(SYSTEM), "system.lua");
	return 1;
}

int loadBuffer(lua_State* L, const unsigned char* buffer, const size_t size, const char* name)
{
	if (luaL_loadbuffer(L, (const char*)buffer, size, name) == 0)
	{
		lua_call(L, 0, 1);
	}
	return 1;
}
