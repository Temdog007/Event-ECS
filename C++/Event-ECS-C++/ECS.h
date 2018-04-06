#pragma once

#include "stdafx.h"

class ECS 
{
private:
	lua_State * L;

	void loadModule(const char*);

public:
	ECS();
	~ECS();

	ECS(const ECS&) = delete;
	ECS& operator=(const ECS&) = delete;

	void dispatchEvent(const char* eventName, int& handled);

	void getState(const char*&) const;
};