using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BeerMorten;

namespace BeerTCPServerOpg5
{
    public class Server
    {
        private static List<Beer> _beers = new List<Beer>();
        public void start()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 4646;
                IPAddress iPAddress = IPAddress.Loopback;
                server = new TcpListener(iPAddress, port);
                server.Start();
                Console.WriteLine("server started");
                while (true)
                {
                    TcpClient tcpSocket = server.AcceptTcpClient();
                    Task.Run(() =>
                    {
                        TcpClient tempSocket = tcpSocket;
                        Doclient(tempSocket);
                    });
                }
                server.Stop();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private void Doclient(TcpClient tempSocket)
        {
            Console.WriteLine("server activated");
            Stream ns = tempSocket.GetStream();

            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;
            bool keepGoing = true;
            while (keepGoing)
            {
                string message = sr.ReadLine();
                switch (message)
                {
                    case "stop":
                        keepGoing = false;
                        break;
                    case "HentAlle":
                        sw.WriteLine("Henter alle øl");
                        break;
                    case "Hent":
                        sw.WriteLine("skriv venligst id til øl");
                        break;
                    case "Gem":
                        sw.WriteLine("Skriv venligst din nye øl i JSON format som f.eks. {}");
                        break;
                    default:
                        sw.WriteLine("Dette er ikke en mulighed");
                        break;
                }
            }
            ns.Close();
            tempSocket.Close();
        }
    }
}
