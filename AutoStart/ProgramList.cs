using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStart {
    public class ProgramList {

        public static List<String[]> programList = new List<string[]>();

        AutoStart autoStart = new AutoStart();

        public ProgramList() {
            
        }

        public ProgramList(String[] program) {
            programList.Add(program);

        }

    }
}
