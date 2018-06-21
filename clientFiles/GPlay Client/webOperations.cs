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

namespace GPlay_Client
{
    public class webOperations
    {
        private string home;
        private string login;
        private string signup;

        private string key;
        private string token;

        private readonly string type = "text";

        public readonly string domain = "http://localhost";

        public readonly string clientCache = Directory.GetCurrentDirectory() + @"\onlineCache\";
        public readonly string offlineCache = Directory.GetCurrentDirectory() + @"\clientCache\";
        public readonly string gameDirectory = Directory.GetCurrentDirectory() + @"\games";
        public readonly string gameDirectoryName = "games";

        public webOperations()
        {
            home = domain + @"/api/";
            login = domain + @"/api/login";
            signup = domain + @"/api/signup";
        }

        /// <summary>
        /// Checks if the server is online
        /// </summary>
        public bool isOnline()
        {
            string status = getDataFromServer(domain);
            if (status == "OK") return true;
            else return false;
        }

        /// <summary>
        /// Updates/Sets the home url using the existing key
        /// </summary>
        public void updateHomeUrl()
        {
            home = domain + @"/api/key=" + key + "&type=" + type;
        }

        /// <summary>
        /// Updates/Sets the login url, to make the online login later
        /// </summary>
        public void updateLoginUrl(string user, string password)
        {
            login = domain + @"/api/login/user=" + user + "&password=" + password;
        }

        /// <summary>
        /// Updates/Sets the signup url, to make the online login later
        /// </summary>
        public void updateSignupUrl(string name, string password)
        {
            signup = domain + @"/api/signup/name=" + name + "&password=" + password;
        }

        /// <summary>
        /// Logs in with the credentials and return the api key
        /// </summary>
        public void logIn()
        {
            key = getDataFromServer(login);
            setKey(key);
        }

        /// <summary>
        /// Signup and return the api key (to login)
        /// </summary>
        public void signUp()
        {
            key = getDataFromServer(signup);
            setKey(key);
        }

        /// <summary>
        /// Gets information from the API server home
        /// </summary>
        /// <remarks>
        /// Use this only if you initialised the home url
        /// </remarks>
        public string getDataFromServer()
        {
            return getDataFromServer(home);
        }

        /// <summary>
        /// Gets information from the specified url
        /// </summary>
        public string getDataFromServer(string url)
        {
            try
            {
                Uri uri = new Uri(url, true);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                System.IO.Stream st = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(st);
                string body = sr.ReadToEnd();
                return body;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        System.IO.Stream st = response.GetResponseStream();
                        System.IO.StreamReader sr = new System.IO.StreamReader(st);
                        string body = sr.ReadToEnd();
                        return body;
                    }
                    else return "error";
                }
                else return "error";
            }
        }

        public List<string> filesToDownload = new List<string>();
        public List<long> downloadTimes = new List<long>();
        public void downloadFile(string url,string target)
        {
            WebClient wc = new WebClient();
            Uri uri = new Uri(url, true);
            filesToDownload.Add(target);
            downloadTimes.Add(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            wc.DownloadFileAsync(uri, target);
            wc.DownloadFileCompleted += downloadFileCompleted(target);
        }

        public AsyncCompletedEventHandler downloadFileCompleted(string filename)
        {
            Action<object, AsyncCompletedEventArgs> action = (sender, e) =>
            {
                int index = filesToDownload.IndexOf(filename);
                filesToDownload.Remove(filename);
                long startTime = downloadTimes.ToArray()[index];
                downloadTimes.RemoveAt(index);
                Debug.WriteLine(filename + " was downloaded in " + ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond)-startTime).ToString() + " miliseconds");
            };
            return new AsyncCompletedEventHandler(action);
        }

        public bool filesAreDownloading()
        {
            if (filesToDownload.ToArray().Length != 0) return false;
            else return true;
        }

        public bool specificFileIsDownloaded(string filename)
        {
            return filesToDownload.Contains(filename);
        }

        /// <summary>
        /// Gets the home url
        /// </summary>
        public string getHomeUrl()
        {
            return home;
        }

        /// <summary>
        /// Gets the login url
        /// </summary>
        public string getLoginUrl()
        {
            return login;
        }

        /// <summary>
        /// Gets the signup url
        /// </summary>
        public string getSignupUrl()
        {
            return signup;
        }

        /// <summary>
        /// Gets the API key
        /// </summary>
        public string getKey()
        {
            return key;
        }

        /// <summary>
        /// Sets the API key
        /// </summary>
        private void setKey(string newKey)
        {
            key = newKey;
        }
    }
}
