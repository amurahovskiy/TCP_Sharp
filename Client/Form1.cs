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
    public partial class Form1 : Form
    {
        Form1 form;
        public Form1()
        {
            InitializeComponent();
        }
        string pathXml;

        private void button1_Click(object sender, EventArgs e)
        {
            string answer;
            pathXml =createXML();
            answer= Speaking();
            if(answerServerXML(answer))
            {
                form = new Form1();
                Form2 newForm = new Form2(answer,form);
                this.Hide();
                newForm.Show();

            }else
            {
                MessageBox.Show("Введіть нові значення");
                textBox1.Text = "";
                textBox2.Text = "";
            }


          
        }
        string Speaking()
        {
            string F = File.ReadAllText(pathXml, Encoding.GetEncoding(1251));
            byte[] M = Encoding.GetEncoding(1251).GetBytes(F);
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("10.0.64.148"),54000);
            byte[] bytes = new byte[1000000];
            using (Socket S = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                S.Connect(IPAddress.Parse("10.0.64.148"), 54000);
                S.Send(M);
                int bytesRec = S.Receive(bytes);
                string answerServer = Encoding.GetEncoding(1251).GetString(bytes, 0, bytesRec);
                return answerServer;
                S.Shutdown(SocketShutdown.Both);
            }
        }
        bool answerServerXML(string xmlText)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xmlText);
            XmlNode xnode = xdoc.DocumentElement;
            if (xnode.SelectSingleNode("./resTwo").SelectSingleNode("./mill").InnerText == "true")
                return true;

            return false;
        }
        string createXML()
        {
            string path = @"xmlRequest.xml";
            XDocument xmlDoc = new XDocument();
            XElement document = new XElement("document");
            XElement resOne = new XElement("resOne");
            XAttribute attrOneXML = new XAttribute("access", "true");
            XElement catalogElement = new XElement("catalog", textBox1.Text);
            XElement fileElement = new XElement("file", textBox2.Text);
            XElement radioBut = new XElement("radioButton", checkBox2.Checked);
            XElement resTwo = new XElement("resTwo");
            XAttribute attrTwoXML = new XAttribute("access", "false");
            XElement millElement = new XElement("mill", "false");
            resOne.Add(attrOneXML);
            resOne.Add(catalogElement);
            resOne.Add(fileElement);
            resOne.Add(radioBut);
            resTwo.Add(attrTwoXML);
            resTwo.Add(millElement);
            document.Add(resOne);
            document.Add(resTwo);
            xmlDoc.Add(document);
            xmlDoc.Save(path);
            return path;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
