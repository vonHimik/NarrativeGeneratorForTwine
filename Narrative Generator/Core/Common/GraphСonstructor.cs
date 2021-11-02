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
            HashSet<Edge> edges = new HashSet<Edge>();
            string graphSTR = "digraph G { \r\n";

            // Generating a list of nodes
            foreach (var node in storyGraph.GetNodes())
            {
                graphSTR = graphSTR.Insert(graphSTR.Length, node.GetNumberInSequence() +
                        " [shape =" + '"' + "circle" + '"' + "label =" + '"' + " " + node.GetActiveAgent().Key.GetName().ToString() + '"' + "] \r\n");
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

                        if (edge.GetAction() != null && edge.GetLowerNode() != null)
                        {
                            graphSTR = graphSTR.Insert(graphSTR.Length,
                                edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                + "[label = " + '"' + edge.GetAction().ToString().Remove(0, 20) + '"' + "] \r\n");
                        }
                        else if (edge.GetAction() != null && edge.GetLowerNode() == null)
                        {
                            graphSTR = graphSTR.Insert(graphSTR.Length,
                                edge.GetUpperNode().GetNumberInSequence() + "->" + "null"
                                + "[label = " + '"' + edge.GetAction().ToString().Remove(0, 20) + '"' + "] \r\n");
                        }
                        else if (edge.GetAction() == null && edge.GetUpperNode() != null && edge.GetLowerNode() != null)
                        {
                            graphSTR = graphSTR.Insert(graphSTR.Length,
                                edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                + "[label = " + '"' + "not action" + '"' + "] \r\n");
                        }
                        

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

                        graphSTR = graphSTR.Insert(graphSTR.Length, edge.GetUpperNode().GetNumberInSequence() + "->" + "End"
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
