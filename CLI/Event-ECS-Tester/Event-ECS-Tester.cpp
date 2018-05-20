#include "stdafx.h"

using namespace msclr::interop;
using namespace System;
using namespace System::Reflection;
using namespace System::IO;
using namespace EventECS;

const int Test = 1;

void PrintConsole(const char* log)
{
	Console::WriteLine(log);
}

int main(array<System::String ^> ^args)
{
	marshal_context context;
	try
	{
		ECSMap::SetLogHandler(PrintConsole);
		switch (Test)
		{
			case 0:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\initializerScript.lua");
				ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length);
			}
			break;
			case 1:
			{
				String^ code = File::ReadAllText("E:\\Game Development\\GitHub\\Event-ECS\\Default Initializer Scripts\\loveInitializerScript.lua");
				ECSMap::CreateInstance(context.marshal_as<const char*>(code), code->Length, 
					context.marshal_as<const char*>(Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location)), 
					"Test Game");
			}
			break;
			default:
			{
				ECSMap::CreateInstance();
			}
		}
		return 0;
	}
	catch (const std::exception& e)
	{
		Console::WriteLine(e.what());
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
