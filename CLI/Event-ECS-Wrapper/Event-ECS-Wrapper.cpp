#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

namespace EventECSWrapper
{
	ECSWrapper::ECSWrapper(String^ executablePath, String^ identity) : ECSWrapper(executablePath, identity, EventECS::NORMAL, nullptr)
	{
		
	}

	ECSWrapper::ECSWrapper(String^ executablePath, String^ identity, int type, Func<bool>^ mainThread)
	{
		ecs = new EventECS::ECS(static_cast<EventECS::ECSType>(type));
		IntPtr idStr = Marshal::StringToHGlobalAnsi(identity);
		IntPtr pathStr = Marshal::StringToHGlobalAnsi(executablePath);
		try
		{
			ecs->Initialize(static_cast<const char*>(pathStr.ToPointer()), static_cast<const char*>(idStr.ToPointer()));
			autoUpdate = false;
			if (ecs->getType() == EventECS::ECSType::LOVE)
			{
				if (mainThread == nullptr)
				{
					throw gcnew ArgumentNullException("Must pass the main thread invoke function");
				}
				updateOnMainThreadAction = mainThread;
				loveThread = gcnew Thread(gcnew ParameterizedThreadStart(LoveThread));
				loveThread->Name = "Love Auto Update";
				loveThread->Start(this);
			}
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

	ECSWrapper::~ECSWrapper()
	{	
		{
			Lock lock(this);
			SetAutoUpdate(false);
			if (ecs != nullptr)
			{
				delete ecs;
				ecs = nullptr;
			}
		}
		if (loveThread != nullptr)
		{
			loveThread->Join();
			loveThread = nullptr;
		}
	}

	String ^ ECSWrapper::AddEntity()
	{
		Lock lock(this);
		CheckECS();
		try 
		{
			return gcnew String(ecs->AddEntity().c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::RemoveEntity(int entityID)
	{
		Lock lock(this);
		CheckECS();
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
		Lock lock(this);
		CheckECS();
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

	void ECSWrapper::RegisterComponent(String ^ moduleName, bool replace)
	{
		Lock lock(this);
		CheckECS();
		IntPtr p = Marshal::StringToHGlobalAnsi(moduleName);
		try
		{
			return ecs->RegisterComponent(static_cast<const char*>(p.ToPointer()), replace);
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

	void ECSWrapper::AddComponent(int entityID, String ^ componentName)
	{
		Lock lock(this);
		CheckECS();
		IntPtr p = Marshal::StringToHGlobalAnsi(componentName);
		try
		{
			ecs->AddComponent(entityID, static_cast<const char*>(p.ToPointer()));
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

	void ECSWrapper::AddComponents(int entityID, array<String^>^ componentNames)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			std::list<std::string> list;
			for each(String^ str in componentNames)
			{
				IntPtr p = Marshal::StringToHGlobalAnsi(str);
				list.emplace_back(static_cast<const char*>(p.ToPointer()));
				Marshal::FreeHGlobal(p);
			}
			ecs->AddComponents(entityID, list);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::RemoveComponent(int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->RemoveComponent(entityID, componentID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::Serialize()
	{
		Lock lock(this);
		try
		{
			return gcnew String(ecs->Serialize().c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::SerializeEntity(int entityID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->SerializeEntity(entityID).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::SerializeComponent(int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->SerializeComponent(entityID, componentID).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::LoveUpdate()
	{
		Lock lock(this);
		CheckECS();
		try 
		{
			return ecs->LoveUpdate();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetAutoUpdate(bool value)
	{
		Lock lock(this);
		autoUpdate = value;
	}

	bool ECSWrapper::GetAutoUpdate()
	{
		Lock lock(this);
		return autoUpdate;
	}

	void ECSWrapper::CheckECS()
	{
		if (ecs == nullptr)
		{
			throw gcnew ArgumentNullException("ECS is null");
		}
	}

	void ECSWrapper::LoveThread(Object^ obj)
	{
		ECSWrapper^ wrapper = (ECSWrapper^)(obj);
		while (true)
		{
			{
				Lock lock(wrapper);
				if (wrapper->ecs == nullptr)
				{
					break;
				}
			}
			if (wrapper->GetAutoUpdate()) 
			{
				if (!wrapper->updateOnMainThreadAction())
				{
					break;
				}
				std::string log;
				if (wrapper->logFunction != nullptr && EventECS::ECS::GetLog(log))
				{
					wrapper->logFunction(gcnew String(log.c_str()));
				}
			}
			else 
			{
				Thread::Sleep(100);
			}
		}
	}

	void ECSWrapper::SetLogFunction(Action<String^>^ logFunc)
	{
		logFunction = logFunc;
	}
}

