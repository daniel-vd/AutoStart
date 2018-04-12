using CSharpAnalytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ConsoleApp {
    public class Program {
        static String directoryPath;
        static String programsFile;
        static String idFile;

        [STAThread]
        static void Main(string[] args) {
            string id = null;
            string name = null;
            string path = null;

            //Analytics
            AutoMeasurement.Instance = new WinFormAutoMeasurement();
            AutoMeasurement.Start(new MeasurementConfiguration("UA-117174081-1"), "", new TimeSpan(1));

            AutoMeasurement.Client.TrackScreenView("Console");

            directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoStart");
            programsFile = Path.Combine(directoryPath, "Programs.xml");
            idFile = Path.Combine(directoryPath, "ID.txt");

            Console.Title = "AutoStart";//Set console title

            //Write explanation of this console
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i < "AutoStart will now start programs".Length; i++) {
                Console.Write("AutoStart will now start programs."[i]);
                Thread.Sleep(5);
            }
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.White;

            //Some information
            Console.WriteLine(" ");
            for (int i = 0; i < ("Opening programs database file").Length; i++) {
                Console.Write(("Opening programs database file")[i]);
                Thread.Sleep(5);
            }

            Console.WriteLine(" ");
            for (int i = 0; i < ("Now opening programs...").Length; i++) {
                Console.Write(("Now opening programs...")[i]);
                Thread.Sleep(5);
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Task<bool> checkUpdate = CheckUpdate();

            XmlDocument doc = new XmlDocument();
            doc.Load(programsFile);

            foreach (XmlNode node in doc.SelectNodes("/Programs/Program")) {
                id = node.Attributes["id"].InnerText;
                name = node.SelectSingleNode("name").InnerText;
                path = node.SelectSingleNode("path").InnerText;

                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int i = 0; i < ("Opening: " + name).Length; i++) {
                    Console.Write(("Opening: " + name)[i]);
                    Thread.Sleep(1);
                }

                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < ("with id: " + id).Length; i++) {
                    Console.Write(("with id: " + id)[i]);
                    Thread.Sleep(1);
                }

                Console.WriteLine(" ");
                for (int i = 0; i < ("and path: " + path).Length; i++) {
                    Console.Write(("and path: " + path)[i]);
                    Thread.Sleep(1);
                }

                Console.WriteLine(" ");
                for (int i = 0; i < ("Now starting..." ).Length; i++) {
                    Console.Write(("Now starting...")[i]);
                    Thread.Sleep(1);
                }

                Process.Start(path);

                Console.WriteLine(" ");
                Console.WriteLine(" ");

            }

            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < ("AutoStart succesfully started all programs!").Length; i++) {
                Console.Write(("AutoStart succesfully started all programs!")[i]);
                Thread.Sleep(5);
            }
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.WriteLine("Checking for updates...");

            checkUpdate.Wait();

            bool updateAvailable = checkUpdate.Result;

            if (updateAvailable)
            {
                if (MessageBox.Show("A new version of AutoStart is available, would you like to download the update?",
                    "Update AutoStart", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Process.Start("http://www.danielvd.tk/autostart/?msg=thankyou");
                    Process.Start("http://www.danielvd.tk/autostart/download.php");
                }
            }

        }
        
        //Method for checking for updates
        static async Task<bool> CheckUpdate()
        {
            if (!CheckForInternetConnection())
            {
                Console.WriteLine("Could not check for updates due to no internet connection!");
                return false;
            }

            var http = new HttpClient();

            string latestVersionString = await http.GetStringAsync(new Uri("http://danielvd.tk/autostart/version.php"));
            Version latestVersion = new Version(latestVersionString);

            //get my own version to compare against latest.
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version myVersion = new Version(fvi.ProductVersion);

            if (latestVersion > myVersion)
            {
                return true;
            }
            return false;
        }

        //Method for checking internet connection
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
