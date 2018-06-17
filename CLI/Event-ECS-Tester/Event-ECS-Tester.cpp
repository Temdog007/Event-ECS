#include "stdafx.h"

using namespace msclr::interop;
using namespace System;
using namespace System::Threading;
using namespace System::Reflection;
using namespace System::IO;
using namespace EventECS;

#define DefaultTest 1

void PrintConsole(const char* log)
{
	Console::WriteLine(gcnew String(log));
}

void RunUpdate(ECSMap& map, double milliseconds)
{
	Console::WriteLine("Running update for {0} milliseconds", milliseconds);
	System::DateTime start = DateTime::Now;
	for (System::DateTime now = System::DateTime::Now; (now - start).TotalMilliseconds < milliseconds; now = System::DateTime::Now)
	{
		Console::WriteLine("Update returned {0}", map.UpdateLove());
	}
	map.QuitLove();
	Console::WriteLine("Quit love");
	Console::WriteLine("Update returned {0}", map.UpdateLove());
	Console::WriteLine("Update done");
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
			}
			break;
			case 1:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\initializerScript.lua");
				map = ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length,
					context.marshal_as<const char*>(Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location)), 
					"Test Game");
				RunUpdate(*map, 5000);
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
				Console::WriteLine(gcnew String(ecsMap["Default"].SerializeEntity(1).c_str()));
			}
			break;
			default:
			{
				map = ECSMap::CreateInstance();
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
