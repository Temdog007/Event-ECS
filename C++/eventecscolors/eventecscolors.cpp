// eventecscolors.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"

namespace 
{
	struct nocase_compare
	{
		bool operator()(const char& c1, const char& c2) const 
		{
			return tolower(c1) < tolower(c2);
		}

		bool operator()(const std::string& s1, const std::string& s2) const
		{
			return std::lexicographical_compare(s1.begin(), s1.end(), s2.begin(), s2.end(), nocase_compare());
		}
	};

	std::map<std::string, Color, nocase_compare> colors
	{ 
	{ "midnightblue",{ 0.098039, 0.098039, 0.439216, 1.000000 } },
	{ "indianred",{ 0.803922, 0.360784, 0.360784, 1.000000 } },
	{ "purple",{ 0.501961, 0.000000, 0.501961, 1.000000 } },
	{ "antiqueBronze",{ 0.400000, 0.364706, 0.117647, 1.000000 } },
	{ "seagreen",{ 0.180392, 0.545098, 0.341176, 1.000000 } },
	{ "linen",{ 0.980392, 0.941176, 0.901961, 1.000000 } },
	{ "dodgerblue",{ 0.117647, 0.564706, 1.000000, 1.000000 } },
	{ "azure",{ 0.941176, 1.000000, 1.000000, 1.000000 } },
	{ "turquoise",{ 0.250980, 0.878431, 0.815686, 1.000000 } },
	{ "khaki",{ 0.941176, 0.901961, 0.549020, 1.000000 } },
	{ "mediumturquoise",{ 0.282353, 0.819608, 0.800000, 1.000000 } },
	{ "mediumorchid",{ 0.729412, 0.333333, 0.827451, 1.000000 } },
	{ "silver",{ 0.752941, 0.752941, 0.752941, 1.000000 } },
	{ "papayawhip",{ 1.000000, 0.937255, 0.835294, 1.000000 } },
	{ "darkslateblue",{ 0.282353, 0.239216, 0.545098, 1.000000 } },
	{ "tomato",{ 1.000000, 0.388235, 0.278431, 1.000000 } },
	{ "sienna",{ 0.627451, 0.321569, 0.176471, 1.000000 } },
	{ "dimgray",{ 0.411765, 0.411765, 0.411765, 1.000000 } },
	{ "darkorange",{ 1.000000, 0.549020, 0.000000, 1.000000 } },
	{ "darkviolet",{ 0.580392, 0.000000, 0.827451, 1.000000 } },
	{ "orangered",{ 1.000000, 0.270588, 0.000000, 1.000000 } },
	{ "royalblue",{ 0.254902, 0.411765, 0.882353, 1.000000 } },
	{ "darkslategray",{ 0.184314, 0.309804, 0.309804, 1.000000 } },
	{ "aliceblue",{ 0.941176, 0.972549, 1.000000, 1.000000 } },
	{ "springgreen",{ 0.000000, 1.000000, 0.498039, 1.000000 } },
	{ "bisque",{ 1.000000, 0.894118, 0.768627, 1.000000 } },
	{ "greenyellow",{ 0.678431, 1.000000, 0.184314, 1.000000 } },
	{ "bronze",{ 0.803922, 0.498039, 0.196078, 1.000000 } },
	{ "lemonchiffon",{ 1.000000, 0.980392, 0.803922, 1.000000 } },
	{ "forestgreen",{ 0.133333, 0.545098, 0.133333, 1.000000 } },
	{ "aqua",{ 0.000000, 1.000000, 1.000000, 1.000000 } },
	{ "red",{ 1.000000, 0.000000, 0.000000, 1.000000 } },
	{ "lime",{ 0.000000, 1.000000, 0.000000, 1.000000 } },
	{ "limegreen",{ 0.196078, 0.803922, 0.196078, 1.000000 } },
	{ "teal",{ 0.000000, 0.501961, 0.501961, 1.000000 } },
	{ "navajowhite",{ 1.000000, 0.870588, 0.678431, 1.000000 } },
	{ "darksalmon",{ 0.913725, 0.588235, 0.478431, 1.000000 } },
	{ "burlywood",{ 0.870588, 0.721569, 0.529412, 1.000000 } },
	{ "ghostwhite",{ 0.972549, 0.972549, 1.000000, 1.000000 } },
	{ "lavender",{ 0.901961, 0.901961, 0.980392, 1.000000 } },
	{ "oldlace",{ 0.992157, 0.960784, 0.901961, 1.000000 } },
	{ "mediumaquamarine",{ 0.400000, 0.803922, 0.666667, 1.000000 } },
	{ "darkkhaki",{ 0.741176, 0.717647, 0.419608, 1.000000 } },
	{ "antiquewhite",{ 0.980392, 0.921569, 0.843137, 1.000000 } },
	{ "lavenderblush",{ 1.000000, 0.941176, 0.960784, 1.000000 } },
	{ "deeppink",{ 1.000000, 0.078431, 0.576471, 1.000000 } },
	{ "lightsalmon",{ 1.000000, 0.627451, 0.478431, 1.000000 } },
	{ "mediumvioletred",{ 0.780392, 0.082353, 0.521569, 1.000000 } },
	{ "olive",{ 0.501961, 0.501961, 0.000000, 1.000000 } },
	{ "indigo",{ 0.294118, 0.000000, 0.509804, 1.000000 } },
	{ "crimson",{ 0.862745, 0.078431, 0.235294, 1.000000 } },
	{ "darkred",{ 0.545098, 0.000000, 0.000000, 1.000000 } },
	{ "cyan",{ 0.000000, 1.000000, 1.000000, 1.000000 } },
	{ "steelblue",{ 0.274510, 0.509804, 0.705882, 1.000000 } },
	{ "lightcoral",{ 0.941176, 0.501961, 0.501961, 1.000000 } },
	{ "lightblue",{ 0.678431, 0.847059, 0.901961, 1.000000 } },
	{ "brown",{ 0.647059, 0.164706, 0.164706, 1.000000 } },
	{ "lightgray",{ 0.827451, 0.827451, 0.827451, 1.000000 } },
	{ "orange",{ 1.000000, 0.647059, 0.000000, 1.000000 } },
	{ "gainsboro",{ 0.862745, 0.862745, 0.862745, 1.000000 } },
	{ "darkgreen",{ 0.000000, 0.392157, 0.000000, 1.000000 } },
	{ "mediumspringgreen",{ 0.000000, 0.980392, 0.603922, 1.000000 } },
	{ "darkmagenta",{ 0.545098, 0.000000, 0.545098, 1.000000 } },
	{ "salmon",{ 0.980392, 0.501961, 0.447059, 1.000000 } },
	{ "deepskyblue",{ 0.000000, 0.749020, 1.000000, 1.000000 } },
	{ "black",{ 0.000000, 0.000000, 0.000000, 1.000000 } },
	{ "yellow",{ 1.000000, 1.000000, 0.000000, 1.000000 } },
	{ "white",{ 1.000000, 1.000000, 1.000000, 1.000000 } },
	{ "navy",{ 0.000000, 0.000000, 0.501961, 1.000000 } },
	{ "goldenrod",{ 0.854902, 0.647059, 0.125490, 1.000000 } },
	{ "orchid",{ 0.854902, 0.439216, 0.839216, 1.000000 } },
	{ "darkgoldenrod",{ 0.721569, 0.525490, 0.043137, 1.000000 } },
	{ "olivedrab",{ 0.419608, 0.556863, 0.137255, 1.000000 } },
	{ "tan",{ 0.823529, 0.705882, 0.549020, 1.000000 } },
	{ "lightskyblue",{ 0.529412, 0.807843, 0.980392, 1.000000 } },
	{ "blueviolet",{ 0.541176, 0.168627, 0.886275, 1.000000 } },
	{ "palevioletred",{ 0.858824, 0.439216, 0.576471, 1.000000 } },
	{ "magenta",{ 1.000000, 0.000000, 1.000000, 1.000000 } },
	{ "pink",{ 1.000000, 0.752941, 0.796078, 1.000000 } },
	{ "darkcyan",{ 0.000000, 0.545098, 0.545098, 1.000000 } },
	{ "green",{ 0.000000, 0.501961, 0.000000, 1.000000 } },
	{ "peru",{ 0.803922, 0.521569, 0.247059, 1.000000 } },
	{ "gray",{ 0.501961, 0.501961, 0.501961, 1.000000 } },
	{ "whitesmoke",{ 0.960784, 0.960784, 0.960784, 1.000000 } },
	{ "mediumslateblue",{ 0.482353, 0.407843, 0.933333, 1.000000 } },
	{ "darkgrey",{ 0.662745, 0.662745, 0.662745, 1.000000 } },
	{ "darkgray",{ 0.662745, 0.662745, 0.662745, 1.000000 } },
	{ "mistyrose",{ 1.000000, 0.894118, 0.882353, 1.000000 } },
	{ "thistle",{ 0.847059, 0.749020, 0.847059, 1.000000 } },
	{ "grey",{ 0.501961, 0.501961, 0.501961, 1.000000 } },
	{ "lightslategray",{ 0.466667, 0.533333, 0.600000, 1.000000 } },
	{ "lightyellow",{ 1.000000, 1.000000, 0.878431, 1.000000 } },
	{ "honeydew",{ 0.941176, 1.000000, 0.941176, 1.000000 } },
	{ "lightseagreen",{ 0.125490, 0.698039, 0.666667, 1.000000 } },
	{ "ivory",{ 1.000000, 1.000000, 0.941176, 1.000000 } },
	{ "mediumblue",{ 0.000000, 0.000000, 0.803922, 1.000000 } },
	{ "maroon",{ 0.501961, 0.000000, 0.000000, 1.000000 } },
	{ "snow",{ 1.000000, 0.980392, 0.980392, 1.000000 } },
	{ "floralwhite",{ 1.000000, 0.980392, 0.941176, 1.000000 } },
	{ "lightsteelblue",{ 0.690196, 0.768627, 0.870588, 1.000000 } },
	{ "dimgrey",{ 0.411765, 0.411765, 0.411765, 1.000000 } },
	{ "saddlebrown",{ 0.545098, 0.270588, 0.074510, 1.000000 } },
	{ "mintcream",{ 0.960784, 1.000000, 0.980392, 1.000000 } },
	{ "seashell",{ 1.000000, 0.960784, 0.933333, 1.000000 } },
	{ "darkorchid",{ 0.600000, 0.196078, 0.800000, 1.000000 } },
	{ "peachpuff",{ 1.000000, 0.854902, 0.725490, 1.000000 } },
	{ "palegoldenrod",{ 0.933333, 0.909804, 0.666667, 1.000000 } },
	{ "violet",{ 0.933333, 0.509804, 0.933333, 1.000000 } },
	{ "moccasin",{ 1.000000, 0.894118, 0.709804, 1.000000 } },
	{ "lightgoldenrodyellow",{ 0.980392, 0.980392, 0.823529, 1.000000 } },
	{ "sandybrown",{ 0.956863, 0.643137, 0.376471, 1.000000 } },
	{ "chocolate",{ 0.823529, 0.411765, 0.117647, 1.000000 } },
	{ "fuchsia",{ 1.000000, 0.000000, 1.000000, 1.000000 } },
	{ "darkturquoise",{ 0.000000, 0.807843, 0.819608, 1.000000 } },
	{ "slategray",{ 0.439216, 0.501961, 0.564706, 1.000000 } },
	{ "rosybrown",{ 0.737255, 0.560784, 0.560784, 1.000000 } },
	{ "cornsilk",{ 1.000000, 0.972549, 0.862745, 1.000000 } },
	{ "wheat",{ 0.960784, 0.870588, 0.701961, 1.000000 } },
	{ "blanchedalmond",{ 1.000000, 0.921569, 0.803922, 1.000000 } },
	{ "lightgreen",{ 0.564706, 0.933333, 0.564706, 1.000000 } },
	{ "beige",{ 0.960784, 0.960784, 0.862745, 1.000000 } },
	{ "lightpink",{ 1.000000, 0.713725, 0.756863, 1.000000 } },
	{ "mediumseagreen",{ 0.235294, 0.701961, 0.443137, 1.000000 } },
	{ "plum",{ 0.866667, 0.627451, 0.866667, 1.000000 } },
	{ "chartreuse",{ 0.498039, 1.000000, 0.000000, 1.000000 } },
	{ "palegreen",{ 0.596078, 0.984314, 0.596078, 1.000000 } },
	{ "lightcyan",{ 0.878431, 1.000000, 1.000000, 1.000000 } },
	{ "lightgrey",{ 0.827451, 0.827451, 0.827451, 1.000000 } },
	{ "slateblue",{ 0.415686, 0.352941, 0.803922, 1.000000 } },
	{ "darkblue",{ 0.000000, 0.000000, 0.545098, 1.000000 } },
	{ "skyblue",{ 0.529412, 0.807843, 0.921569, 1.000000 } },
	{ "cornflowerblue",{ 0.392157, 0.584314, 0.929412, 1.000000 } },
	{ "blue",{ 0.000000, 0.000000, 1.000000, 1.000000 } },
	{ "cadetblue",{ 0.372549, 0.619608, 0.627451, 1.000000 } },
	{ "darkolivegreen",{ 0.333333, 0.419608, 0.184314, 1.000000 } },
	{ "lawngreen",{ 0.486275, 0.988235, 0.000000, 1.000000 } },
	{ "powderblue",{ 0.690196, 0.878431, 0.901961, 1.000000 } },
	{ "coral",{ 1.000000, 0.498039, 0.313725, 1.000000 } },
	{ "paleturquoise",{ 0.686275, 0.933333, 0.933333, 1.000000 } },
	{ "mediumpurple",{ 0.576471, 0.439216, 0.858824, 1.000000 } },
	{ "hotpink",{ 1.000000, 0.411765, 0.705882, 1.000000 } },
	{ "darkseagreen",{ 0.560784, 0.737255, 0.560784, 1.000000 } },
	{ "yellowgreen",{ 0.603922, 0.803922, 0.196078, 1.000000 } },
	{ "gold",{ 1.000000, 0.843137, 0.000000, 1.000000 } },
	{ "aquamarine",{ 0.498039, 1.000000, 0.831373, 1.000000 } },
	{ "firebrick",{ 0.698039, 0.133333, 0.133333, 1.000000 } },
	};
}

int luaopen_getcolor(lua_State* L)
{
	try 
	{
		const char* key = luaL_checkstring(L, 2);
		const Color& color = colors.at(key);
		return color.lua_createTable(L);
	}
	catch (const std::exception& e)
	{
		lua_settop(L, 0);
		lua_pushnil(L);
		lua_pushstring(L, e.what());
		return 2;
	}
}

int luaopen_addcolor(lua_State* L)
{
	try
	{
		std::string key = luaL_checkstring(L, 2);
	
		if (colors.count(key) != 0)
		{
			return luaL_error(L, "Colors with name '%s' already exists", key.c_str());
		}

		if (lua_istable(L, 3))
		{
			lua_Number values[4];
			for (int i = 1; i <= 4; ++i)
			{
				lua_rawgeti(L, 3, i);
				values[i - 1] = luaL_optnumber(L, -1, 1);
				lua_remove(L, -1);
			}

			colors.emplace(std::piecewise_construct, std::make_tuple(key), std::make_tuple(values));
		}
		else 
		{
			const lua_Number color = luaL_checknumber(L, 3);
			colors.emplace(std::piecewise_construct, std::make_tuple(key), std::make_tuple((unsigned long)color));
		}
	}
	catch (const std::exception& e)
	{
		return luaL_error(L, e.what());
	}

	return 0;
}

bool colorSort(const std::string& s1, const std::string& s2)
{
	return colors.at(s1) < colors.at(s2);
}

int luaopen_getallcolors(lua_State* L)
{
	if (lua_isboolean(L, 1) && lua_toboolean(L, 1))
	{
		std::list<std::string> sortList;
		
		for (auto pair : colors)
		{
			sortList.push_back(pair.first);
		}

		sortList.sort(colorSort);

		lua_newtable(L);

		int index = 1;
		for (const std::string& name : sortList)
		{
			lua_pushnumber(L, index++);

			lua_newtable(L);

			lua_pushstring(L, name.c_str());
			lua_setfield(L, -2, "name");

			colors.at(name).lua_createTable(L);
			lua_setfield(L, -2, "color");

			lua_settable(L, -3);
		}
	}
	else
	{
		lua_newtable(L);

		int index = 1;
		for (auto pair : colors)
		{
			lua_pushnumber(L, index++);

			lua_newtable(L);

			lua_pushstring(L, pair.first.c_str());
			lua_setfield(L, -2, "name");

			pair.second.lua_createTable(L);
			lua_setfield(L, -2, "color");

			lua_settable(L, -3);
		}
	}
	return 1;
}

int luaopen_colorToNumber(lua_State* L)
{
	const lua_Number 
		r = luaL_checknumber(L, 1),
		g = luaL_checknumber(L, 2),
		b = luaL_checknumber(L, 3),
		a = luaL_optnumber(L, 4, 1);

	Color c(r, g, b, a);
	lua_pushnumber(L, c.ToLong());
	return 1;
}

int luaopen_inverseColor(lua_State* L)
{
	if (lua_istable(L, 1))
	{
		for (int i = 1; i <= 3; ++i) 
		{
			lua_rawgeti(L, 1, i);
			lua_pushnumber(L, abs(luaL_checknumber(L, -1) - 1));
			lua_remove(L, -2);
		}
	}
	else if(lua_isnumber(L, 1) && lua_isnumber(L, 2) && lua_isnumber(L, 3))
	{
		const lua_Number r = lua_tonumber(L, 1), g = lua_tonumber(L, 2), b = lua_tonumber(L, 3);
		lua_pushnumber(L, abs(r - 1));
		lua_pushnumber(L, abs(g - 1));
		lua_pushnumber(L, abs(b - 1));
	}
	else
	{
		const char * key = luaL_checkstring(L, 1);
		try 
		{
			const Color& color = colors.at(key);
			lua_pushnumber(L, abs(color.r - 1));
			lua_pushnumber(L, abs(color.g - 1));
			lua_pushnumber(L, abs(color.b - 1));
		}
		catch (const std::exception& e)
		{
			return luaL_error(L, e.what());
		}
	}

	return 3;
}

int luaopen_eventecscolors(lua_State* L)
{
	lua_newtable(L);

	lua_pushcfunction(L, luaopen_getallcolors);
	lua_setfield(L, -2, "getAllColors");

	lua_pushcfunction(L, luaopen_inverseColor);
	lua_setfield(L, -2, "inverseColor");

	lua_pushcfunction(L, luaopen_colorToNumber);
	lua_setfield(L, -2, "colorToNumber");

	lua_newtable(L);
	lua_pushcfunction(L, luaopen_addcolor);
	lua_setfield(L, -2, "__newindex");

	lua_pushcfunction(L, luaopen_getcolor);
	lua_setfield(L, -2, "__index");

	lua_setmetatable(L, -2);

	return 1;
}