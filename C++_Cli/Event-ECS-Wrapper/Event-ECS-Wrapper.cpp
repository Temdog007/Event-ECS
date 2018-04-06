#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

namespace EventECSWrapper
{
	ECSWrapper::ECSWrapper() : m_ecs(nullptr)
	{
		try 
		{
			m_ecs = new ECS();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	ECSWrapper::~ECSWrapper()
	{
		if (m_ecs != nullptr)
		{
			delete m_ecs;
			m_ecs = nullptr;
		}
	}

	int ECSWrapper::DispatchEvent(String^ eventName)
	{
		IntPtr p = IntPtr::Zero;
		try 
		{
			int handled;
			p = Marshal::StringToHGlobalAnsi(eventName);
			char *ev = static_cast<char*>(p.ToPointer());
			m_ecs->dispatchEvent(ev, handled);
			return handled;
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