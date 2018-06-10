// stdafx.cpp : source file that includes just the standard includes
// Event-ECS-C++.pch will be the pre-compiled header
// stdafx.obj will contain the pre-compiled type information

#include "stdafx.h"

namespace EventECS 
{
	bool luax_toboolean(lua_State* L, int idx)
	{
		return lua_toboolean(L, idx) != 0;
	}

	void luax_pushboolean(lua_State* L, bool b)
	{
		lua_pushboolean(L, b ? 1 : 0);
	}

	void stackDump(lua_State *L)
	{
		int i;
		int top = lua_gettop(L);
		for (i = 1; i <= top; i++)  /* repeat for each level */
		{
			int t = lua_type(L, i);
			switch (t)
			{

			case LUA_TSTRING:  /* strings */
				printf("`%s'", lua_tostring(L, i));
				break;

			case LUA_TBOOLEAN:  /* booleans */
				printf(luax_toboolean(L, i) ? "true" : "false");
				break;

			case LUA_TNUMBER:  /* numbers */
				printf("%g", lua_tonumber(L, i));
				break;

			default:  /* other values */
				printf("%s", lua_typename(L, t));
				break;

			}
			printf("  ");  /* put a separator */
		}
		printf("\n");  /* end the listing */
	}

	void luax_call(lua_State* L, int nargs, int nresults)
	{
#ifdef NDEBUG
		lua_call(L, nargs, nresults);
#else
		if (lua_pcall(L, nargs, nresults, 0) != 0)
		{
			throw std::exception(lua_tostring(L, -1));
		}
#endif
	}
}