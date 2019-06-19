using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml.Linq;
using System.Xml;


namespace Laabaaaaaaaaaaaaaaaaaaa4_Server_
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        string pathSer;
        Thread workThread;
        string argFirst;
        string argSecond;
        string argThird;
        bool mustStop;
        protected override void OnStart(string[] args)
        {
            workThread = new Thread(Work);
            workThread.Start();
        }

        protected override void OnStop()
        {
            if ((workThread != null) && (workThread.IsAlive))
            {
                mustStop = true;
            }
        }
        void Work()
        {
            IPAddress IP = IPAddress.Parse("10.0.64.148");
            IPEndPoint ipEndPoint = new IPEndPoint(IP, 54000);
            Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            S.Bind(ipEndPoint);
            S.Listen(10);
            while (!mustStop)
            {
                using (Socket H = S.Accept())
                {
                    IPEndPoint L = new IPEndPoint(IP, 54000);
                    EndPoint R = (EndPoint)(L);
                    byte[] D = new byte[10000];
                    int Receive = H.ReceiveFrom(D, ref R);
                    string Request = Encoding.GetEncoding(1251).GetString(D, 0, Receive);
                    ReadXML(Request);
                    if (checkXML(Request))
                    {
                        if (Check())
                        {
                            WriteXML(Request, "./resTwo", "./mill", "true");
                        }
                        else
                        {
                            WriteXML(Request, "./resTwo", "./mill", "false");
                        }
                    }
                    else
                    {
                        createFileOrDirectory();
                        WriteXML(Request, "./resTwo", "./mill", "Все створено");
                    }
                    string W = File.ReadAllText(@"C:\Users\Владислав\server.xml", Encoding.GetEncoding(1251));
                    byte[] M = Encoding.GetEncoding(1251).GetBytes(W);
                    H.Send(M);
                    H.Shutdown(SocketShutdown.Both);
                }
            }

        
         }
        void createFileOrDirectory()
        {
            if(argThird=="true")
            {
                Directory.CreateDirectory(argFirst + "\\" + argSecond);
            }
            else
            {
                File.Create(argFirst + "\\" + argSecond);
            }
        }
        void WriteXML(string xmlText,string pathOne,string pathTwo,string word)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNode root = xmlDoc.DocumentElement.SelectSingleNode(pathOne);
            root.SelectSingleNode(pathTwo).InnerText = word;
            xmlDoc.Save(@"C:\Users\Владислав\server.xml");
        }
        void ReadXML(string xmlText)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNode root = xmlDoc.DocumentElement;
            argFirst = root.SelectSingleNode("./resOne").SelectSingleNode("./catalog").InnerText;
            argSecond= root.SelectSingleNode("./resOne").SelectSingleNode("./file").InnerText;
            argThird=root.SelectSingleNode("./resOne").SelectSingleNode("./radioButton").InnerText;
        }
        bool checkXML(string xmlText)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            XmlNode root = xmlDoc.DocumentElement;
            if (root.SelectSingleNode("./resOne").Attributes["access"].Value == "true")
                return true;
            else
                return false;
        }
        bool Check()
        {
            if(argThird!="false"&& Directory.Exists(argFirst)&&!Directory.Exists(argFirst+"\\"+argSecond))
            {
                return true;
            }else if(argThird=="false"&& Directory.Exists(argFirst) && !File.Exists(argFirst+"\\"+argSecond))
            {
                return true;
            }
            else
            {
                return false;
            }
                   

            
        }
       

    }
}
