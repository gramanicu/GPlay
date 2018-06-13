using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace GPlay_Client
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Uri u = new Uri(@"http://localhost/api/key=1234&type=json");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(u);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            System.IO.Stream st = res.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(st);
            string body = sr.ReadToEnd();
            MessageBox.Show(body);
        }
    }
}
