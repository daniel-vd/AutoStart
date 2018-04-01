using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AutoStart {
    public class FilesManager {

        FileWriter fileWriter;
        FileReader fileReader;

        static String directoryPath;
        public String programsFile;
        public String idFile;

        public FilesManager() {

            directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AutoStart");
            programsFile = Path.Combine(directoryPath, "Programs.xml");
            idFile = Path.Combine(directoryPath, "ID.txt");
        }

        public void CheckFiles () {
            fileWriter = new FileWriter();
            //Make sure folder exists, no need for checking
            System.IO.Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments), "AutoStart"));

            //If files do not exist, create them
            if (!File.Exists(programsFile)) {
                XmlWriter writer = XmlWriter.Create(programsFile);

                fileWriter.WriteProgramsFileStart(writer);
            }
            if (!File.Exists(idFile)) {
                fileWriter.WriteIDFileStart();
            }
        }

        public int getID() {
            //Get last used program id
            fileReader = new FileReader();

            return fileReader.getID();
        }

        public void AddProgramsRow (String path, int id, String name) {
            fileWriter = new FileWriter();

            //Document and element properties
            XDocument doc = XDocument.Load(programsFile);
            XElement programsRow = doc.Element("Programs");

            fileWriter.WriteProgramsRow(path, id, name, doc, programsRow);

            fileWriter.NextID(id);
        }

    }
}
