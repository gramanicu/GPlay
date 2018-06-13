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
using System.Diagnostics;
using System.Drawing.Text;

namespace GPlay_Client
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            menuStrip1.Renderer = new MyRenderer();
        }

        #region ToolStripUIModifiers

        private class MyRenderer : ToolStripProfessionalRenderer
        {
            public MyRenderer() : base(new MyColors()) { }
        }

        private class MyColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(64, 64, 64); }
            }
            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(96,96,96); }
            }
        }

        #endregion ToolStripUIModifiers

        #region OriginalProprietes

        Point _originalPointGroupBox1 = new Point(457, 148);
        Point _originalPointGroupBox2 = new Point(457, 148);
        Point _originalPointGroupBox3 = new Point(406, 151);

        #endregion OriginalProprietes

        #region UI

        Point offscreen = new Point(2000, 2000);

        private void _loadGroupBox1()
        {
            textBox1.Text = settings.getSavedUsername();
            textBox2.Text = "";
            checkBox1.Checked = settings.getAutoLogin();
        }

        private void _loadGroupBox3()
        {
            checkBox3.Checked = settings.getAutoLogin();
        }

        private void _showGroupBox1()
        {
            groupBox1.Visible = true;
            groupBox1.Location = _originalPointGroupBox1;
        }

        private void _showGroupBox2()
        {
            groupBox2.Visible = true;
            groupBox2.Location = _originalPointGroupBox2;
        }

        private void _showGroupBox3()
        {
            groupBox3.Visible = true;
            groupBox3.Location = _originalPointGroupBox3;
        }

        private void _hideGroupBox1()
        {
            groupBox1.Visible = false;
            groupBox1.Location = offscreen;
        }

        private void _hideGroupBox2()
        {
            groupBox2.Visible = false;
            groupBox2.Location = offscreen;
        }

        private void _hideGroupBox3()
        {
            groupBox3.Visible = false;
            groupBox3.Location = offscreen;
        }

        private void _clearGropBox1()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            checkBox1.Checked = false;
        }

        private void _clearGropBox2()
        {
            textBox3.Text = "";
            textBox4.Text = "";
            checkBox1.Checked = false;
        }

        private void _hideMenu()
        {
            menuStrip1.Visible = false;
        }

        private void _showMenu()
        {
            menuStrip1.Visible = true;
        }

        private void addGameTab(Image icon, string title, string time, string status, string buttonText)
        {
            Point _1 = new Point(3, 5);
            Point _2 = new Point(89, 26);
            Point _3 = new Point(371, 33);
            Point _4 = new Point(487, 28);
            Point _5 = new Point(653, 28);
            Point _6 = new Point(961, 38);

            Panel p = new Panel();

            p.Name = "panel" + title;
            p.Size = new System.Drawing.Size(1183, 90);
            p.TabIndex = 0;

            PictureBox pic = new PictureBox();
            pic.BackgroundImage = icon;
            pic.BackgroundImageLayout = ImageLayout.Stretch;
            pic.Name = "iconPictureBox" + title;
            pic.Location = _1;
            pic.Size = new System.Drawing.Size(80, 80);
            pic.TabIndex = 0;
            pic.TabStop = false;

            Label name = new Label();
            name.Text = title;
            name.Name = "nameLabel" + title;
            name.Location = _2;
            name.AutoSize = true;
            name.Font = new System.Drawing.Font("Roboto Black", 24F, System.Drawing.FontStyle.Bold);
            name.Size = new System.Drawing.Size(193, 38);
            name.TabIndex = 1;

            Label currentStatus = new Label();
            currentStatus.Text = status;
            currentStatus.Name = "statusLabel" + title;
            currentStatus.Location = _3;
            currentStatus.AutoSize = true;
            currentStatus.Font = new System.Drawing.Font("Roboto Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentStatus.Size = new System.Drawing.Size(81, 29);
            currentStatus.TabIndex = 1;

            Button action = new Button();
            action.Text = buttonText;
            action.Name = "actionLabel" + title;
            action.Location = _4;
            action.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            action.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(189)))), ((int)(((byte)(0)))));
            action.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            action.Size = new System.Drawing.Size(157, 42);
            action.TabIndex = 0;
            action.UseVisualStyleBackColor = true;

            ProgressBar progress = new ProgressBar();
            progress.Visible = false;
            progress.Name = "downloadProgressBar" + title;
            progress.Location = _5;
            progress.Size = new System.Drawing.Size(229, 42);
            progress.TabIndex = 2;

            Label currentTime = new Label();
            currentTime.Text = "Time played: "+ time;
            currentTime.Name = "timeLabel" + title;
            currentTime.Location = _6;
            currentTime.AutoSize = true;
            currentTime.Font = new System.Drawing.Font("Roboto Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentTime.Size = new System.Drawing.Size(122, 23);
            currentTime.TabIndex = 1;

            p.Controls.Add(pic);
            p.Controls.Add(name);
            p.Controls.Add(currentStatus);
            p.Controls.Add(action);
            p.Controls.Add(progress);
            p.Controls.Add(currentTime);

            flowLayoutPanel1.Controls.Add(p);
        }

        #endregion UI

        public webOperations client = new webOperations();
        public settingsOperations settings = new settingsOperations();

        private void Main_Load(object sender, EventArgs e)
        {
            bool fontExists = false;
            var fontsCollection = new InstalledFontCollection();
            foreach (var fontFamiliy in fontsCollection.Families)
            {
                if (fontFamiliy.Name == "Roboto Black")
                {
                    fontExists = true;
                }
            }

            if (!fontExists)
            {
                MessageBox.Show("Your computer is missing some fonts!");
            }

            _hideGroupBox1();
            _hideGroupBox2();
            _hideGroupBox3();
            _hideMenu();

            if (settings.getAutoLogin())
            {
                string user = settings.getSavedUsername();
                string password = settings.getSavedPassword();
                client.updateLoginUrl(user, password);
                client.logIn();
                if (client.getKey() == "Forbidden")
                {
                    MessageBox.Show("Login failed!");
                }
                else if (client.getKey() == "error")
                {
                    MessageBox.Show("Login failed!");
                }
                else
                {
                    loggedIn();
                }
            }
            else
            {
                _loadGroupBox1();
                _showGroupBox1();
            }
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
                        MessageBox.Show("Login failed!");
                    }
                    else if (client.getKey() == "error")
                    {
                        MessageBox.Show("Login failed!");
                    }
                    else
                    {
                        settings.setAutoLogin(checkBox1.Checked);
                        loggedIn();
                    }
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.updateHomeUrl();
            MessageBox.Show(client.getDataFromServer());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
                if (textBox3.Text != "")
                {
                    string user = textBox4.Text;
                    string pass = textBox3.Text;
                    client.updateSignupUrl(user, pass);
                    client.signUp();
                    if (client.getKey() == "Forbidden")
                    {
                        MessageBox.Show("Login failed!");
                    }
                    else if (client.getKey() == "error")
                    {
                        MessageBox.Show("Login failed!");
                    }
                    else
                    {
                        settings.setAutoLogin(checkBox2.Checked);
                        loggedIn();
                    }
                }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _clearGropBox1();
            _hideGroupBox1();
            _showGroupBox2();
        }


        private void loggedIn()
        {
            _showMenu();
            _clearGropBox1();
            _clearGropBox2();
            _hideGroupBox1();
            _hideGroupBox2();
        }

        private void loggedOut()
        {
            _hideMenu();
            _loadGroupBox1();
            _showGroupBox1();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loggedOut();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            settings.setAutoLogin(checkBox3.Checked);

            _hideGroupBox3();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _hideGroupBox3();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _loadGroupBox3();
            _showGroupBox3();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGameTab(global::GPlay_Client.Properties.Resources.logo640x640, "Half-Life 3", "23H", "v1.5.2", "Play");
        }


    }

}
