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
using System.Windows;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

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
        Point _originalPointGroupBox4 = new Point(236, 148);

        #endregion OriginalProprietes

        #region UI

        Point offscreen = new Point(5000, 5000);

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

        private void _showGroupBox4()
        {
            groupBox4.Visible = true;
            groupBox4.Location = _originalPointGroupBox4;
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

        private void _hideGroupBox4()
        {
            groupBox4.Visible = false;
            groupBox4.Location = offscreen;
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

        private void _clearGameList()
        {
            flowLayoutPanel1.Controls.Clear();
        }

        private void _hideMenu()
        {
            menuStrip1.Visible = false;
        }

        private void _showMenu()
        {
            menuStrip1.Visible = true;
        }

        private void _hideGameList()
        {
            flowLayoutPanel1.Visible = false;
        }

        private void _showGameList()
        {
            flowLayoutPanel1.Visible = true;
        }


        #endregion UI

        #region ZIP

        /// <summary>
        /// Helper method to determin if invoke required, if so will rerun method on correct thread.
        /// if not do nothing.
        /// </summary>
        /// <param name="c">Control that might require invoking</param>
        /// <param name="a">action to preform on control thread if so.</param>
        /// <returns>true if invoke required</returns>
        public static bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }

        public void unzip(string origin, string target, Action<string> callback)
        {
            FastZip fastZip = new FastZip();
            string fileFilter = null;
            fastZip.ExtractZip(origin, target, fileFilter);
            callback(origin);
        }

        public void installationFinished(string origin)
        {
            gameList._resetProgressBar(gameList.findTab(currentDownload));
            gameList._hideProgressBar(gameList.findTab(currentDownload));
            gameList.changeButtonText("Play", gameList.findTab(currentDownload));
            currentDownload = "";

            if (File.Exists(origin))
            {
                File.Delete(origin);
            }

        }

        #endregion ZIP

        public webOperations client = new webOperations();
        public settingsOperations settings = new settingsOperations();
        public uiOperations gameList = new uiOperations();

        private void Main_Load(object sender, EventArgs e)
        {
            gameList.layoutPanel = flowLayoutPanel1;
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
                MessageBox.Show("Your computer is missing some fonts! (Roboto)");
            }

            load();

        }

        private void load()
        {
            _clearGameList();
            _hideGroupBox1();
            _hideGroupBox2();
            _hideGroupBox3();
            _hideGroupBox4();
            _hideMenu();
            _hideGameList();

            if (client.isOnline())
            {
                onlineToolStripMenuItem.Text = "Online";
                if (settings.getAutoLogin())
                {
                    string user = settings.getSavedUsername();
                    string password = settings.getSavedPassword();
                    client.updateLoginUrl(user, password);
                    client.logIn();
                    if (client.getKey() == "Forbidden")
                    {
                        MessageBox.Show("Login failed!");
                        _loadGroupBox1();
                        _showGroupBox1();
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
            else
            {
                onlineToolStripMenuItem.Text = "Offline";
                appIsOffline();
            }
        }

        private void appIsOffline()
        {
            _hideGroupBox1();
            _hideGroupBox2();
            _hideGroupBox3();
            _hideGroupBox4();
            _hideMenu();
            _hideGameList();
            _showGroupBox4();
        }

        //there should be a directory in every game directory that has that versions description (a function to download the description?)
        private void downloadCache()
        {
            bool cacheExist = true;
            try
            {
                int directoryCount = Directory.GetDirectories(@"clientCache").Length;
            }
            catch { cacheExist = false; }

            if (cacheExist)
            {
                string gameLibraryList = client.domain + @"/api/games/general%key=" + client.getKey() + @"&type=text";
                gameLibraryList = client.getDataFromServer(gameLibraryList);
                string[] gameListArray = gameLibraryList.Split(';');
                for (int i = 0; i < gameListArray.Length; i++)
                {
                    gameListArray[i] = gameListArray[i].Trim();
                    string gameUrlForm = gameListArray[i].Replace(' ', '_');
                    string originUrlDesc = client.domain + @"/api/games/about=" + gameUrlForm + @"%key=" + client.getKey() + @"&type=text";
                    string originUrlLogo = client.domain + @"/api/games/about=" + gameUrlForm + @"%key=" + client.getKey() + @"&type=image";
                    string cache = client.clientCache + gameListArray[i] + @"\";
                    Directory.CreateDirectory(client.clientCache + gameListArray[i]);
                    client.downloadFile(originUrlDesc, cache + @"desc.json");
                    client.downloadFile(originUrlLogo, cache + @"logo.png");
                }
                //download user info
                loadCache();
            }
            else
            {
                string gameLibraryList = client.domain + @"/api/games/general%key=" + client.getKey() + @"&type=text";
                gameLibraryList = client.getDataFromServer(gameLibraryList);
                string[] gameListArray = gameLibraryList.Split(';');
                for (int i = 0; i < gameListArray.Length; i++)
                {
                    gameListArray[i] = gameListArray[i].Trim();
                    string gameUrlForm = gameListArray[i].Replace(' ', '_');
                    string originUrlDesc = client.domain + @"/api/games/about=" + gameUrlForm + @"%key=" + client.getKey() + @"&type=text";
                    string originUrlLogo = client.domain + @"/api/games/about=" + gameUrlForm + @"%key=" + client.getKey() + @"&type=image";
                    string cache = client.offlineCache + gameListArray[i] + @"\";
                    Directory.CreateDirectory(client.offlineCache + gameListArray[i]);
                    client.downloadFile(originUrlDesc, cache + @"desc.json");
                    client.downloadFile(originUrlLogo, cache + @"logo.png");
                }

                //download user info
                loadCache();
            }
        }

        private void updateCache()
        {
            string gameDirectory = client.gameDirectory;
            string[] files = Directory.GetDirectories(@"clientCache");
            string[] cacheFiles = Directory.GetDirectories(@"onlineCache");

            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i].Replace(@"clientCache\", "");
                for (int j = 0; j < cacheFiles.Length; j++)
                {
                    string newName = cacheFiles[j].Replace(@"onlineCache\", "");
                    if (newName == name)
                    {

                        string oldText = File.ReadAllText(files[i] + @"\desc.json");
                        string newText = File.ReadAllText(cacheFiles[j] + @"\desc.json");
                        JToken oldDesc = JObject.Parse(oldText);
                        JToken newDesc = JObject.Parse(newText);

                        string oldV = (string)oldDesc.SelectToken("version");
                        string newV = (string)newDesc.SelectToken("version");

                        if (oldV != newV)
                        {
                            gameList.changeButtonText("Update", gameList.findTab(name));
                        }
                    }
                }
            }
        }

        private void replaceSpecificCache(string cacheName)
        {
            string gameDirectory = client.gameDirectory;
            string[] files = Directory.GetDirectories(@"clientCache");
            string[] cacheFiles = Directory.GetDirectories(@"onlineCache");

            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i].Replace(@"clientCache\", "");
                if (cacheName == name)
                {
                    for (int j = 0; j < cacheFiles.Length; j++)
                    {
                        string newName = cacheFiles[j].Replace(@"onlineCache\", "");
                        if (newName == cacheName)
                        {
                            FileSystem.CopyDirectory(cacheFiles[j], files[i], true);
                            _clearGameList();
                            loadCache();
                        }
                    }
                }
            }
        }

        private void loadCache()
        {
            bool cacheExist = true;
            try
            {
                int directoryCount = Directory.GetDirectories(@"clientCache").Length;
            }
            catch { cacheExist = false; }

            if (cacheExist)
            {
                int directoryCount = Directory.GetDirectories(@"clientCache").Length;
                string[] files = Directory.GetDirectories(@"clientCache");

                //check if files are not downloading
                System.Threading.Thread.Sleep(2500);
                for (int i = 0; i < directoryCount; i++)
                {
                    string text = File.ReadAllText(files[i] + @"\desc.json");
                    JToken desc = JObject.Parse(text);

                    string name = (string)desc.SelectToken("name");
                    string version = (string)desc.SelectToken("version");
                    string description = (string)desc.SelectToken("description");
                    string gameKey = (string)desc.SelectToken("gameKey");

                    string imageLocation = files[i] + @"\logo.png";

                    //get the user info from a file


                    cacheExist = true;
                    try
                    {
                        int directoryCountSecondary = Directory.GetDirectories(@"offlineCache").Length;
                    }
                    catch { cacheExist = false; }
                    if (cacheExist)
                    {
                        gameList.addGameTab(imageLocation, name, "0 hours", version, "Play");
                    }
                    else
                    {
                        gameList.addGameTab(imageLocation, name, "0 hours", version, "Download");
                    }

                    gameList.getButton(gameList.findTab(name)).Click += downloadButtonClick;
                }
                if (client.isOnline())
                {
                    cacheExist = true;
                    try
                    {
                        int directoryCountSecondary = Directory.GetDirectories(@"offlineCache").Length;
                    }
                    catch { cacheExist = false; }
                    if (cacheExist)
                    {
                        updateCache();
                    }
                }
            }
            else
            {
                //Show a message for first time config
                load();
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
                if (textBox2.Text != "")
                {
                    if (client.isOnline())
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
                    else
                    {
                        string user = textBox1.Text;
                        string pass = textBox2.Text;
                        if (pass == settings.getSavedPassword() && user == settings.getSavedUsername())
                        {
                            offlineLoggedIn();
                        }

                    }
                }
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
            downloadCache();
            _showMenu();
            _showGameList();
            _clearGropBox1();
            _clearGropBox2();
            _hideGroupBox1();
            _hideGroupBox2();
        }

        private void offlineLog()
        {
            _hideMenu();
            _hideGameList();
            _hideGroupBox2();
            _hideGroupBox3();
            _hideGroupBox4();
            _showGroupBox1();
            _clearGropBox1();
        }

        private void offlineLoggedIn()
        {
            _hideGroupBox1();
            _hideGroupBox4();
            _showMenu();
            _showGameList();
            loadCache();
        }

        private void loggedOut()
        {
            _clearGameList();
            _hideGameList();
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
            _hideGameList();
            _loadGroupBox3();
            _showGroupBox3();
        }

        public string currentDownload = "";
        private void downloadButtonClick(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Download")
            {
                if (currentDownload == "")
                {
                    currentDownload = gameList.getTitle((Panel)(((Button)sender).Parent));
                    gameList.changeButtonText("Downloading", gameList.findTab(currentDownload));
                    gameList._showProgressBar(gameList.findTab(currentDownload));
                    WebClient wc = new WebClient();
                    wc.DownloadFileCompleted += downloadFinished;
                    wc.DownloadProgressChanged += downloadProgress;
                    wc.DownloadFileAsync(new System.Uri(client.domain + @"/games/" + currentDownload + @".zip"), Directory.GetCurrentDirectory() + @"\" + currentDownload + @".zip");
                }
                else MessageBox.Show("Wait for the other operation to end!");
            }
            else if (((Button)sender).Text == "Update")
            {
                if (currentDownload == "")
                {
                    currentDownload = gameList.getTitle((Panel)(((Button)sender).Parent));
                    gameList.changeButtonText("Downloading", gameList.findTab(currentDownload));
                    WebClient wc = new WebClient();
                    wc.DownloadFileCompleted += updateFinished;
                    wc.DownloadProgressChanged += downloadProgress;
                    wc.DownloadFileAsync(new System.Uri(client.domain + @"/games/" + currentDownload + @".zip"), Directory.GetCurrentDirectory() + @"\" + currentDownload + @".zip");
                }
                else MessageBox.Show("Wait for the other operation to end!");
            }
            else if (((Button)sender).Text == "Play")
            {
                string gameToStart = gameList.getTitle((Panel)(((Button)sender).Parent));
                Process.Start(Directory.GetCurrentDirectory() + @"\games\" + gameToStart + @"\" + gameToStart + @".exe");
            }
        }

        private void downloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            gameList.setProgressPercentage(e.ProgressPercentage, gameList.findTab(currentDownload));
        }

        private void downloadFinished(object sender, AsyncCompletedEventArgs e)
        {
            gameList.changeButtonText("Installing", gameList.findTab(currentDownload));
            new Task(() => { unzip(Directory.GetCurrentDirectory() + @"\" + currentDownload + @".zip", Directory.GetCurrentDirectory() + @"\games\" + currentDownload, installationFinished); }).Start();
        }

        private void updateFinished(object sender, AsyncCompletedEventArgs e)
        {
            replaceSpecificCache(currentDownload);
            gameList.changeButtonText("Installing", gameList.findTab(currentDownload));
            new Task(() => { unzip(Directory.GetCurrentDirectory() + @"\" + currentDownload + @".zip", Directory.GetCurrentDirectory() + @"\games\" + currentDownload, installationFinished); }).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            load();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            offlineLog();
        }

        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onlineToolStripMenuItem.Text=="Offline")
            {
                load();
            }
        }



    }

}
