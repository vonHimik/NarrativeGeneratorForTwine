using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rivers;
using Rivers.Serialization.Dot;

namespace Narrative_Generator
{
    class RiversWrapper
    {
        public Graph CreateRiversGraph(StoryGraph storyGraph, string graphName)
        {
            var graph = new Graph();
            graph.Name = graphName;

            int counter = 0;
            string nodeName1 = "";
            string nodeName2 = "";
            string inActionName = "";
            string inActionRole = "";
            string action = "";

            foreach (var node in storyGraph.GetNodes())
            {
                counter++;

                inActionName = node.GetActiveAgent().Key.GetName();
                inActionRole = node.GetActiveAgent().Key.GetRole().ToString();
                action = node.GetEdges().First().GetAction().GetType().ToString().Remove(0, 20);

                if (nodeName1 == "" && nodeName2 == "")
                {
                    nodeName1 = "On the move: " + inActionName + " Role: " + inActionRole + " Action: " + action;
                    graph.Nodes.Add(new Node(nodeName1));
                }
                else if (nodeName1 != "" && nodeName2 == "")
                {
                    nodeName2 = "On the move: " + inActionName + " Role: " + inActionRole + " Action: " + action;
                    graph.Nodes.Add(new Node(nodeName2));

                    if (counter > 1)
                    {
                        graph.Edges.Add(new Rivers.Edge(graph.Nodes[nodeName1], graph.Nodes[nodeName2]));
                    }

                    nodeName1 = "";
                }
                else if (nodeName1 == "" && nodeName2 != "")
                {
                    nodeName1 = "On the move: " + inActionName + " Role: " + inActionRole + " Action: " + action;
                    graph.Nodes.Add(new Node(nodeName1));

                    if (counter > 1)
                    {
                        graph.Edges.Add(new Rivers.Edge(graph.Nodes[nodeName2], graph.Nodes[nodeName1]));
                    }

                    nodeName2 = "";
                }
            }

            return graph;
        }

        public void CreateDotFile(Graph graph)
        {
            FileStream file = new FileStream(graph.Name + ".dot", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));
            var dotWriter = new DotWriter(streamWriter);
            dotWriter.Write(graph);
            streamWriter.Close();
        }
    }
}
