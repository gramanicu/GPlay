using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPlay_Client.Properties;

using System.Security;
using System.Security.Cryptography;

namespace GPlay_Client
{
    public class settingsOperations
    {
        public settingsOperations()
        {
        }

        public void saveSettings()
        {
            Settings.Default.Save();
        }

        public string getSavedUsername()
        {
            return decrypt(Settings.Default.Username);
        }

        public void setSavedUsername(string user)
        {
            Settings.Default.Username = user;
            saveSettings();
        }

        public string getSavedPassword()
        {
            return decrypt(Settings.Default.Password);
        }

        public void setSavedPassword(string password)
        {
            Settings.Default.Password = password;
            saveSettings();
        }

        public bool getAutoLogin()
        {
            return Settings.Default.AutoLogin;
        }

        public void setAutoLogin(bool auto)
        {
            Settings.Default.AutoLogin = auto;
            saveSettings();
        }

        public string decrypt(string info)
        {
            return StringSecurityHelper.DecryptString(info);
        }

        public string encrypt(string info)
        {
            return info.EncryptString(); 
        }

        public string[] encryptAll(string[] info)
        {
            int i = 0;
            foreach (var s in info)
            {
                info[i] = s.EncryptString();
                i++;
            }
            return info;
        }

        public string[] decryptAll(string[] info)
        {
            int i = 0;
            foreach (var s in info)
            {
                info[i] = StringSecurityHelper.DecryptString(s);
                i++;
            }
            return info;
        }
        
    }

    public static class StringSecurityHelper
    {
        private static readonly byte[] entropy = Encoding.Unicode.GetBytes("5ID'&mc %sJo@lGtbi%n!G^ fiVn8 *tNh3eB %rDaVijn!.c b");

        public static string EncryptString(this string input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(input), entropy, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptString(this string encryptedData)
        {
            if (encryptedData == null)
            {
                return null;
            }

            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);

                return Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }
    }
}
