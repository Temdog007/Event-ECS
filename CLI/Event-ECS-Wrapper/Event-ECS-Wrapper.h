#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	public:
		ECSWrapper(String^ executablePath, String^ identity, int type);
		~ECSWrapper();

		String^ AddEntity();
		int RemoveEntity(int entityID);

		int DispatchEvent(String^ eventName);
		void RegisterComponent(String^ moduleName, bool replace);

		String^ AddComponent(int entityID, String^ componentName);
		void AddComponents(int entityID, array<String^>^ componentNames);

		bool RemoveComponent(int entityID, int componentID);

		String^ Serialize();
		String^ SerializeEntity(int entityID);
		String^ SerializeComponent(int entityID, int componentID);

		bool LoveUpdate();

		void SetAutoUpdate(bool value);
		bool GetAutoUpdate();

	private:
		EventECS::ECS *ecs;
		bool autoUpdate;

		Thread^ loveThread;
		Lock^ lock;

		static void LoveThread(Object^ obj);
	};
}
