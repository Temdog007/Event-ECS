#pragma once


using namespace System::Threading;

ref class Lock 
{
private:
	Object^ m_obj;

public:
	Lock(Object^ obj) : m_obj(obj) 
	{
		Monitor::Enter(m_obj);
	}

	~Lock()
	{
		Monitor::Exit(m_obj);
	}
};