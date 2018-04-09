#pragma once

using namespace System;
using namespace System::Runtime::InteropServices;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	public:
		ECSWrapper();
		~ECSWrapper();

		bool Initialize(String^ executablePath, String^ identity, int type);

		String^ AddEntity();
		int RemoveEntity(int entityID);

		void RegisterComponent(String^ moduleName);

		int DispatchEvent(String^ eventName);

		String^ Serialize();

		bool LoveUpdate();

	private:
		ECS *ecs;
	};
}
