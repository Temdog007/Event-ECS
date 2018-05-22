#pragma once

using namespace System;
using namespace msclr::interop;
using namespace System::Windows::Threading;
using namespace System::Collections::Generic;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	public:
		ECSWrapper(String^ initializerCode);
		ECSWrapper(String^ initializerCode, String^ executablePath, String^ identity);
		~ECSWrapper();

		void Reset();

		String^ AddEntity(String^ systemName);
		bool RemoveEntity(String^ systemName, int entityID);

		void BroadcastEvent(String^ eventName);
		int DispatchEvent(String^ systemName, String^ eventName);

		bool IsSystemEnabled(String^ systemName);
		void SetSystemEnabled(String^ systemName, bool value);

		bool IsEntityEnabled(String^ systemName, int entityID);
		void SetEntityEnabled(String^ systemName, int entityID, bool value);

		bool IsComponentEnabled(String^ systemName, int entityID, int componentID);
		void SetComponentEnabled(String^ systemName, int entityID, int componentID, bool value);

		void AddComponent(String^ systemName, int entityID, String^ componentName);
		void AddComponents(String^ systemName, int entityID, array<String^>^ componentNames);

		bool RemoveComponent(String^ systemName, int entityID, int componentID);

		void SetSystemBool(String^ systemName, String^ key, bool value);
		void SetSystemNumber(String^ systemName, String^ key, lua_Number value);
		void SetSystemString(String^ systemName, String^ key, String^ value);

		void SetEntityBool(String^ systemName, int entityID, String^ key, bool value);
		void SetEntityNumber(String^ systemName, int entityID, String^ key, lua_Number value);
		void SetEntityString(String^ systemName, int entityID, String^ key, String^ value);

		void SetComponentBool(String^ systemName, int entityID, int componentID, String^ key, bool value);
		void SetComponentNumber(String^ systemName, int entityID, int componentID, String^ key, lua_Number value);
		void SetComponentString(String^ systemName, int entityID, int componentID, String^ key, String^ value);

		bool GetSystemBool(String^ systemName, String^ key);
		lua_Number GetSystemNumber(String^ systemName, String^ key);
		String^ GetSystemString(String^ systemName, String^ key);

		bool GetEntityBool(String^ systemName, int entityID, String^ key);
		lua_Number GetEntityNumber(String^ systemName, int entityID, String^ key);
		String^ GetEntityString(String^ systemName, int entityID, String^ key);

		bool GetComponentBool(String^ systemName, int entityID, int componentID, String^ key);
		lua_Number GetComponentNumber(String^ systemName, int entityID, int componentID, String^ key);
		String^ GetComponentString(String^ systemName, int entityID, int componentID, String^ key);

		array<String^>^ Serialize();
		String^ SerializeSystem(String^ systemName);
		String^ SerializeEntity(String^ systemName, int entityID);
		String^ SerializeComponent(String^ systemName, int entityID, int componentID);

		bool LoveUpdate();
		lua_Number LoveDraw(bool skipSleep);

		void SetAutoUpdate(bool value);
		bool GetAutoUpdate();

		bool IsDisposing();

		static Action<String^>^ LogEvent;

	private:
		EventECS::ECSMap *ecs;
		bool autoUpdate;
		bool disposing;

		Thread^ loveUpdateThread;

		void CheckECS();

		marshal_context context;

		const char* toChar(String^ str);

		static void LoveUpdateThread(Object^ obj);
	};
}
