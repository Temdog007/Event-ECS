#pragma once

#include "stdafx.h"

enum ECSType
{
	NORMAL,
	LOVE
};

class ECS 
{
private:
	lua_State * L;

	bool initialized;

	void Require(const char* moduleName, const char* globalName);

public:
	ECS();
	virtual ~ECS();

	ECS(const ECS&) = delete;
	ECS& operator=(const ECS&) = delete;

	bool Initialize(const char* identity, ECSType type = ECSType::NORMAL);

	const char* AddEntity();
	int RemoveEntity(int entityID);

	void RegisterComponent(const char* moduleName);

	//void AddComponent(int entityID, const char* componentName);
	//void RemoveComponent(int entityID, int componentID);

	const char* Serialize() const;
	/*void SerializeEntity(int entityID) const;
	void SerializeComponent(int entityID, int componentID) const;*/
};

