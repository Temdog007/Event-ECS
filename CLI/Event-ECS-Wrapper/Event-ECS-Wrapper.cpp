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

	bool ECSWrapper::Initialize(String^ identity, int type)
	{
		IntPtr p = Marshal::StringToHGlobalAnsi(identity);
		try 
		{
			return ecs->Initialize(static_cast<const char*>(p.ToPointer()), static_cast<ECSType>(type));
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

	String ^ ECSWrapper::AddEntity()
	{
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
		try 
		{ 
			return ecs->RemoveEntity(entityID); 
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::RegisterComponent(String ^ moduleName)
	{
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
		try
		{
			return gcnew String(ecs->Serialize());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

}

