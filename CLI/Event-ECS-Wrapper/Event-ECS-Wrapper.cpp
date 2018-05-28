#include "stdafx.h"

#include "Event-ECS-Wrapper.h"

using namespace System::Runtime::InteropServices;

namespace EventECSWrapper
{
	void LogHandler(const char* log)
	{
		ECSWrapper::LogEvent(gcnew String(log));
	}

	ECSWrapper::ECSWrapper(String^ initializerScript)
	{
		try
		{
			ecs = EventECS::ECSMap::CreateInstance(toChar(initializerScript), initializerScript->Length);
			EventECS::ECSMap::SetLogHandler(LogHandler);
			disposing = false;
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	ECSWrapper::ECSWrapper(String^ initializerScript, String^ executablePath, String^ identity)
	{
		try
		{
			ecs = EventECS::ECSMap::CreateInstance(toChar(initializerScript), initializerScript->Length, toChar(executablePath), toChar(identity));
			EventECS::ECSMap::SetLogHandler(LogHandler);
			disposing = false;

			autoUpdate = true;

			loveUpdateThread = gcnew Thread(gcnew ParameterizedThreadStart(LoveUpdateThread));
			loveUpdateThread->Name = "Love Update Thread";
			loveUpdateThread->Start(this);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	ECSWrapper::~ECSWrapper()
	{
		disposing = true;

		if (loveUpdateThread != nullptr)
		{
			loveUpdateThread->Join();
			loveUpdateThread = nullptr;
		}

		if (ecs != nullptr)
		{
			Monitor::Enter(this);
			EventECS::ECSMap::DeleteInstance();
			ecs = nullptr;
			Monitor::Exit(this);
		}
	}

	void ECSWrapper::Reset()
	{
		Lock lock(this);
		CheckECS();

		try
		{
			ecs->Reset();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::GetClassName(String^ systemName)
	{
		Lock lock(this);
		CheckECS();

		try 
		{
			return gcnew String(ecs->GetClassname(toChar(systemName)).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	const char* ECSWrapper::toChar(String^ str)
	{
		if (String::IsNullOrEmpty(str))
		{
			return "";
		}
		return context.marshal_as<const char*>(str);
	}

	String ^ ECSWrapper::AddEntity(String^ systemName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).AddEntity().c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::RemoveEntity(String^ systemName, int entityID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).RemoveEntity(entityID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::BroadcastEvent(String^ eventName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->BroadcastEvent(toChar(eventName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::DispatchEvent(String^ systemName, String^ eventName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).DispatchEvent(toChar(eventName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	int ECSWrapper::DispatchEvent(String^ systemName, int entityID, String^ eventName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).DispatchEvent(entityID, toChar(eventName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEventsToIgnore(array<String^>^ events)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			std::set<std::string> list;
			for each(String^ ev in events)
			{
				list.emplace(toChar(ev));
			}
			ecs->SetEventsToIgnore(list);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::AddComponent(String^ systemName, int entityID, String ^ componentName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).AddComponent(entityID, toChar(componentName));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::AddComponents(String^ systemName, int entityID, array<String^>^ componentNames)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			std::list<std::string> list;
			for each(String^ str in componentNames)
			{
				list.emplace_back(toChar(str));
			}
			ecs->operator[](toChar(systemName)).AddComponents(entityID, list);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::RemoveComponent(String^ systemName, int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).RemoveComponent(entityID, componentID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetLoggingEvents(bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->SetLogEvents(value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::IsLoggingEvents()
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->IsLoggingEvents();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	#pragma region System

	void ECSWrapper::SetSystemBool(String^ systemName, String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetSystemBool(toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetSystemNumber(String^ systemName, String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetSystemNumber(toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetSystemString(String^ systemName, String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetSystemString(toChar(key), toChar(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetSystemBool(String^ systemName, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetSystemBool(toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetSystemNumber(String^ systemName, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetSystemNumber(toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetSystemString(String^ systemName, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).GetSystemString(toChar(key)).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	#pragma region Entity

	void ECSWrapper::SetEntityBool(String^ systemName, int entityID, String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetEntityBool(entityID, toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEntityNumber(String^ systemName, int entityID, String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetEntityNumber(entityID, toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEntityString(String^ systemName, int entityID, String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetEntityString(entityID, toChar(key), toChar(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetEntityBool(String^ systemName, int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetEntityBool(entityID, toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetEntityNumber(String^ systemName, int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetEntityNumber(entityID, toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetEntityString(String^ systemName, int entityID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).GetEntityString(entityID, toChar(key)).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	#pragma endregion

	#pragma region Component

	void ECSWrapper::SetComponentEnabled(String^ systemName, int entityID, int componentID, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetComponentEnabled(entityID, componentID, value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetEntityEnabled(String^ systemName, int entityID, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetEntityEnabled(entityID, value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetSystemEnabled(String^ systemName, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetSystemEnabled(value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentBool(String^ systemName, int entityID, int componentID, String^ key, bool value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetComponentBool(entityID, componentID, toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentNumber(String^ systemName, int entityID, int componentID, String^ key, lua_Number value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetComponentNumber(entityID, componentID, toChar(key), value);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	void ECSWrapper::SetComponentString(String^ systemName, int entityID, int componentID, String^ key, String^ value)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			ecs->operator[](toChar(systemName)).SetComponentString(entityID, componentID, toChar(key), toChar(value));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::IsComponentEnabled(String^ systemName, int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).IsComponentEnabled(entityID, componentID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::IsEntityEnabled(String^ systemName, int entityID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).IsEntityEnabled(entityID);
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::IsSystemEnabled(String^ systemName)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).IsSystemEnabled();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	bool ECSWrapper::GetComponentBool(String^ systemName, int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetComponentBool(entityID, componentID, toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::GetComponentNumber(String^ systemName, int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return ecs->operator[](toChar(systemName)).GetComponentNumber(entityID, componentID, toChar(key));
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String^ ECSWrapper::GetComponentString(String^ systemName, int entityID, int componentID, String^ key)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).GetComponentString(entityID, componentID, toChar(key)).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	#pragma endregion

	#pragma endregion

	#pragma region Serialization

	array<String ^>^ ECSWrapper::Serialize()
	{
		Lock lock(this);
		try
		{
			std::list<std::string> list = ecs->Serialize();
			List<String^>^ rval = gcnew List<String^>();
			for (std::string data : list)
			{
				rval->Add(gcnew String(data.c_str()));
			}
			return rval->ToArray();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::SerializeSystem(String^ systemName)
	{
		Lock lock(this);
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).Serialize().c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::SerializeEntity(String^ systemName, int entityID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).SerializeEntity(entityID).c_str());
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	String ^ ECSWrapper::SerializeComponent(String^ systemName, int entityID, int componentID)
	{
		Lock lock(this);
		CheckECS();
		try
		{
			return gcnew String(ecs->operator[](toChar(systemName)).SerializeComponent(entityID, componentID).c_str());
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
			return ecs->UpdateLove();
		}
		catch (const std::exception& e)
		{
			throw gcnew Exception(gcnew String(e.what()));
		}
	}

	lua_Number ECSWrapper::LoveDraw(bool skipSleep)
	{
		Lock lock(this);
		try
		{
			CheckECS();
			return ecs->DrawLove(skipSleep);
		}
		catch (const std::exception& e)
		{
			LogEvent(gcnew String(e.what()));
		}
		return 1;
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

	void ECSWrapper::LoveUpdateThread(Object^ obj)
	{
		ECSWrapper^ wrapper = (ECSWrapper^)(obj);
		try
		{
			while (!wrapper->disposing)
			{
				if (wrapper->GetAutoUpdate())
				{
					wrapper->LoveUpdate();
					if (wrapper->disposing)
					{
						break;
					}
					Thread::Sleep(10);
				}
			}
		}
		catch (const std::exception& e)
		{
			wrapper->LogEvent(gcnew String(e.what()));
		}
	}

#pragma endregion
}

