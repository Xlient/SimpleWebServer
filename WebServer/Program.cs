using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Program
    {
        static void Main(string[] args)
        {

            var server = new WebServer("http://localhost:51111/", @"F:\Web_Dev\");

            try
            {
                server.Start();
                Console.WriteLine("Server started sucessfully.");
                Console.WriteLine("Listening. . . . Press enter to stop");
                Console.ReadLine();
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
