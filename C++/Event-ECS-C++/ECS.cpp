#include "stdafx.h"

ECS::ECS() : L(luaL_newstate()), initialized(false)
{
	
}

ECS::~ECS()
{
	if (type == ECSType::LOVE)
	{
		lua_getglobal(L, "love");
		lua_getfield(L, -1, "event");
		lua_getfield(L, -1, "quit");
		if (lua_pcall(L, 0, 0, 0) != 0)
		{
			while (this->LoveUpdate());
		}
	}
	lua_close(L);
}

bool ECS::Initialize(const char* executablePath, const char* identity, ECSType type)
{
	if (!initialized)
	{
		this->type = type;
		
		luaL_openlibs(L);
		Require("eventecs", nullptr);

		switch (type)
		{
		case ECSType::LOVE:
			Require("love", "love");
			Require("loveSystem", "System");
			break;
		default:
			Require("system", "System");
			break;
		}

		lua_getglobal(L, "System");
		lua_pushstring(L, identity);
		lua_pushstring(L, executablePath);
		if (lua_pcall(L, 2, 1, 0) == 0)
		{
			lua_setglobal(L, "mySystem");
			lua_pop(L, 1);
			initialized = true;
		}
		else 
		{
			throw std::exception(lua_tostring(L, -1));
		}
	}
	return initialized;
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

const char* ECS::AddEntity()
{
	lua_getglobal(L, "mySystem");
	lua_getfield(L, -1, "createEntity");
	lua_pushvalue(L, -2);
	if (lua_pcall(L, 1, 1, 0) == 0)
	{
		lua_getfield(L, -1, "serialize");
		lua_pushvalue(L, -2);
		if (lua_pcall(L, 1, 1, 0) == 0)
		{
			const char* rval = lua_tostring(L, -1);
			lua_pop(L, 1);
			return rval;
		}
	}
	throw std::exception(lua_tostring(L, -1));
	
}

int ECS::RemoveEntity(int entityID)
{
	lua_getglobal(L, "mySystem");
	lua_getfield(L, -1, "removeEntity");
	lua_pushvalue(L, -2);
	lua_pushnumber(L, entityID);
	if (lua_pcall(L, 2, 1, 0) == 0)
	{
		int entitesRemoved = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return entitesRemoved;
	}
	throw std::exception(lua_tostring(L, -1));
}

int ECS::DispatchEvent(const char* eventName)
{
	lua_getglobal(L, "mySystem");
	lua_getfield(L, -1, "dispatchEvent");
	lua_pushvalue(L, -2);
	lua_pushstring(L, eventName);
	if (lua_pcall(L, 2, 1, 0) == 0)
	{
		int handles = static_cast<int>(lua_tonumber(L, -1));
		lua_pop(L, 1);
		return handles;
	}
	throw std::exception(lua_tostring(L, -1));
}

void ECS::RegisterComponent(const char* modName)
{
	lua_getglobal(L, "require");
	lua_pushstring(L, modName);
	if (lua_pcall(L, 1, 1, 0) == 0)
	{
		lua_setglobal(L, "Component");

		lua_getglobal(L, "mySystem");
		lua_getfield(L, -1, "registerComponent");
		lua_pushnumber(L, -2);
		lua_getglobal(L, "Component");
		if (lua_pcall(L, 2, 0, 0) == 0)
		{
			lua_pushnil(L);
			lua_setglobal(L, "Component");
			return;
		}
	}
	throw std::exception(lua_tostring(L, -1));
}

const char* ECS::Serialize() const
{
	lua_getglobal(L, "mySystem");
	lua_getfield(L, -1, "serialize");
	lua_pushvalue(L, -2);
	if (lua_pcall(L, 1, 1, 0) == 0)
	{
		const char* rval(lua_tostring(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	throw std::exception(lua_tostring(L, -1));
}

bool ECS::LoveUpdate()
{
	if (type != ECSType::LOVE)
	{
		throw std::exception("Not Love System. Can't call update");
	}

	lua_getglobal(L, "mySystem");
	lua_getfield(L, -1, "run");
	lua_pushvalue(L, -2);
	if (lua_pcall(L, 1, 1, 0) == 0)
	{
		bool rval = static_cast<bool>(lua_toboolean(L, -1));
		lua_pop(L, 1);
		return rval;
	}
	throw std::exception(lua_tostring(L, -1));
}