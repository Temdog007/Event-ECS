#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	public:
		ECSWrapper(String^ executablePath, String^ identity);
		ECSWrapper(String^ executablePath, String^ identity, int type, Func<bool>^ updateAction);
		~ECSWrapper();

		String^ AddEntity();
		int RemoveEntity(int entityID);

		int DispatchEvent(String^ eventName);
		void RegisterComponent(String^ moduleName, bool replace);

		void AddComponent(int entityID, String^ componentName);
		void AddComponents(int entityID, array<String^>^ componentNames);

		bool RemoveComponent(int entityID, int componentID);

		String^ Serialize();
		String^ SerializeEntity(int entityID);
		String^ SerializeComponent(int entityID, int componentID);

		bool LoveUpdate();

		void SetLogFunction(Action<String^>^ logFunc);

		void SetAutoUpdate(bool value);
		bool GetAutoUpdate();

		void SetFrameRate(int fps);
		int GetFrameRate();

	private:
		EventECS::ECS *ecs;
		bool autoUpdate;

		Func<bool>^ updateOnMainThreadAction;
		Thread^ loveThread;
		const Lock^ lock;

		void CheckECS();

		Action<String^>^ logFunction;

		static void LoveThread(Object^ obj);
	};
}
