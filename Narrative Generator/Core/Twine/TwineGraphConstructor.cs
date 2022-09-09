using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class TwineGraphConstructor
    {

        public void CreateTwineGraph (StoryGraph storyGraph)
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
                //if (node.goalState) { continue; }

                if (node.GetActivePlayer() || node.goalState)
                {
                    if (storyNodes.Contains(node.GetEdge(0).GetUpperNode())) { continue; }

                    if (!startNode) { startNode = true; node.GetEdge(0).GetUpperNode().SetNumberInSequence(0); }

                    if (node.GetActivePlayer() && !node.goalState)
                    {
                        AddNode(ref positionX, ref positionY, node.GetEdge(0).GetUpperNode(), ref twineGraphCode);
                    }
                    else if (node.goalState)
                    {
                        AddNode(ref positionX, ref positionY, node.GetEdge(0).GetLowerNode(), ref twineGraphCode);
                    }

                    storyNodes.Add(node.GetEdge(0).GetUpperNode());
                }
            }

            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, "</tw-storydata>");

            CreateHTMLFileWithGame(twineGraphCode, storyGraph.GetRoot().GetWorldState().GetStaticWorldPart().GetSetting());
        }

        public void AddNode (ref int positionX, ref int positionY, StoryNode node, ref string twineGraphCode)
        {
            Random random = new Random();
            int r = random.Next(2);
            if (r == 0) { positionX += 200; } else { positionX -= 200; }
            positionY += 400;
            int nodeID = node.GetNumberInSequence();
            string nodeContent = "";

            nodeContent = nodeContent.Insert(nodeContent.Length, "<tw-passagedata pid=\"" + nodeID + "\" name=\"Node " + nodeID + "\" tags=\"\" " +
                "position=\"" + positionX + "," + positionY + "\" size=\"200,200\">" + AddText(node));

            if (!node.goalState)
            {
                foreach (var edge in node.GetEdges())
                {
                    AddEdge(node, edge, ref nodeContent);
                }
            }

            nodeContent = nodeContent.Insert(nodeContent.Length, " </tw-passagedata>");
            twineGraphCode = twineGraphCode.Insert(twineGraphCode.Length, Environment.NewLine + nodeContent);
        }

        public void AddEdge (StoryNode node, Edge edge, ref string nodeContent) 
        {
            if (edge.GetUpperNode().Equals(node) && !edge.GetLowerNode().goalState) // Убеждаемся, что не идём по верхней грани.
            {
                string desc = " ";

                if (edge.GetAction() is Move)
                {
                    desc = " to " + ((Move)edge.GetAction()).To.Key.GetName();
                }
                else if (edge.GetAction() is CounterMove)
                {
                    desc = " to " + ((CounterMove)edge.GetAction()).To.Key.GetName();
                }
                else if (edge.GetAction() is Talk)
                {
                    desc = " with " + ((Talk)edge.GetAction()).Agent2.Key.GetName();
                }
                else if (edge.GetAction() is InvestigateRoom)
                {
                    desc = " " + ((InvestigateRoom)edge.GetAction()).Location.Key.GetName();
                }

                nodeContent = nodeContent.Insert(nodeContent.Length, Environment.NewLine + "[[" + edge.GetAction().GetType().ToString().Remove(0, 20) + desc +
                    " -&gt;Node " + searchNextNode(edge.GetLowerNode()) + "]]");
            }
        }

        public int searchNextNode (StoryNode currentNode)
        {
            // TO DO: repair it, now here are bugs and it didn't work. :(
            int number = -1;
            StoryNode testedNode = currentNode;

            if (testedNode.GetActivePlayer() && !testedNode.goalState) { testedNode = testedNode.GetLastEdge().GetLowerNode(); }

            while (!testedNode.GetActivePlayer())
            {
                if (testedNode.goalState) { number = testedNode.GetNumberInSequence(); break; }
                //else if (testedNode.GetActivePlayer()) { number = testedNode.GetEdge(0).GetUpperNode().GetNumberInSequence(); }
                else testedNode = testedNode.GetEdge(1).GetLowerNode();

                if (testedNode.GetActivePlayer()) { number = testedNode.GetFirstEdge().GetUpperNode().GetNumberInSequence(); }
            }

            return number;
        }

        public string AddText (StoryNode node)
        {
            string text = "";

            text = text.Insert(text.Length, Environment.NewLine + "You are in a location: " + node.GetActiveAgent().Value.GetMyLocation().GetName() + Environment.NewLine 
                                                + "The following people are also here: ");

            foreach (var agent in node.GetWorldState().GetLocationByName(node.GetActiveAgent().Value.GetMyLocation().GetName()).Value.GetAgents())
            {
                if (!agent.Key.GetRole().Equals(AgentRole.PLAYER))
                {
                    text = text.Insert(text.Length, agent.Key.GetName() + " ");
                }
            }

            if (!node.goalState)
            {
                text = text.Insert(text.Length, Environment.NewLine + "You can perform the following actions: ");
            }
            else
            {
                text = text.Insert(text.Length, Environment.NewLine + "END");
            }

            return text;
        }

        public void CreateHTMLFileWithGame (string twineGraphCode, Setting setting)
        {
            string fileName = setting.ToString() + "Quest";

            FileStream file = new FileStream(fileName + ".html", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            streamWriter.WriteLine(twineGraphCode);
            streamWriter.Close();
        }
    }
}
