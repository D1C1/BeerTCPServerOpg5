﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BeerTCPServerOpg5
{
    class Server
    {
        private TcpListener server = null;

        public void start()
        {
            Int32 port = 4646;
            IPAddress iPAddress = IPAddress.Loopback;
            server.Start();
            Console.WriteLine("server started");
            try
            {
                TcpClient tcpSocket = server.AcceptTcpClient();
                Task.Run(() =>
                {
                    TcpClient tempSocket = tcpSocket;
                    Doclient(tempSocket);
                });
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private void Doclient(TcpClient tempSocket)
        {
            throw new NotImplementedException();
        }
    }
}
