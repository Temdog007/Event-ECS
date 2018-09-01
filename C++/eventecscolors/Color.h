#pragma once

#include "stdafx.h"

struct Color
{
	const lua_Number r, g, b, a;

	Color() : r(1), g(1), b(1), a(1) {}
	Color(const lua_Number numbers[4]) : r(numbers[0]), g(numbers[1]), b(numbers[2]), a(numbers[3]) {}
	Color(lua_Number _r, lua_Number _g, lua_Number _b, lua_Number _a) : r(_r), g(_g), b(_b), a(_a) {}

	Color(const unsigned long& hex) :
		r(((hex & 0xff000000) >> 24) / 255.0),
		g(((hex & 0xff0000) >> 16) / 255.0),
		b(((hex & 0xff00) >> 8) / 255.0),
		a((hex & 0xff) / 255.0) {}

	unsigned long ToLong() const
	{
		unsigned long rInt = ((unsigned long)floor(r * 255)) << 24;
		unsigned long gInt = ((unsigned long)floor(g * 255)) << 16;
		unsigned long bInt = ((unsigned long)floor(b * 255)) << 8;
		unsigned long aInt = (unsigned long)floor(a * 255);
		return rInt | gInt | bInt | aInt;
	}

	int lua_pushColor(lua_State* L) const
	{
		lua_pushnumber(L, r);
		lua_pushnumber(L, g);
		lua_pushnumber(L, b);
		lua_pushnumber(L, a);
		return 4;
	}

	int lua_createTable(lua_State* L) const
	{
		lua_newtable(L);
		lua_pushinteger(L, 1);
		lua_pushnumber(L, r);
		lua_settable(L, -3);

		lua_pushinteger(L, 2);
		lua_pushnumber(L, g);
		lua_settable(L, -3);

		lua_pushinteger(L, 3);
		lua_pushnumber(L, b);
		lua_settable(L, -3);

		lua_pushinteger(L, 4);
		lua_pushnumber(L, a);
		lua_settable(L, -3);
		return 1;
	}

	bool operator==(const Color& c) const
	{
		return r == c.r && g == c.g && b == c.b && a == c.a;
	}

	bool operator!=(const Color& c) const
	{
		return !(*this == c);
	}

	bool operator<(const Color& c) const
	{
		return ToLong() < c.ToLong();
	}

	bool operator<=(const Color& c) const
	{
		return ToLong() <= c.ToLong();
	}

	bool operator>(const Color& c) const
	{
		return ToLong() > c.ToLong();
	}

	bool operator>=(const Color& c) const
	{
		return ToLong() >= c.ToLong();
	}
};