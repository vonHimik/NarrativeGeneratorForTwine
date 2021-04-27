using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class GraphСonstructor
    {
        public string CreateGraph(StoryGraph storyGraph, string graphName)
        {
            int counterNodes = 0;
            int counterEdges = 0;
            int offset = 0;
            string graphSTR = "digraph G { \r\n";

            // Generating a list of nodes
            foreach (var node in storyGraph.GetNodes())
            {
                if (counterNodes == 0)
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, counterNodes + 
                        " [shape =" + '"' + "doublecircle" + '"' + "label =" + '"' + "S" + counterNodes + '"' + "] \r\n");
                }
                else if (counterNodes > 0)
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, counterNodes +
                        " [shape =" + '"' + "circle" + '"' + "label =" + '"' + "S" + counterNodes + '"' + "] \r\n");
                }

                counterNodes++;
            }

            // Generating graph edges
            foreach (var node in storyGraph.GetNodes())
            {
                if (node != storyGraph.GetLastNode())
                {
                    int tempCounter = 1;
                    offset--;

                    foreach (var edge in node.GetEdges())
                    {
                        graphSTR = graphSTR.Insert(graphSTR.Length, counterEdges + "->" + (counterEdges + tempCounter + offset) 
                            + "[label = " + '"' + edge.GetAction().ToString() + '"' + "] \r\n");
                        tempCounter++;
                    }

                    counterEdges++;
                    offset += node.GetEdges().Count;
                }
            }

            graphSTR = graphSTR.Insert(graphSTR.Length, "}");
            return graphSTR;
        }

        public void SaveGraph(string fileName, string graph)
        {
            FileStream file = new FileStream(fileName + ".dot", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));
            streamWriter.Write(graph);
            streamWriter.Close();
        }
    }
}
