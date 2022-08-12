using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            IPHostEntry ipe = Dns.GetHostEntry("localhost");
            IPAddress[] ips = ipe.AddressList;

            foreach (IPAddress ip in ips)
            {
                Console.WriteLine(ip.ToString());

            }
            Console.ReadKey();
            */
           // Console.WriteLine(IPAddress.Any);

            Socket ss = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ss.Bind(new IPEndPoint(IPAddress.Any, 1234));
            ss.Listen(5);

            Socket cs = ss.Accept();
            Console.WriteLine("[+] Connection From: {0}", cs.RemoteEndPoint);

            Console.WriteLine("Enter A message To send: ");
            string msg;
            msg = Console.ReadLine();
            cs.Send(Encoding.ASCII.GetBytes(msg));
            while (msg != "quit")
            {
               
                byte[] buf = new byte[2048];
                Array.Clear(buf, 0, buf.Length);

                cs.Receive(buf);

                Console.WriteLine(Encoding.ASCII.GetString(buf).TrimEnd('\0'));

                Console.WriteLine("Enter A message To send: ");
                msg = Console.ReadLine();
                cs.Send(Encoding.ASCII.GetBytes(msg));
            }
            cs.Close();
            ss.Close();
             

            //Console.ReadKey();

        }
    }
}
