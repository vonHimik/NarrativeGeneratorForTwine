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
        TwineGraph constructedGraph = new TwineGraph();

        // TO DO
        public void ConstructGraph(StoryGraph storyGraph)
        {
            CreateStoryTextLog(storyGraph);
        }

        // TO DO
        public void AddNode()
        {
            throw new NotImplementedException();
        }

        // TO DO
        public void AddEdge() // = answer option / action / link to another node
        {
            throw new NotImplementedException();
        }

        // TO DO
        public void AddText() // Into node
        {
            throw new NotImplementedException();
        }

        // TO DO
        public void CreateHTMLFileWithGame()
        {
            throw new NotImplementedException();
        }

        // Writing the story as text.
        public void CreateStoryTextLog(StoryGraph storyGraph)
        {
            FileStream file = new FileStream("StoryText [TEST].txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            string inActionName = "";
            string inActionRole = "";
            string action = "";
            List<string> actionParameters = new List<string>();
            string resultString = "";

            foreach (var node in storyGraph.GetNodes())
            {
                if (!node.GetActivePlayer() && !(node.Equals(storyGraph.GetNodes().Last())))
                {
                    inActionName = node.GetActiveAgent().Key.GetName();
                    inActionRole = node.GetActiveAgent().Key.GetRole().ToString();
                    action = node.GetEdges().First().GetAction().GetType().ToString().Remove(0, 20);

                    foreach (var parameter in node.GetEdges().First().GetAction().Arguments)
                    {
                        if (parameter.GetType() == node.GetActiveAgent().GetType())
                        {
                            KeyValuePair<AgentStateStatic, AgentStateDynamic> agentTypeParameter =
                                (KeyValuePair<AgentStateStatic, AgentStateDynamic>)parameter;

                            actionParameters.Add(agentTypeParameter.Key.GetName());
                        }
                        else if (parameter.GetType() == node.GetWorldState().GetRandomLocation().GetType())
                        {
                            KeyValuePair<LocationStatic, LocationDynamic> locationTypeParameter =
                                (KeyValuePair<LocationStatic, LocationDynamic>)parameter;

                            actionParameters.Add(locationTypeParameter.Key.GetName());
                        }
                    }

                    resultString = CreateText(inActionName, inActionRole, action, actionParameters);
                    streamWriter.WriteLine(resultString);
                    actionParameters.Clear();
                }
                else
                {
                    // TO DO
                }
            }

            streamWriter.Close();
        }

        public string CreateText(string inActionName, string inActionRole, string action, List<string> actionParameters)
        {
            Random random = new Random();

            string resultString = "";
            string text = "";
            int r = random.Next(1, 4);

            switch (action)
            {
                case "Move":
                    switch (r)
                    {
                        case 1: text = " leaves " + actionParameters[1] + " and enters in " + actionParameters[2];
                            break;
                        case 2: text = " looking around carefully, exits from " + actionParameters[1] + " and enters in " + actionParameters[2];
                            break;
                        case 3: text = " carelessly exits from " + actionParameters[1] + " and goes to " + actionParameters[2];
                            break;
                    }
                    break;
                case "Kill":
                    switch (r)
                    {
                        case 1: text = " quietly creeps up from behind to " + actionParameters[0] 
                                          + " and covering the victim's mouth with his hands, strangles his";
                            break;
                        case 2: text = " making sure that no one is around, he pulls out a pistol with a silencer and, taking aim, shoots at " 
                                          + actionParameters[0];
                            break;
                        case 3: text = " looking around to make sure that no one is around, pulls out a knife and, sneaking up to " 
                                          + actionParameters[0] + ", slits his throat.";
                            break;
                    }
                    break;
                case "Entrap":
                    switch (r)
                    {
                        case 1: text = " makes strange sounds in " + actionParameters[2] + ", hoping to lure " + actionParameters[0] + " here";
                            break;
                        case 2: text = " is constantly shown to the eyes " + actionParameters[0] + 
                                       " and immediately disappears around the corner, luring " + actionParameters[0] + " in " + actionParameters[2];
                            break;
                        case 3: text = " прячась от " + actionParameters[0] + " тонким голосом зазывает " + actionParameters[0];
                            break;
                    }
                    break;
                case "InvestigateRoom":
                    text = " scrutinizes " + actionParameters[2] + ", hoping to find some evidence against the killer";
                    break;
                case "Fight":
                    text = "";
                    break;
                case "NeutralizeKiller":
                    text = "";
                    break;
                case "NothingToDo":
                    text = "";
                    break;
                case "Reassure":
                    text = "";
                    break;
                case "Run":
                    text = "";
                    break;
                case "TellAboutASuspicious":
                    text = "";
                    break;
            }

            resultString = resultString.Insert(resultString.Length, "[" + inActionName + "]: " + text);

            return resultString;
        }
    }
}
