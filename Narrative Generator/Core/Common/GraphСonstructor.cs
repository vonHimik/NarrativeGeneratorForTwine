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
        /// <summary>
        /// A method that describes the transmitted story graph in text format and creates a visualization based on it.
        /// </summary>
        public void CreateGraph(StoryGraph storyGraph, string graphName)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            string graphSTR = "digraph G { \r\n";

            // Generating a list of nodes.
            foreach (var node in storyGraph.GetNodes())
            {
                // We compose a line with information about the node under consideration.
                if (node.goalState)
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, node.GetNumberInSequence() +
                    " [shape =" + '"' + "circle" + '"' + " label =" + '"' + " "
                      + node.GetNumberInSequence() + '"'
                      + " " + "style = " + '"' + "filled" + '"' + " fillcolor = "  + '"' + "yellow" + '"'
                      + "] \r\n");
                }
                else if (node.skiped)
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, node.GetNumberInSequence() +
                        " [shape =" + '"' + "circle" + '"' + " label =" + '"' + " "
                        + node.GetNumberInSequence() + '"'
                        + " " + "style = " + '"' + "filled" + '"' + " fillcolor = " + '"' + "green" + '"'
                        + "] \r\n");
                }
                else if (node.counteract)
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, node.GetNumberInSequence() +
                        " [shape =" + '"' + "circle" + '"' + " label =" + '"' + " "
                        + node.GetNumberInSequence() + '"'
                        + " " + "style = " + '"' + "filled" + '"' + " fillcolor = " + '"' + "aquamarine" + '"'
                        + "] \r\n");
                }
                else
                {
                    graphSTR = graphSTR.Insert(graphSTR.Length, node.GetNumberInSequence() +
                    " [shape =" + '"' + "circle" + '"' + " label =" + '"' + " "
                      + node.GetNumberInSequence()
                      + '"' + "] \r\n");
                }
            }

            // Generating graph edges.
            foreach (var node in storyGraph.GetNodes())
            {
                // If the node under consideration is not the last.
                if (node != storyGraph.GetLastNode())
                {
                    // Then we go along the edges attached to this node.
                    foreach (var edge in node.GetEdges())
                    {
                        bool skip = false;

                        // We go through the list of already traversed edges and compare 
                        //   if we have previously met the edge that we are considering now.
                        foreach (var e in edges)
                        {
                            if (e.Equals(edge))
                            {
                                skip = true;
                                break;
                            }
                        }

                        // If already met, then move on to the next one.
                        if (skip) { continue; }

                        // If the edge has an attached action and the bottom end is connected to some node.
                        if (edge.GetAction() != null && edge.GetLowerNode() != null)
                        {
                            // Create a line with information about the edge.
                            if (edge.GetAction() is Kill)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine 
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString() 
                                    + '"' + " color = " + '"' + " red" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Entrap)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString() 
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Move)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString() 
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is UnexpectedObstacle)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + " color = " + '"' + " green" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is InvestigateRoom)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length, edge.GetUpperNode().GetNumberInSequence()
                                    + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + " color = " + '"' + " blue" + '"' + "] \r\n");
                            }
                            else
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString() 
                                    + '"' + "] \r\n");
                            }
                        }
                        // If the edge has an attached action, but the bottom end is not attached to a node.
                        else if (edge.GetAction() != null && edge.GetLowerNode() == null)
                        {
                            if (edge.GetAction() is Kill)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length, 
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Entrap)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Move)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is InvestigateRoom)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().ToString().Remove(0, 20)
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,  
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                   + "[label = " + " " + '"' 
                                   + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                   + Environment.NewLine
                                   + " " + edge.GetAction().ToString().Remove(0, 20)
                                   + Environment.NewLine
                                   + " " + edge.GetAction().success.ToString() 
                                   + '"' + "] \r\n");
                            }
                        }

                        // Add the considered edge to the list.
                        edges.Add(edge);
                    }
                }
                // We separately process the case when the considered node is the last one.
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

            // Save the resulting graph to a text file.
            SaveGraph(graphName, graphSTR);

           // Then we render the resulting graph.
           // PrintGraph(graphName);
        }

        /// <summary>
        /// A method that saves the textual description of the graph to a file with the specified name.
        /// </summary>
        public void SaveGraph(string fileName, string graph)
        {
            FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));
            streamWriter.Write(graph);
            streamWriter.Close();
        }

        public void PrintGraph(string fileName)
        {
            Graphviz graphviz = new Graphviz();
            graphviz.Run(fileName);
        }
    }
}
