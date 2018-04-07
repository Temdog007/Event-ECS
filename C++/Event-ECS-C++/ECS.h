#pragma once

#include "stdafx.h"

class ECS 
{
private:
	lua_State * L;
	bool initialized;

	void loadModule(const char*,int);

	void safeCall(int nargs, int rvalue);

public:
	ECS();
	~ECS();

	ECS(const ECS&) = delete;
	ECS& operator=(const ECS&) = delete;

	void Initialize();

	void dispatchEvent(const char* eventName, int& handled);

	void getState(const char*& jsonCode);
};