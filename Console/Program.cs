using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApp {
    public class Program {
        static String directoryPath;
        static String programsFile;
        static String idFile;

        static void Main(string[] args) {
            string id = null;
            string name = null;
            string path = null;

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
            for (int i = 0; i < ("This screen will automatically close in 1 second").Length; i++) {
                Console.Write(("This screen will automatically close in 1 second")[i]);
                Thread.Sleep(5);
            }


            Thread.Sleep(1000);


        }
    }
}
