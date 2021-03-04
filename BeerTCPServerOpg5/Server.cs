using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BeerMorten;
using Newtonsoft.Json;

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
                        sw.WriteLine("Henter alle &oslash;l");
                        foreach (var VARIABLE in _beers)
                        {
                            sw.WriteLine(VARIABLE.ToString());
                        }
                        
                        break;
                    case "Hent":
                        sw.WriteLine("skriv venligst id til &oslash;l");
                        try
                        {
                            int id = Convert.ToInt32(sr.ReadLine());
                            sw.WriteLine(_beers.Find(beer => beer.Id == id));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ikke korrekt input streng pr&oslash;v igen");
                        }
                        break;
                    case "Gem":
                        sw.WriteLine("Skriv venligst din nye l i JSON format som f.eks. {}");
                        string jsonBeer = sr.ReadLine();
                        if (jsonBeer != null)
                        {
                            try
                            {
                                Beer newBeer = JsonConvert.DeserializeObject<Beer>(jsonBeer);
                                _beers.Add(newBeer);
                            }
                            catch (Exception e)
                            {
                                sw.WriteLine("Dette matcher ikke et json objekt prøv igen");
                            }
                            
                        }

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
