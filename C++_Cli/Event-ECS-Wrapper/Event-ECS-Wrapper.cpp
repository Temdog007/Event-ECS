#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

namespace EventECSWrapper
{
	ECSWrapper::ECSWrapper() : m_ecs(nullptr)
	{
		m_ecs = new ECS();
	}

	ECSWrapper::~ECSWrapper()
	{
		if (m_ecs != nullptr)
		{
			delete m_ecs;
			m_ecs = nullptr;
		}
	}

	void ECSWrapper::Init()
	{
		try
		{
			m_ecs->Init();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::DispatchEvent(String^ eventName)
	{
		int handled;
		IntPtr p = Marshal::StringToHGlobalAnsi(eventName);
		char *ev = static_cast<char*>(p.ToPointer());

		try
		{
			m_ecs->dispatchEvent(ev, handled);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
		finally
		{
			if (p != IntPtr::Zero)
			{
				Marshal::FreeHGlobal(p);
			}
		}

		return handled;
	}

	String^ ECSWrapper::GetState()
	{
		try 
		{
			const char* data = nullptr;
			m_ecs->getState(data);
			return gcnew String(data);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}
}