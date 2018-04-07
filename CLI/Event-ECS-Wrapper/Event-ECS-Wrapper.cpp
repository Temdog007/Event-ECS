#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

#include <exception>

namespace EventECSWrapper
{
	ECSWrapper::ECSWrapper()
	{
		ecs = new ECS();
	}

	ECSWrapper::~ECSWrapper()
	{
		if (ecs != nullptr)
		{
			delete ecs;
			ecs = nullptr;
		}
	}

	void ECSWrapper::Require(String^ modName)
	{
		const char* str = nullptr;
		try
		{
			str = (char*)(void*)Marshal::StringToHGlobalAnsi(modName);
			ecs->Require(str, nullptr);
		}
		catch (const std::exception& e)
		{
			gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			if (str != nullptr)
			{
				Marshal::FreeHGlobal((IntPtr)(void*)str);
			}
		}
	}

	void ECSWrapper::Require(String^ modName, String^ globalName)
	{
		const char* str1 = nullptr;
		const char* str2 = nullptr;
		try
		{
			str1 = (char*)(void*)Marshal::StringToHGlobalAnsi(modName);
			str2 = (char*)(void*)Marshal::StringToHGlobalAnsi(globalName);
			ecs->Require(str1, str2);
		}
		catch (const std::exception& e)
		{
			gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			if (str1 != nullptr)
			{
				Marshal::FreeHGlobal((IntPtr)(void*)str1);
			}
			if (str2 != nullptr)
			{
				Marshal::FreeHGlobal((IntPtr)(void*)str2);
			}
		}
	}

	void ECSWrapper::DoString(String^ code)
	{
		const char* str = nullptr;
		try
		{
			str = (char*)(void*)Marshal::StringToHGlobalAnsi(code);
			ecs->DoString(str);
		}
		catch (const std::exception& e)
		{
			gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			if (str != nullptr)
			{
				Marshal::FreeHGlobal((IntPtr)(void*)str);
			}
		}
	}
}

