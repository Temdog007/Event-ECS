#include "stdafx.h"
#include "Server.h"

namespace eventecsserver 
{
	std::string GetErrorString(const int error)
	{
		char errStr[1024];
		ZeroMemory(errStr, sizeof(errStr));
		FormatMessageA(FORMAT_MESSAGE_IGNORE_INSERTS | FORMAT_MESSAGE_FROM_SYSTEM, NULL, error, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), errStr, sizeof(errStr), NULL);
		return errStr;
	}

	const int Server::Family = AF_INET;

	const int Server::Type = SOCK_STREAM;

	const int Server::Protocol = IPPROTO_TCP;

	Server::Server(unsigned short _port) : sock(INVALID_SOCKET), runFlag(true), port(_port), OnError(nullptr), OnReceive(nullptr)
	{
		int result;
		WSADATA wsaData = { 0 };
		if ((result = WSAStartup(MAKEWORD(2, 2), &wsaData)) != 0)
		{
			throw std::exception(GetErrorString(result).c_str());
		}

		sock = socket(Family, Type, Protocol);
		if (sock == INVALID_SOCKET)
		{
			throw std::exception(GetErrorString(WSAGetLastError()).c_str());
		}

		thread = std::thread(&Server::SocketThread, this);
	}

	Server::~Server()
	{
		runFlag = false;

		if (thread.joinable())
		{
			thread.join();
		}

		if (sock != INVALID_SOCKET) 
		{
			int result;
			if (result = closesocket(sock) < 0 && OnError != nullptr)
			{
				OnError(GetErrorString(result).c_str());
			}
		}

		WSACleanup();
	}

	void Server::SetErrorHandler(void(*func)(const char*))
	{
		OnError = func;
	}

	void Server::SetListener(std::string(*func)(const std::string&))
	{
		OnReceive = func;
	}

	Server& Server::Instance()
	{
		static Server server;
		return server;
	}

	void Server::SocketThread()
	{
		sockaddr_in clientService;
		ZeroMemory(&clientService, sizeof(clientService));
		clientService.sin_family = AF_INET;
		InetPton(Server::Family, L"127.0.0.1", &clientService);
		clientService.sin_port = htons(port);

		const int addrlen = sizeof(clientService);
		int result;

		if ((result = bind(sock, (SOCKADDR*)&clientService, sizeof(clientService))) == SOCKET_ERROR)
		{
			throw std::exception(GetErrorString(result).c_str());
		}

		bool flag = true;
		int val = 0;
		if ((result = setsockopt(sock, SOL_SOCKET, SO_KEEPALIVE, (char*)&val, TRUE)) == SOCKET_ERROR)
		{
			throw std::exception(GetErrorString(result).c_str());
		}

		result = listen(sock, 1);
		if (result == SOCKET_ERROR)
		{
			throw std::exception(GetErrorString(result).c_str());
		}

		int sendResult;
		char buffer[1024];
		SOCKET acceptSocket;
		while (runFlag)
		{
			acceptSocket = accept(sock, (SOCKADDR*)&clientService, (socklen_t*)&addrlen);
			if (acceptSocket != INVALID_SOCKET)
			{
				do 
				{
					if ((result = recv(acceptSocket, buffer, sizeof(buffer), 0)) < 0)
					{
						if (OnError != nullptr)
						{
							OnError(GetErrorString(result).c_str());
						}
					}
					else if (result != 0)
					{
						if (OnReceive != nullptr)
						{
							const std::string response = OnReceive(buffer);
							if (!response.empty())
							{
								if ((sendResult = send(acceptSocket, response.c_str(), response.size(), 0)) < 0)
								{
									if (OnError != nullptr)
									{
										OnError(response.c_str());
									}
								}
							}
						}
					}
				} while (result > 0);
			}
		}
	}
}