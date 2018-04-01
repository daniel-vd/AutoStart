using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStart {
    class FileReader {

        FilesManager filesManager;

        String idFile;
        int id;


        public int getID() {
            filesManager = new FilesManager();

            idFile = filesManager.idFile;

            id = Int32.Parse(File.ReadAllText(idFile));

            return id;
        }

    }
}
