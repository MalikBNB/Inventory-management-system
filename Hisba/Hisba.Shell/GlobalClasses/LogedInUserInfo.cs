using DevExpress.Xpf.Core;
using Hisba.Data.Bll.Entities;
using Hisba.Data.Layers.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hisba.Shell.GlobalClasses
{
    public class LogedInUserInfo
    {
        public static User CurrentUser = new User();

        public LogedInUserInfo()
        {
            GetUserInfo();
        }

        public static async void GetUserInfo()
        {
            var user =  await UserBll.GetUserByUsername("Admin");

            if (user != null)
                CurrentUser = user;
        }

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\data.txt";

                if (Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                string dataToSave = Username + "#//#" + Password;

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine(dataToSave);

                    return true;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\data.txt";

                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0];
                            Password = result[1];
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show($"An error occurred: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
