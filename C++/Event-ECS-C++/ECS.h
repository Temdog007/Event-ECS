#pragma once

#include "stdafx.h"

class ECS 
{
private:
	lua_State * L;

public:
	ECS();
	~ECS();

	ECS(const ECS&) = delete;
	ECS& operator=(const ECS&) = delete;

	void Require(const char* moduleName, const char* globalName);

	bool DoString(const char* code);
};
