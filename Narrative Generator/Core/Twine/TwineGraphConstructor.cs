using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class for converting a story graph into an interactive story for Twine.
    /// </summary>
    class TwineGraphConstructor
    {
        OpenAI openAI = new OpenAI();
        
        public string ApiKey { get; set; }

        public bool OpenAIGeneration { get; set; }

        /// <summary>
        /// A method for converting a story graph into an interactive story for Twine.
        /// </summary>
        /// <param name="storyGraph">The generated graph.</param>
        /// <param name="txtOutputPath">The path where the resulting file will be created.</param>
        public void CreateTwineGraph (StoryGraph storyGraph, string txtOutputPath)
        {
            string twineGraphCode = "";
            int positionX = 0;
            int positionY = 0;
            string questName = storyGraph.GetRoot().GetWorldState().GetStaticWorldPart().GetSetting().ToString();
            string questSettings = Environment.NewLine + "name=\"" + questName + "\"" + Environment.NewLine + "startnode=\"0\"" + Environment.NewLine + ">";
            string styleSettings = Environment.NewLine + "<style role=\"stylesheet\" id=\"twine-user-stylesheet\" type=\"text/twine-css\">" +
                "tw-icon[title=\"Undo\"], tw-icon[title=\"Redo\"] { display: none; }</style>";

            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, "<tw-storydata" + questSettings + styleSettings);

            HashSet<StoryNode> storyNodes = new HashSet<StoryNode>();
            bool startNode = false;

            foreach (var node in storyGraph.GetNodes())
            {
                if (node.GetActivePlayer() || node.goalState)
                {
                    if (node.GetEdges().Count == 0) { continue; }

                    if (storyNodes.Contains(node.GetEdge(0).GetUpperNode()) && !node.goalState) { continue; }

                    if (!startNode) { startNode = true; node.GetEdge(0).GetUpperNode().SetNumberInSequence(0); }

                    if (node.GetActivePlayer() && !node.goalState)
                    {
                        //AddNode(ref positionX, ref positionY, node.GetEdge(0).GetUpperNode(), ref twineGraphCode);

                        foreach (var edge in node.GetEdges())
                        {
                            if (edge.GetLowerNode().Equals(node))
                            {
                                AddNode(ref positionX, ref positionY, edge.GetUpperNode(), ref twineGraphCode);
                            }
                        }
                    }
                    else if (node.goalState)
                    {
                        AddNode(ref positionX, ref positionY, node.GetEdge(0).GetLowerNode(), ref twineGraphCode);
                    }

                    storyNodes.Add(node.GetEdge(0).GetUpperNode());
                }
            }

            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, "</tw-storydata>");

            CreateHTMLFileWithGame(twineGraphCode, storyGraph.GetRoot().GetWorldState().GetStaticWorldPart().GetSetting(), txtOutputPath);
        }

        /// <summary>
        /// A method that creates text for an individual node.
        /// </summary>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="node">Described node.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        public void AddNode (ref int positionX, ref int positionY, StoryNode node, ref string twineGraphCode)
        {
            Random random = new Random();
            int r = random.Next(2);
            if (r == 0) { positionX += 200; } else { positionX -= 200; }
            positionY += 400;
            int nodeID = node.GetNumberInSequence();
            string nodeContent = "";

            nodeContent = nodeContent.Insert(nodeContent.Length, "<tw-passagedata pid=\"" + nodeID + "\" name=\"Node " + nodeID + "\" tags=\"\" " +
                "position=\"" + positionX + "," + positionY + "\" size=\"200,200\">" + AddTextAsync(node).Result);

            if (!node.goalState)
            {
                foreach (var edge in node.GetEdges())
                {
                    AddEdge(node, edge, ref nodeContent, ref positionX, ref positionY, ref twineGraphCode);
                }
            }

            nodeContent = nodeContent.Insert(nodeContent.Length, " </tw-passagedata>");
            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, Environment.NewLine + nodeContent);
        }

        /// <summary>
        /// A method for creating special nodes that are not originally present in the graph.
        /// </summary>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="nodeNumber">Node number to create.</param>
        /// <param name="nextNodeNumber">Node number with which the created node will be connected.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        /// <param name="text">The text that will be displayed when the generated graph is reached.</param>
        public void AddSpecialNode (ref int positionX, 
                                    ref int positionY, 
                                    int nodeNumber, 
                                    int nextNodeNumber, 
                                    ref string twineGraphCode, 
                                    string text)
        {
            Random random = new Random();
            int r = random.Next(2);
            if (r == 0) { positionX += 200; } else { positionX -= 200; }
            positionY += 400;
            string nodeContent = "";

            nodeContent = nodeContent.Insert(nodeContent.Length, "<tw-passagedata pid=\"" + nodeNumber + "\" name=\"Node " + nodeNumber + "\" tags=\"\" " +
                "position=\"" + positionX + "," + positionY + "\" size=\"200,200\">");

            nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + text);

            nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[[" + "Continue" + " -&gt;Node " + nextNodeNumber + "]]");

            nodeContent = nodeContent.Insert(nodeContent.Length, " </tw-passagedata>");
            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, Environment.NewLine + nodeContent);
        }

        /// <summary>
        /// A method for creating a node that is used to create a contextual relationship between other nodes.
        /// </summary>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="nodeNumber">Node number to create.</param>
        /// <param name="nextNodeNumber">Node number with which the created node will be connected.</param>
        /// <param name="action">The action (counteraction) that created connection.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        /// <param name="succsess">Information about the success or failure of an action.</param>
        /// <param name="node">The source node.</param>
        public void AddIntermediateNode (ref int positionX, ref int positionY, int nodeNumber, int nextNodeNumber, 
                                         string action, ref string twineGraphCode, bool succsess, StoryNode node)
        {
            string text = "You were unable to perform this action. " +
                "Circumstances have developed in such a way that you performed another action - " + action;

            if (succsess) { text = text.Insert(text.Length, Environment.NewLine + "And you have completed this action successfully."); }
            else { text = text.Insert(text.Length, Environment.NewLine + "And this action was unsuccessful."); }

            if (succsess && action.Equals("InvestigateRoom"))
            {
                text = text.Insert(text.Length, Environment.NewLine + "You find evidence pointing to the killer! It's... " +
                                         InsertSpaces(node.GetWorldState().GetAgentByRole(AgentRole.ANTAGONIST).Key.GetName()));
            }
            else if (succsess && action.Equals("Fight"))
            {
                text = text.Insert(text.Length, Environment.NewLine + "You fought and you won!");
            }

            AddSpecialNode (ref positionX, ref positionY, nodeNumber, nextNodeNumber, ref twineGraphCode, text);
        }

        /// <summary>
        /// An optional node that describes the result of the search evidences action and provides details.
        /// </summary>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="nodeNumber">Node number to create.</param>
        /// <param name="nextNodeNumber">Node number with which the created node will be connected.</param>
        /// <param name="succsess">Information about the success or failure of an action.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        /// <param name="node">The source node.</param>
        public void AddInvestigateNode (ref int positionX, ref int positionY, int nodeNumber, int nextNodeNumber, 
                                        bool succsess, ref string twineGraphCode, StoryNode node)
        {
            string text = "";

            if (succsess) { text = Environment.NewLine + "You managed to find evidence pointing to the killer! It's... " + 
                                                     InsertSpaces(node.GetWorldState().GetAgentByRole(AgentRole.ANTAGONIST).Key.GetName()); }
            else { text = Environment.NewLine + "This time around, your search turned up nothing, and you couldn't find any clues."; }

            AddSpecialNode(ref positionX, ref positionY, nodeNumber, nextNodeNumber, ref twineGraphCode, text);
        }

        /// <summary>
        /// An optional node that describes the result of the fight action and provides details.
        /// </summary>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="nodeNumber">Node number to create.</param>
        /// <param name="nextNodeNumber">Node number with which the created node will be connected.</param>
        /// <param name="succsess">Information about the success or failure of an action.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        /// <param name="action">The action (counteraction) that created connection.</param>
        public void AddFightNode (ref int positionX, ref int positionY, int nodeNumber, int nextNodeNumber,
                                  bool succsess, ref string twineGraphCode, Fight action)
        {
            string text = "";

            text = Environment.NewLine + "You fought with " + InsertSpaces(action.Agent2.Key.GetName()) + " and you won!";

            AddSpecialNode(ref positionX, ref positionY, nodeNumber, nextNodeNumber, ref twineGraphCode, text);
        }

        /// <summary>
        /// A method that creates a representation of an edge in a graph - in Twine it is a connection between nodes.
        /// </summary>
        /// <param name="node">The upper node of the edge.</param>
        /// <param name="edge">The source edge.</param>
        /// <param name="nodeContent">The generated node description to which the currently generated edge description will be added.</param>
        /// <param name="positionX">Position along the X axis in the Twine editor.</param>
        /// <param name="positionY">Position along the Y axis in the Twine editor.</param>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        public void AddEdge (StoryNode node, Edge edge, ref string nodeContent, ref int positionX, ref int positionY, ref string twineGraphCode) 
        {
            if (edge.GetUpperNode().Equals(node)) // We make sure that we do not go along the upper edge.
            {
                string desc = " ";

                if (edge.GetAction() is Move)
                {
                    desc = " to " + InsertSpaces(((Move)edge.GetAction()).To.Key.GetName());
                }
                else if (edge.GetAction() is Talk)
                {
                    desc = " with " + InsertSpaces(((Talk)edge.GetAction()).Agent2.Key.GetName());
                }
                else if (edge.GetAction() is Fight)
                {
                    desc = " with " + InsertSpaces(((Fight)edge.GetAction()).Agent2.Key.GetName());
                }
                else if (edge.GetAction() is InvestigateRoom)
                {
                    desc = " " + InsertSpaces(((InvestigateRoom)edge.GetAction()).Location.Key.GetName());
                }

                int charDeleteCounter = 20;
                int midNodeNumber = (node.GetNumberInSequence() + 10000);

                if (edge.GetAction().GetType().ToString().Contains("Counter"))
                {

                    if (edge.GetAction().ReturnOriginal().Key.Equals("Move"))
                    {
                        desc = " to " + InsertSpaces(((Move)edge.GetAction().ReturnOriginal().Value).To.Key.GetName());
                    }
                    else if (edge.GetAction().ReturnOriginal().Key.Equals("Talk"))
                    {
                        desc = " with " + InsertSpaces(((Talk)edge.GetAction().ReturnOriginal().Value).Agent2.Key.GetName());
                    }
                    else if (edge.GetAction().ReturnOriginal().Key.Equals("Fight"))
                    {
                        desc = " with " + InsertSpaces(((Fight)edge.GetAction().ReturnOriginal().Value).Agent2.Key.GetName());
                    }
                    else if (edge.GetAction().ReturnOriginal().Key.Equals("InvestigateRoom"))
                    {
                        desc = " " + InsertSpaces(((InvestigateRoom)edge.GetAction().ReturnOriginal().Value).Location.Key.GetName());
                    }

                    nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[[" +
                        InsertSpaces(edge.GetAction().ReturnOriginal().Key)
                        + InsertSpaces(desc) + " -&gt;Node " + midNodeNumber.ToString() + "]]");

                    charDeleteCounter = 27;

                    AddIntermediateNode(ref positionX, ref positionY, midNodeNumber, SearchNextNode(edge.GetLowerNode()), 
                                        InsertSpaces(edge.GetAction().GetType().ToString().Remove(0, charDeleteCounter)), 
                                        ref twineGraphCode, edge.GetAction().success, node);
                }
                else
                {
                    if (edge.GetAction() is InvestigateRoom)
                    {
                        nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[["
                            + InsertSpaces(edge.GetAction().GetType().ToString().Remove(0, charDeleteCounter))
                            + InsertSpaces(desc) + " -&gt;Node " + midNodeNumber.ToString() + "]]");

                        AddInvestigateNode(ref positionX, ref positionY, midNodeNumber, SearchNextNode(edge.GetLowerNode()),
                                            edge.GetAction().success, ref twineGraphCode, node);
                    }
                    else if (edge.GetAction() is Fight && edge.GetAction().success)
                    {
                        nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[["
                            + InsertSpaces(edge.GetAction().GetType().ToString().Remove(0, charDeleteCounter))
                            + InsertSpaces(desc) + " -&gt;Node " + midNodeNumber.ToString() + "]]");

                        AddFightNode(ref positionX, ref positionY, midNodeNumber, SearchNextNode(edge.GetLowerNode()), edge.GetAction().success,
                                     ref twineGraphCode, (Fight)edge.GetAction());
                    }
                    else
                    {
                        nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[[" 
                            + InsertSpaces(edge.GetAction().GetType().ToString().Remove(0, charDeleteCounter))
                            + InsertSpaces(desc) + " -&gt;Node " + SearchNextNode(edge.GetLowerNode()) + "]]");
                    }
                }
            }
        }

        /// <summary>
        /// Method for finding the next node in which the right to act is passed to the player.
        /// </summary>
        /// <param name="currentNode">The currently considered node from which the edge (link) will radiate.</param>
        /// <returns>Looking node number.</returns>
        public int SearchNextNode (StoryNode currentNode)
        {
            int number = -1;
            StoryNode testedNode = currentNode;

            if (testedNode.GetActivePlayer())
            {
                if (currentNode.GetWorldState().GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge)
                    && !testedNode.GetLastEdge().GetLowerNode().GetActivePlayer())
                {
                    testedNode = testedNode.GetLastEdge().GetLowerNode();
                    number = testedNode.GetNumberInSequence();
                }
                else
                {
                    if (currentNode.GetWorldState().GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge))
                    {
                        number = testedNode.GetNumberInSequence();
                        return number;
                    }
                    else
                    {
                        testedNode = testedNode.GetLastEdge().GetLowerNode();
                        number = testedNode.GetNumberInSequence();
                    }
                }
            }

            while (!testedNode.GetActivePlayer())
            {
                if (testedNode.goalState) { number = testedNode.GetNumberInSequence(); break; }
                else testedNode = testedNode.GetEdge(1).GetLowerNode();

                if (currentNode.GetWorldState().GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge))
                {
                    if (testedNode.GetActivePlayer()) { return number; }
                    number = testedNode.GetNumberInSequence();
                }
                else
                {
                    if (testedNode.GetActivePlayer()) { number = testedNode.GetFirstEdge().GetUpperNode().GetNumberInSequence(); }
                }
            }

            return number;
        }

        /// <summary>
        /// Method for creating a description of the reached node.
        /// </summary>
        /// <param name="node">The node for which the description is generated.</param>
        /// <returns>Description of the node in text format.</returns>
        public async Task<string> AddTextAsync (StoryNode node)
        {
            string text = "";

            int edgeNumber = 1;

            if (node.GetEdges().Count == 1) { edgeNumber = 0; }

            if (node.goalState)
            {
                if (node.GetActiveAgent().Key.GetRole().Equals(AgentRole.ANTAGONIST) ||
                    node.GetActiveAgent().Key.GetRole().Equals(AgentRole.ENEMY))
                {
                    text = text.Insert(text.Length, Environment.NewLine + "You have been killed " + InsertSpaces(node.GetActiveAgent().Key.GetName()) + " and lost.");
                }
                else if (node.GetActiveAgent().Key.GetRole().Equals(AgentRole.PLAYER))
                {
                    text = text.Insert(text.Length, Environment.NewLine + "You coped with the villain and won!");
                }
                else
                {
                    text = text.Insert(text.Length, Environment.NewLine + "With or without your help, the villains were defeated. Game over.");
                }

                text = text.Insert(text.Length, Environment.NewLine + "[[" + "Restart" + " -&gt;Node 0]]");
            }
            else
            {
                text = text.Insert(text.Length, /*Environment.NewLine +*/ "You are in a location: " +
                InsertSpaces(((KeyValuePair<AgentStateStatic, AgentStateDynamic>)node.GetEdge(edgeNumber).GetAction().Arguments[0]).Value.GetMyLocation().GetName()));

                if (node.GetWorldState().GetLocationByName(((KeyValuePair<AgentStateStatic, AgentStateDynamic>)node.GetEdge(edgeNumber).GetAction().Arguments[0]).Value.GetMyLocation().GetName()).Value.GetAgents().Count > 1)
                {
                    text = text.Insert(text.Length, /*Environment.NewLine +*/ ", the following people are also here: ");

                    foreach (var agent in node.GetWorldState().GetLocationByName(((KeyValuePair<AgentStateStatic, AgentStateDynamic>)node.GetEdge(edgeNumber).GetAction().Arguments[0]).Value.GetMyLocation().GetName()).Value.GetAgents())
                    {
                        if (!agent.Key.GetRole().Equals(AgentRole.PLAYER))
                        {
                            text = text.Insert(text.Length, InsertSpaces(agent.Key.GetName()));

                            if (!agent.Key.Equals
                                (node.GetWorldState().GetLocationByName(((KeyValuePair<AgentStateStatic, AgentStateDynamic>)node.GetEdge(edgeNumber).
                                GetAction().Arguments[0]).Value.GetMyLocation().GetName()).Value.GetAgents().Last().Key))
                            {
                                text = text.Insert(text.Length, ", ");
                            }
                        }
                    }
                }
                else
                {
                    text = text.Insert(text.Length, /*Environment.NewLine +*/ ", there is no one except you. ");
                }

                if (OpenAIGeneration)
                {
                    var generatedText = OpenAI.CompletionRequest(text, ApiKey).Result;

                    text = generatedText;
                }

                text = text.Insert(text.Length, Environment.NewLine + Environment.NewLine + "You can perform the following actions: ");
            }            

            return text;
        }

        /// <summary>
        /// A method that separates compound words into separate ones, using capital letters as a separator.
        /// </summary>
        /// <param name="text">The text to change.</param>
        /// <returns>Changed text.</returns>
        public string InsertSpaces (string text)
        {
            for (int i = 0; i < text.Count(); i++)
            {
                char chr = text[i];
                string upperChr = chr.ToString().ToUpper();

                if ((chr.ToString() == upperChr) && i != 0)
                {
                    string newText = text.Insert(i, " ");
                    text = newText;
                    i++;
                }
            }

            return text;
        }

        /// <summary>
        /// Method for writing the generated text and html markup to a file of the corresponding format.
        /// </summary>
        /// <param name="twineGraphCode">The text of the file to be created.</param>
        /// <param name="setting">The name of the setting in which the story was generated is used in the file name.</param>
        /// <param name="path">The path where the file will be created.</param>
        public void CreateHTMLFileWithGame (string twineGraphCode, Setting setting, string path)
        {           
            string fileName = setting.ToString() + "Quest";

            FileStream file = new FileStream(path + "\\" + fileName + ".html", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            streamWriter.WriteLine(twineGraphCode);
            streamWriter.Close();
        }
    }
}
