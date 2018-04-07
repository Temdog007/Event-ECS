// eventecsTester.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

int main()
{
	try 
	{
		ECS ecs;
		ecs.Initialize();
		int handled;
		ecs.dispatchEvent("eventAddedComponent", handled);
		printf("%d", handled);
	}
	catch (const std::exception& e)
	{
		printf(e.what());
	}

	std::cout << "Press any key to continue...";
	char c;
	std::cin >> c;
	return 0;
}
