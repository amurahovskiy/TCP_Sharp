using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace lABAAAAAAAAAAAA4_CLIENT_
{
    public partial class Form2 : Form
    {
        Form1 newForm;
        Form2 form;
        public bool type;
        string answer;
        public Form2(string ans,Form1 f)
        {
            newForm = f;
            answer = ans;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string F = File.ReadAllText(createXML(answer), Encoding.GetEncoding(1251));
            byte[] M = Encoding.GetEncoding(1251).GetBytes(F);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.0.64.148"), 54000);
            byte[] bytes = new byte[1000000];
            using (Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                S.Connect(IPAddress.Parse("10.0.64.148"), 54000);
                S.Send(M);
                int bytesRec = S.Receive(bytes);
                string answerServer = Encoding.GetEncoding(1251).GetString(bytes, 0, bytesRec);
                string path = @"xmlRequest.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(answerServer);
                XmlNode root = xmlDoc.DocumentElement;
                MessageBox.Show(root.SelectSingleNode("./resTwo").SelectSingleNode("./mill").InnerText);
                S.Shutdown(SocketShutdown.Both);
            }
             newForm = new Form1();
            this.Close();
            newForm.Show();
        }
        string createXML(string ans)
        {
            string path = @"xmlRequest.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ans);
            XmlNode root = xmlDoc.DocumentElement;
            root.SelectSingleNode("./resOne").Attributes["access"].Value = "false";
            xmlDoc.Save(path);
            return path;
        }
    }
}
