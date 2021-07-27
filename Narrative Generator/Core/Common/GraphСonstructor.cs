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
        public void CreateGraph(StoryGraph storyGraph, string graphName)
        {
            int counterNodes = 0;
            HashSet<Edge> edges = new HashSet<Edge>();
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
                    foreach (var edge in node.GetEdges())
                    {
                        bool skip = false;

                        foreach(var e in edges)
                        {
                            if (e.Equals(edge))
                            {
                                skip = true;
                                break;
                            }
                        }

                        if (skip) { continue; }

                        graphSTR = graphSTR.Insert(graphSTR.Length, edge.GetUpperNode().numberInSequence + "->" + edge.GetLowerNode().numberInSequence 
                            + "[label = " + '"' + edge.GetAction().ToString().Remove(0, 20) + '"' + "] \r\n");

                        edges.Add(edge);
                    }
                }
                else
                {
                    foreach (var edge in node.GetEdges())
                    {
                        bool skip = false;

                        foreach (var e in edges)
                        {
                            if (e.Equals(edge))
                            {
                                skip = true;
                                break;
                            }
                        }

                        if (skip) { continue; }

                        graphSTR = graphSTR.Insert(graphSTR.Length, edge.GetUpperNode().numberInSequence + "->" + "End"
                            + "[label = " + '"' + edge.GetAction().ToString().Remove(0, 20) + '"' + "] \r\n");

                        edges.Add(edge);
                    }
                }
            }

            graphSTR = graphSTR.Insert(graphSTR.Length, "}");

            SaveGraph(graphName, graphSTR);
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
