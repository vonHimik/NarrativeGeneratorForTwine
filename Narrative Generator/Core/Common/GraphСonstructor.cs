using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that controls the conversion of the story graph to dot format.
    /// </summary>
    class GraphСonstructor
    {
        /// <summary>
        /// Active settings marker: whether to hide "empty" actions.
        /// </summary>
        bool hideNothingToDoActios;

        /// <summary>
        /// Activator settings to hide "empty" actions.
        /// </summary>
        public void HideNTDActions() { hideNothingToDoActios = true; }
        /// <summary>
        /// Deactivator settings to hide "empty" actions.
        /// </summary>
        public void ShowNTDActions() { hideNothingToDoActios = false; }

        Graphviz graphviz = new Graphviz();

        /// <summary>
        /// A method that describes the transmitted story graph in text format and creates a visualization based on it.
        /// </summary>
        /// <param name="storyGraph">Story graph, which is a collection of nodes connected by oriented edges.</param>
        /// <param name="filePath">The path to the saved files.</param>
        /// <param name="graphName">The name of the saved files.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void CreateGraph (StoryGraph storyGraph, string filePath, string graphName, ref TextBox note)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            string graphSTR = "digraph G { \r\n";

            // Generating a list of nodes.
            foreach (var node in storyGraph.GetNodes())
            {
                bool skip = false;

                foreach (var edge in node.GetEdges())
                {
                    if (edge.GetAction() is NothingToDo && edge.GetLowerNode().Equals(node))
                    {
                        skip = true;
                    }
                }

                if (skip && hideNothingToDoActios) { continue; }

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

            if (hideNothingToDoActios)
            {
                foreach (var nodeFix in storyGraph.GetNodes())
                {
                    // Going through all the edges attached to the node
                    for (int i = 0; i < nodeFix.GetEdges().Count; i++)
                    {
                        // If the edge with the action Do Nothing
                        if (nodeFix.GetEdges().ElementAt(i).GetAction() is NothingToDo)
                        {
                            // If the current node is the upper node on this edge
                            if (nodeFix.GetEdges().ElementAt(i).GetUpperNode().Equals(nodeFix)) {}
                            // If the current node is the lower one on this edge
                            else if (nodeFix.GetEdges().ElementAt(i).GetLowerNode().Equals(nodeFix))
                            {
                                int edgeCounter = nodeFix.GetEdges().Count;

                                // Again we go through all the egdes attached to the node
                                for (int j = 0; j < edgeCounter; j++)
                                {
                                    // Select those edges where the current node is the upper one
                                    if (nodeFix.GetEdges().ElementAt(j).GetUpperNode().Equals(nodeFix))
                                    {
                                        // We change for these nodes the link to the upper node from the current one, 
                                        // to the one that is higher than it in the edge where it is lower.
                                        StoryNode upperNode = nodeFix.GetEdges().ElementAt(i).GetUpperNode();
                                        nodeFix.GetEdges().ElementAt(j).SetUpperNode(ref upperNode);

                                        // We add a link to the edge to the new node, delete it from the old one.
                                        upperNode.AddEdge(nodeFix.GetEdges().ElementAt(j));
                                        upperNode.RemoveEdge(nodeFix.GetEdges().ElementAt(i));

                                        nodeFix.RemoveEdge(nodeFix.GetEdges().ElementAt(j));

                                        edgeCounter = nodeFix.GetEdges().Count;
                                        j--;
                                    }
                                }

                                nodeFix.GetEdges().ElementAt(i).ClearEdge();
                            }
                        }
                    }
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

                        string originalAction = "";
                        if (edge.GetAction() is CounterMove)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterMove)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterTalk)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterTalk)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterNeutralizeKiller)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterNeutralizeKiller)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterInvestigateRoom)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterInvestigateRoom)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterEntrap)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterEntrap)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterFight)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterFight)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterKill)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterKill)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterTellAboutASuspicious)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterTellAboutASuspicious)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterRun)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterRun)edge.GetAction()).OriginalAction.Key;
                        }
                        else if (edge.GetAction() is CounterReassure)
                        {
                            originalAction = Environment.NewLine + " Original action: " + ((CounterReassure)edge.GetAction()).OriginalAction.Key;
                        }

                        // If the edge has an attached action and the bottom end is connected to some node.
                        if (edge.GetAction() != null && edge.GetLowerNode() != null)
                        {
                            // Create a line with information about the edge.
                            if (edge.GetAction() is Kill || edge.GetAction() is CounterKill)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString() 
                                    + '"' + " color = " + '"' + " red" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Entrap || edge.GetAction() is CounterEntrap)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString() 
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Move || edge.GetAction() is CounterMove)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString() 
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is InvestigateRoom || edge.GetAction() is CounterInvestigateRoom)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length, edge.GetUpperNode().GetNumberInSequence()
                                    + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + " color = " + '"' + " blue" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Fight || edge.GetAction() is CounterFight)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "with: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + " color = " + '"' + " red" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is NeutralizeKiller || edge.GetAction() is CounterNeutralizeKiller)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + " color = " + '"' + " red" + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Reassure || edge.GetAction() is CounterReassure)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "who: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Run || edge.GetAction() is CounterRun)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Talk || edge.GetAction() is CounterTalk)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "with: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Value.GetBeliefs().GetMyLocation().GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is TellAboutASuspicious || edge.GetAction() is CounterTellAboutASuspicious)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "who: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[1]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "from: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "to: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[3]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + edge.GetLowerNode().GetNumberInSequence()
                                    + "[label = " + " " + '"' 
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "Success: " + edge.GetAction().success.ToString() 
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
                                    + " " + edge.GetAction().WriteDescription() + originalAction
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
                                    + " " + edge.GetAction().WriteDescription() + originalAction
                                    + Environment.NewLine
                                    + " " + "whom: " + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + "where: " + ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().success.ToString()
                                    + '"' + "] \r\n");
                            }
                            else if (edge.GetAction() is Move || edge.GetAction() is CounterMove)
                            {
                                graphSTR = graphSTR.Insert(graphSTR.Length,
                                    edge.GetUpperNode().GetNumberInSequence() + "->" + "END"
                                    + "[label = " + " " + '"'
                                    + ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.GetName()
                                    + Environment.NewLine
                                    + " " + edge.GetAction().WriteDescription() + originalAction
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
                                    + " " + edge.GetAction().WriteDescription() + originalAction
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
                                   + " " + edge.GetAction().WriteDescription() + originalAction
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
                            + "[label = " + '"' + edge.GetAction().WriteDescription() + '"' + "] \r\n");

                        edges.Add(edge);
                    }
                }
            }

            graphSTR = graphSTR.Insert(graphSTR.Length, "}");

            // Save the resulting graph to a text file.
            SaveGraph(filePath, graphName, graphSTR, ref note);
        }

        public void GraphVisualization (string path, string name, ref TextBox note)
        {
            note.Text = "VISUALIZATION STARTING";
            graphviz.Run(path, name, ref note);
        }

        /// <summary>
        /// A method that saves the textual description of the graph to a file with the specified name.
        /// </summary>
        /// <param name="filePath">The path to the saved files.</param>
        /// <param name="fileName">The name of the saved files.</param>
        /// <param name="graph">Generated description of the story graph in dot format.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void SaveGraph(string filePath, string fileName, string graph, ref TextBox note)
        {
            note.Text = "GRAPH SAVING IN FILE";

            FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));
            streamWriter.Write(graph);
            streamWriter.Close();

            GraphVisualization(filePath, fileName, ref note);
        }
    }
}
