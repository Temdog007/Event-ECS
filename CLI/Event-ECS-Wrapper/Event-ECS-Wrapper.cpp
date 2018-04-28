#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

using namespace System::Runtime::InteropServices;

namespace EventECSWrapper
{
	void LogHandler(const char* log)
	{
		ECSWrapper::LogEvent(gcnew String(log));
	}

	ECSWrapper::ECSWrapper()
	{
		try
		{
			ecs = new EventECS::ECS();
			EventECS::ECS::SetLogHandler(LogHandler);
			disposing = false;
			context = gcnew marshal_context();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	ECSWrapper::~ECSWrapper()
	{
		disposing = true;

		if (loveThread != nullptr)
		{
			loveThread->Join();
			loveThread = nullptr;
		}

		if (ecs != nullptr)
		{
			Monitor::Enter(this);
			delete ecs;
			ecs = nullptr;
			Monitor::Exit(this);
		}
		if (context != nullptr)
		{
			delete context;
			context = nullptr;
		}
	}

	bool ECSWrapper::InitializeLove(String^ executablePath, String^ identity, Func<bool>^ mainThread)
	{
		Lock lock(this);
		CheckECS();

		try
		{
			bool rval = ecs->InitializeLove(context->marshal_as<const char*>(executablePath), context->marshal_as<const char*>(identity));
			autoUpdate = false;

			if (mainThread == nullptr)
			{
				throw gcnew System::Exception("Main thread must be passed to initialize LOVE");
			}

			updateOnMainThreadAction = mainThread;
			loveThread = gcnew Thread(gcnew ParameterizedThreadStart(LoveThread));
			loveThread->Name = "Love Auto Update";
			loveThread->Start(this);

			return rval;
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
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

	bool ECSWrapper::RemoveEntity(int entityID)
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
		try
		{
			return ecs->DispatchEvent(context->marshal_as<const char*>(eventName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::RegisterComponent(String ^ moduleName, bool replace)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->RegisterComponent(context->marshal_as<const char*>(moduleName), replace);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::AddComponent(int entityID, String ^ componentName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->AddComponent(entityID, context->marshal_as<const char*>(componentName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
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
				list.emplace_back(context->marshal_as<const char*>(str));
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

	#pragma region System

	void ECSWrapper::SetSystemBool(String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetSystemBool(context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetSystemNumber(String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetSystemNumber(context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetSystemString(String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetSystemString(context->marshal_as<const char*>(key), context->marshal_as<const char*>(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetSystemBool(String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetSystemBool(context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetSystemNumber(String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetSystemNumber(context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetSystemString(String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->GetSystemString(context->marshal_as<const char*>(key)));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	#pragma region Entity

	void ECSWrapper::SetEntityBool(int entityID, String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetEntityBool(entityID, context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEntityNumber(int entityID, String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetEntityNumber(entityID, context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEntityString(int entityID, String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetEntityString(entityID, context->marshal_as<const char*>(key), context->marshal_as<const char*>(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetEntityBool(int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetEntityBool(entityID, context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetEntityNumber(int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetEntityNumber(entityID, context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetEntityString(int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->GetEntityString(entityID, context->marshal_as<const char*>(key)));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

#pragma endregion

	#pragma region Component

	void ECSWrapper::SetEnabled(int entityID, int componentID, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetEnabled(entityID, componentID, value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentBool(int entityID, int componentID, String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetComponentBool(entityID, componentID, context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentNumber(int entityID, int componentID, String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetComponentNumber(entityID, componentID, context->marshal_as<const char*>(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentString(int entityID, int componentID, String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetComponentString(entityID, componentID, context->marshal_as<const char*>(key), context->marshal_as<const char*>(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::IsEnabled(int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->IsEnabled(entityID, componentID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetComponentBool(int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetComponentBool(entityID, componentID, context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetComponentNumber(int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->GetComponentNumber(entityID, componentID, context->marshal_as<const char*>(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetComponentString(int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->GetComponentString(entityID, componentID, context->marshal_as<const char*>(key)));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

#pragma endregion

	#pragma region Serialization

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

	#pragma endregion

	#pragma region LOVE

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
		autoUpdate = value;
	}

	bool ECSWrapper::GetAutoUpdate()
	{
		return autoUpdate;
	}

	bool ECSWrapper::IsDisposing()
	{
		return disposing;
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
		try
		{
			ECSWrapper^ wrapper = (ECSWrapper^)(obj);
			while (!wrapper->disposing)
			{
				if (wrapper->ecs == nullptr)
				{
					break;
				}

				if (wrapper->GetAutoUpdate())
				{
					if (!wrapper->updateOnMainThreadAction())
					{
						break;
					}
				}
				else
				{
					Thread::Sleep(100);
				}
			}
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

#pragma endregion
}

