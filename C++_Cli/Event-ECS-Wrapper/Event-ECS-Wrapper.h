#pragma once

#include "ECS.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace EventECSWrapper 
{
	public ref class ECSWrapper
	{
	private:
		ECS * m_ecs;

	public:
		ECSWrapper();
		~ECSWrapper();

		int DispatchEvent(String^ jsonCode);

		String^ GetState();
	};
}
