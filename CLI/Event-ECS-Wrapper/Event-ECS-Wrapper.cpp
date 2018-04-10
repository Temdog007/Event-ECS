#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

#include <exception>

namespace EventECSWrapper
{
	ECSWrapper::ECSWrapper()
	{
		ecs = new ECS();
		disposed = false;
	}

	ECSWrapper::~ECSWrapper()
	{
		if (!disposed)
		{
			if (ecs != nullptr)
			{
				delete ecs;
				ecs = nullptr;
				disposed = true;
			}
		}
	}

	bool ECSWrapper::Initialize(String^ executablePath, String^ identity, int type)
	{
		if (disposed)
		{
			return false;
		}

		IntPtr idStr = Marshal::StringToHGlobalAnsi(identity);
		IntPtr pathStr = Marshal::StringToHGlobalAnsi(executablePath);
		try
		{
			return ecs->Initialize(static_cast<const char*>(pathStr.ToPointer()), static_cast<const char*>(idStr.ToPointer()), static_cast<ECSType>(type));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			Marshal::FreeHGlobal(idStr);
			Marshal::FreeHGlobal(pathStr);
		}
	}

	String ^ ECSWrapper::AddEntity()
	{
		if (disposed)
		{
			return String::Empty;
		}

		try 
		{
			return gcnew String(ecs->AddEntity());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::RemoveEntity(int entityID)
	{
		if (disposed)
		{
			return 0;
		}

		try 
		{ 
			return ecs->RemoveEntity(entityID); 
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::DispatchEvent(String^ eventName)
	{
		if (disposed)
		{
			return 0;
		}

		IntPtr p = Marshal::StringToHGlobalAnsi(eventName);
		try
		{
			return ecs->DispatchEvent(static_cast<const char*>(p.ToPointer()));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			Marshal::FreeHGlobal(p);
		}
	}

	void ECSWrapper::RegisterComponent(String ^ moduleName)
	{
		if (disposed)
		{
			return;
		}

		IntPtr p = Marshal::StringToHGlobalAnsi(moduleName);
		try
		{
			return ecs->RegisterComponent(static_cast<const char*>(p.ToPointer()));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			Marshal::FreeHGlobal(p);
		}
	}

	String ^ ECSWrapper::Serialize()
	{
		if (disposed)
		{
			return String::Empty;
		}

		try
		{
			return gcnew String(ecs->Serialize());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::LoveUpdate()
	{
		if (disposed)
		{
			return false;
		}

		try 
		{
			return ecs->LoveUpdate();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}
}

