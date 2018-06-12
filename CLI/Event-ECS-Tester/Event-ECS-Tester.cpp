#include "stdafx.h"

using namespace msclr::interop;
using namespace System;
using namespace System::Threading;
using namespace System::Reflection;
using namespace System::IO;
using namespace EventECS;

#define DefaultTest 2;

void PrintConsole(const char* log)
{
	Console::WriteLine(gcnew String(log));
}

void TestUpdate(ECSMap& map)
{
	for (int i = 0; i < 10; ++i)
	{
		map.BroadcastEvent("eventupdate");
		Thread::Sleep(100);
	}
}

void TestLoveUpdate(ECSMap& map, bool broadcast = false)
{
	for (int i = 0; i < 300; ++i)
	{
		Console::WriteLine("Update returned {0}", map.UpdateLove());
		if (i > 0) 
		{
			if (broadcast && i % 20 == 0)
			{
				map.BroadcastEventWithArgs("eventbroadcast");
			}
			/*if (i % 60 == 0)
			{
				map["Default"].SetSystemEnabled(!map["Default"].IsSystemEnabled());
			}*/
			if (i % 30 == 0)
			{
				map["Default"].SetComponentEnabled(1, 2, !map["Default"].IsComponentEnabled(1, 2));
			}
			/*if (i % 30 == 0)
			{
				map["Default"].SetEntityEnabled(1, !map["Default"].IsEntityEnabled(1));
			}*/
		}
	}
}

int main(array<System::String ^> ^args)
{
	Int32 test = DefaultTest;
	if (args->Length > 0)
	{
		Int32::TryParse(args[0], test);
	}

	marshal_context context;
	try
	{
		ECSMap* map;
		ECSMap::SetLogHandler(PrintConsole);
		switch (test)
		{
			case 0:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\initializerScript.lua");
				map = ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length);
				TestUpdate(*map);
			}
			break;
			case 1:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\initializerScript.lua");
				map = ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length,
					context.marshal_as<const char*>(Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location)), 
					"Test Game");
				TestLoveUpdate(*map);
			}
			break;
			case 2:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\loveInitializerScript.lua");
				map = ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length,
					context.marshal_as<const char*>(Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location)),
					"Test Game");

				ECSMap& ecsMap = *map;
				ecsMap.SetLogEvents(true);
				/*for (auto str : ecsMap["Default"].Serialize())
				{
					Console::WriteLine(gcnew String(str.c_str()));
				}*/
				Console::WriteLine(gcnew String(ecsMap["Default"].SerializeEntity(1).c_str()));
				TestLoveUpdate(ecsMap, true);
			}
			break;
			default:
			{
				map = ECSMap::CreateInstance();
				TestUpdate(*map);
			}
		}
		
		Console::WriteLine("Event ECS test completed succesfully");
		return 0;
	}
	catch (const std::exception& e)
	{
		Console::WriteLine(gcnew String(e.what()));
		return -1;
	}
	catch (Exception^ e)
	{
		Console::WriteLine(e);
		return -2;
	}
	finally
	{
		ECSMap::DeleteInstance();
	}
}
