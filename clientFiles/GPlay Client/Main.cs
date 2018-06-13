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

        public webOperations client = new webOperations();

        private void Main_Load(object sender, EventArgs e)
        {
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
                if (textBox2.Text != "")
                {
                    string user = textBox1.Text;
                    string pass = textBox2.Text;
                    client.updateLoginUrl(user, pass);
                    client.logIn();
                    if (client.getKey() == "Forbidden")
                    {
                        MessageBox.Show("login failed!");
                    }
                    else if (client.getKey() == "error")
                    {
                        MessageBox.Show("login failed!");
                    }
                    else MessageBox.Show("login successfull!");
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.updateHomeUrl();
            MessageBox.Show(client.getDataFromServer());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                if (textBox2.Text != "")
                {
                    string user = textBox1.Text;
                    string pass = textBox2.Text;
                    client.updateSignupUrl(user, pass);
                    client.signUp();
                    if (client.getKey() == "Forbidden")
                    {
                        MessageBox.Show("login failed!");
                    }
                    else if (client.getKey() == "error")
                    {
                        MessageBox.Show("login failed!");
                    }
                    else MessageBox.Show("login successfull!");
                }
        }

    }
}
