using System;

namespace BeerTCPServerOpg5
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.start();
        }
    }
}
