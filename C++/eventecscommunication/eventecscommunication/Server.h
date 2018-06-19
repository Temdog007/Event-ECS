#pragma once

#include "stdafx.h"

#define DEFAULT_PORT 23232

namespace eventecsserver 
{
	class Server
	{
	public:
		Server(const Server&) = delete;
		Server& operator=(const Server&) = delete;
		~Server();

		static Server& Instance();

		void SetListener(std::string(*)(const std::string&));
		void SetErrorHandler(void(*)(const char*));

		const static int Family;
		const static int Type;
		const static int Protocol;

	private:
		Server(USHORT _port = DEFAULT_PORT);
		SOCKET sock;

		const USHORT port;
		bool runFlag;

		std::string(*OnReceive)(const std::string&);
		void(*OnError)(const char*);

		std::thread thread;

		void SocketThread();
	};
}