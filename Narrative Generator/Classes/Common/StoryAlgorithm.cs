using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Narrative_Generator
{
    class StoryAlgorithm
    {
        public bool manualInput = true;
        public bool reachedGoalState = false;

        public StoryGraph newStoryGraph = new StoryGraph();
        public WorldBeliefs currentStoryState = new WorldBeliefs();

        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();

        public TwineGraphConstructor twineGraphConstructor = new TwineGraphConstructor();

        public void ReadUserSettingsInput()
        {
            throw new NotImplementedException();
        }

        public void GenerateNewPDDLDomain()
        {
            throw new NotImplementedException();
        }

        public void CreateStartState()
        {
            if (manualInput) // Demo story
            {
                // We create locations, determine their names and the presence of evidence in them.
                LocationStatic kitchenS = new LocationStatic("Kitchen");
                LocationDynamic kitchenD = new LocationDynamic(false);

                LocationStatic diningRoomS = new LocationStatic("Dining room");
                LocationDynamic diningRoomD = new LocationDynamic(false);

                LocationStatic hallS = new LocationStatic("Hall");
                LocationDynamic hallD = new LocationDynamic(false);

                LocationStatic gardenS = new LocationStatic("Garden");
                LocationDynamic gardenD = new LocationDynamic(true);

                LocationStatic bedroomS = new LocationStatic("Bedroom");
                LocationDynamic bedroomD = new LocationDynamic(false);

                LocationStatic guestBedroomS = new LocationStatic("Guest bedroom");
                LocationDynamic guestBedroomD = new LocationDynamic(false);

                LocationStatic bathroomS = new LocationStatic("Bathroom");
                LocationDynamic bathroomD = new LocationDynamic(false);

                LocationStatic atticS = new LocationStatic("Attic");
                LocationDynamic atticD = new LocationDynamic(false);

                Dictionary<LocationStatic, LocationDynamic> locations = new Dictionary<LocationStatic, LocationDynamic>();
                locations.Add(kitchenS, kitchenD);
                locations.Add(diningRoomS, diningRoomD);
                locations.Add(hallS, hallD);
                locations.Add(gardenS, gardenD);
                locations.Add(bedroomS, bedroomD);
                locations.Add(guestBedroomS, guestBedroomD);
                locations.Add(bathroomS, bathroomD);
                locations.Add(atticS, atticD);

                // The first step in creating the initial state is setting up the environment, that is - locations.
                CreateEnviroment(locations);

                List<string> names = new List<string>()
                {
                    "Clerk",
                    "Rich",
                    "Politician",
                    "Mafia-boss",
                    "Journalist",
                    "Judge"
                };

                List<bool> statuses = new List<bool>()
                {
                    true, true, true, true, true, true
                };

                List<AgentRole> roles = new List<AgentRole>()
                {
                    AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.KILLER
                };

                WorldBeliefs standardAgentsGoalState = new WorldBeliefs();
                Goal standardAgentGoal = new Goal(false, true, false, standardAgentsGoalState);

                WorldBeliefs killersGoalState = new WorldBeliefs();
                Goal killerAgentGoal = new Goal(false, true, false, killersGoalState);

                List<Goal> goals = new List<Goal>()
                {
                    standardAgentGoal, standardAgentGoal, standardAgentGoal, standardAgentGoal, standardAgentGoal, killerAgentGoal
                };

                WorldBeliefs usualAgentsBeliefs = new WorldBeliefs();
                WorldBeliefs killerBeliefs = new WorldBeliefs();
                List<WorldBeliefs> beliefs = new List<WorldBeliefs>
                {
                    usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, killerBeliefs
                };

                // The second step in creating the initial state is the creation of agents, initially with empty goals and beliefs, 
                // since they are highly dependent on the agents themselves existing in the "world". We'll finish setting this up in the next step.
                CreateAgents(names, statuses, roles, goals, beliefs, "Hall", 6);

                DistributionOfInitiative();
                currentStoryState.OrderByInitiative();

                // The third step in creating an initial state is assigning to agents their goals and beliefs.
                // Goals
                foreach (var agent in currentStoryState.GetAgents()) // Go through all the agents in the world.
                {
                    if (agent.Value.GetGoal().goalTypeStatus == true) // If the agent's goal is to save/kill someone.
                    {
                        if (agent.Key.GetRole() != AgentRole.KILLER) // Unless the agent has the role of the killer.
                        {
                            agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.KILLER, false); // Then his goal is that 
                                                                                                    //    the killer must be neutralized.
                        }
                        else if (agent.Key.GetRole() == AgentRole.KILLER) // If the agent is the killer.
                        {
                            foreach (var anotherAgent in currentStoryState.GetAgents()) // Then we go through all the agents.
                            {
                                if (anotherAgent.Key.GetRole() != AgentRole.KILLER) // And for everyone who is not a killer...
                                {
                                    agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.USUAL, false); // ...add a new "victim" to the goals.
                                }
                            }
                        }
                    }
                }
                // Beliefs
                foreach (var agent in currentStoryState.GetAgents()) // We go through all the agents in the world.
                {
                    foreach (var anotherAgent in currentStoryState.GetAgents()) // For each separately.
                    {
                        if (agent.Equals(anotherAgent)) // If the agent meets himself.
                        {
                            agent.Value.GetBeliefs().AddAgent(agent.Key, agent.Value); // It simply copies itself into its beliefs.
                        }
                        else
                        {
                            agent.Value.GetBeliefs().AddAgent(AgentRole.USUAL, anotherAgent.Key.GetName()); // Otherwise, copies the name 
                                                                                                            //    of the selected agent and by default 
                                                                                                            //       does not consider him a killer.
                        }
                    }
                }
                // Locations
                foreach (var agent in currentStoryState.GetAgents())
                {
                    agent.Value.GetBeliefs().GetStaticWorldPart().AddLocations(currentStoryState.GetStaticWorldPart().GetLocations());
                    // The dynamic part of the location does not need to be added initially; agents will get an idea of it 
                    //    in the course of direct observation (research).
                }
            }
        }

        public void CreateAgents(List<string> names, List<bool> statuses, List<AgentRole> roles, List<Goal> goals, 
                                 List<WorldBeliefs> beliefs, string spawnLocationName, int numbers)
        {
            // We get info about agents from user input.
            // From it we find out how many agents there are, what roles they have, their beliefs, 
            //    and we will have to design them and add them to the game world.

            for (int i = 0; i < numbers; i++)
            {
                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], spawnLocationName);
            }
        }

        public void CreateAgent(string name, bool status, AgentRole role, Goal goals, WorldBeliefs beliefs, string spawnLocationName)
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic(name, role);
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic(status, goals, beliefs);
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = 
                                                 new KeyValuePair<AgentStateStatic, AgentStateDynamic>(newAgentStateStatic, newAgentStateDynamic);

            // Add the agent to the list of agents.
            currentStoryState.AddAgent(newAgentStateStatic, newAgentStateDynamic);

            // We inform the location that an agent has been added to it.
            currentStoryState.AddAgentIntoLocation(currentStoryState.GetLocationByName(spawnLocationName), newAgent);

            // We inform the agent in which location it was created.
            newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName).Value.AddAgent
                (newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName).Key, newAgent);
        }

        public void CreateEnviroment(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            currentStoryState.AddLocations(locations);

            List<LocationStatic> locationsList = new List<LocationStatic>();
            foreach (var location in locations)
            {
                locationsList.Add(location.Key);
            }

            currentStoryState.GetStaticWorldPart().AddLocations(locationsList);
        }

        public void DistributionOfInitiative()
        {
            List<int> valuesOfInitiative = NumberGenerator(currentStoryState.GetNumberOfAgents());

            foreach(var agent in currentStoryState.GetAgents())
            {
                agent.Value.SetInitiative(RandomSelectionFromTheList(ref valuesOfInitiative));
            }
        }

        public List<int> NumberGenerator(int maximum)
        {
            Random random = new Random();
            List<int> result = new List<int>();

            while (result.Count != maximum)
            {
                int temp = random.Next(1, 100);

                if (result.Contains(temp))
                {
                    continue;
                }
                else
                {
                    result.Add(temp);
                }
            }

            return result;
        }

        public int RandomSelectionFromTheList(ref List<int> list)
        {
            Random random = new Random();
            int index = random.Next(0, list.Count());
            int result = list[index];
            list.RemoveAt(index);
            return result;
        }

        public void CreateConstraints()
        {
            if (manualInput) // Constraints for demo-story
            {
                ConstraintAlive killerMustBeAliveFiveTurns = new ConstraintAlive(true, false, currentStoryState.GetAgentByRole(AgentRole.KILLER), 5);
                storyworldConvergence.AddConstraint(killerMustBeAliveFiveTurns);
            }
        }

        public void Start()
        {
            // ReadUserSettingsInput();
            CreateStartState();
            CreateConstraints();

            // We create a start node (root) based on the start state of the world.
            newStoryGraph.startNode.SetWorldState(currentStoryState);
            newStoryGraph.startNode.SetActivePlayer(false);
            newStoryGraph.startNode.SetActiveAgent(currentStoryState.GetFirstAgent());

            storyworldConvergence.ExtractGoals(currentStoryState);

            // Convergence will calculate FOR ITSELF the entire space of stories, all possible options.
            // !!! This is probably too costly and unnecessary !!!
            //storyworldConvergence.CreateGlobalGraph(newStoryGraph.startNode);

            // The algorithm calculates a SPECIFIC story.
            newStoryGraph = CreateStoryGraph(newStoryGraph.startNode);

            // Create an HTML file including Twine engine and generated history.
            twineGraphConstructor.ConstructGraph(newStoryGraph);
            twineGraphConstructor.CreateHTMLFileWithGame();

            // SaveFile();
        }

        public StoryGraph CreateStoryGraph(StoryNode rootNode)
        {
            newStoryGraph.AddNode(rootNode);

            // We continue to take steps until we reach some goal state.
            while (!reachedGoalState)
            {
                Step(newStoryGraph.GetLastNode());
            }

            return newStoryGraph;
        }

        /// <summary>
        /// Convergence in turn asks agents for actions, checks them, applies them, counteracts them, or does not.
        /// </summary>
        /// <param name="currentNode"></param>
        public void Step(StoryNode currentNode)
        {
            newStoryGraph.GetLastNode().GetWorldState().GetStaticWorldPart().IncreaseTurnNumber();

            foreach (var agent in currentStoryState.GetAgents())
            {
                if (agent.Value.GetStatus())
                {
                    // Convergence assigns who is on the turn to the node and then applies the changes to the state of the world.
                    storyworldConvergence.ActionRequest(agent, ref newStoryGraph, ref currentStoryState);
                    reachedGoalState = storyworldConvergence.ControlToAchieveGoalState(currentStoryState);
                }
            }
        }
    }
}