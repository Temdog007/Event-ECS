// eventecsTester.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

int main()
{
	ECS ecs;
	try 
	{
		ecs.Init();
		int handled;
		ecs.dispatchEvent("eventAddedComponent", handled);
		printf("%d", handled);
	}
	catch (const std::exception& e)
	{
		printf(e.what());
	}

	char c;
	std::cin >> c;
	return 0;
}
