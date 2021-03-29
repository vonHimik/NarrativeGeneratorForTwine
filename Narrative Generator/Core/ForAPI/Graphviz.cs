using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Run(string outputPngFileName, string inputDotFileName)
        {
            if (wrapper.RunGraphviz(outputPngFileName + ".png", inputDotFileName + ".dot"))
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
        }
    }
}
