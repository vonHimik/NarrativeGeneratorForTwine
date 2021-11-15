using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Narrative_Generator
{
    class Graphviz
    {
        GraphvizWrapper wrapper;
        public bool isSuccess;

        public Graphviz()
        {
            wrapper = new GraphvizWrapper();
            isSuccess = false;
        }

        public void Run(string fileName)
        {
            if (wrapper.RunGraphviz(fileName)) { isSuccess = true; }
            else { isSuccess = false; }
        }
    }
}
