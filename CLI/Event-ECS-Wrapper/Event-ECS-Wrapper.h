#pragma once

using namespace System;
using namespace msclr::interop;
using namespace System::Threading;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	public:
		ECSWrapper();
		~ECSWrapper();

		bool InitializeLove(String^ executablePath, String^ identity);

		String^ AddEntity();
		bool RemoveEntity(int entityID);

		int DispatchEvent(String^ eventName);
		void RegisterComponent(String^ moduleName, bool replace);

		void AddComponent(int entityID, String^ componentName);
		void AddComponents(int entityID, array<String^>^ componentNames);

		bool RemoveComponent(int entityID, int componentID);

		void SetSystemBool(String^ key, bool value);
		void SetSystemNumber(String^ key, lua_Number value);
		void SetSystemString(String^ key, String^ value);

		void SetEntityBool(int entityID, String^ key, bool value);
		void SetEntityNumber(int entityID, String^ key, lua_Number value);
		void SetEntityString(int entityID, String^ key, String^ value);

		void SetEnabled(int entityID, int componentID, bool value);
		void SetComponentBool(int entityID, int componentID, String^ key, bool value);
		void SetComponentNumber(int entityID, int componentID, String^ key, lua_Number value);
		void SetComponentString(int entityID, int componentID, String^ key, String^ value);

		bool IsEnabled(int entityID, int componentID);
		bool GetSystemBool(String^ key);
		lua_Number GetSystemNumber(String^ key);
		String^ GetSystemString(String^ key);

		bool GetEntityBool(int entityID, String^ key);
		lua_Number GetEntityNumber(int entityID, String^ key);
		String^ GetEntityString(int entityID, String^ key);

		bool GetComponentBool(int entityID, int componentID, String^ key);
		lua_Number GetComponentNumber(int entityID, int componentID, String^ key);
		String^ GetComponentString(int entityID, int componentID, String^ key);

		String^ Serialize();
		String^ SerializeEntity(int entityID);
		String^ SerializeComponent(int entityID, int componentID);

		bool LoveUpdate();
		void LoveDraw();

		void SetAutoUpdate(bool value);
		bool GetAutoUpdate();

		bool IsDisposing();

		event Action<Action^>^ OnMainThread;

		static Action<String^>^ LogEvent;
		
	private:
		EventECS::ECS *ecs;
		bool autoUpdate;
		bool disposing;

		Thread^ loveUpdateThread;
		Thread^ loveDrawThread;

		void CheckECS();
		void DoOnMainThread(Action^ action);

		marshal_context context;

		static void LoveThread(Object^ obj);
	};
}
