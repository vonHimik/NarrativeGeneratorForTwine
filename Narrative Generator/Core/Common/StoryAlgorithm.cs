using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Rivers;

namespace Narrative_Generator
{
    class StoryAlgorithm
    {
        // Settings
        public bool manualInput = true;
        public bool reachedGoalState = false;

        // API components
        public RiversWrapper rivers = new RiversWrapper();
        public Graphviz graphviz = new Graphviz();

        // Internal components
        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();
        public TwineGraphConstructor twineGraphConstructor = new TwineGraphConstructor();

        // State
        public WorldDynamic currentStoryState = new WorldDynamic();
        public int goalsCounter = 1;

        // Output graphs
        public StoryGraph newStoryGraph = new StoryGraph();
        public Graph graph = new Graph();

        // TODO
        public void ReadUserSettingsInput()
        {
            throw new NotImplementedException();
        }

        // TODO
        public void GenerateNewPDDLDomain()
        {
            throw new NotImplementedException();
        }

        public void CreateStartState()
        {
            if (manualInput) // Demo story
            {

                // We create locations, determine their names and the presence of evidence in them.
                List<string> locationNames = new List<string>
                {
                    "kitchen", "dining-room", "hall", "garden", "bedroom", "guest-bedroom", "bathroom", "attic"
                };

                List<bool> locationsEvidences = new List<bool>
                {
                    false, false, false, true, false, false, false, false
                };

                Dictionary<LocationStatic, LocationDynamic> locations = CreateLocationSet(locationNames, locationsEvidences);

                // The first step in creating the initial state is setting up the environment, that is - locations.
                CreateEnviroment(locations);

                List<string> names = new List<string>()
                {
                    "Clerk",
                    "Rich",
                    "Politician",
                    "Mafia-boss",
                    "Journalist",
                    "Judge",
                    "Player"
                };

                List<bool> statuses = new List<bool>()
                {
                    true, true, true, true, true, true, true
                };

                List<AgentRole> roles = new List<AgentRole>()
                {
                    AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.KILLER, AgentRole.PLAYER
                };

                List<Goal> goals = CreateGoalSet(roles);

                List<WorldContext> beliefs = CreateBeliefsSet(7);

                // The second step in creating the initial state is the creation of agents, initially with empty goals and beliefs, 
                //    since they are highly dependent on the agents themselves existing in the "world". We'll finish setting this up in the next step.
                CreateAgents(names, statuses, roles, goals, beliefs, "hall", 7);

                // We randomly assign an initiative value to the agents to determine the order of their turn, and sort the agents on the initiative.
                DistributionOfInitiative();
                currentStoryState.OrderAgentsByInitiative();

                // The third step in creating an initial state is assigning to agents their goals and beliefs.

                // === Goals === //

                // Go through all the agents in the world.
                foreach (var agent in currentStoryState.GetAgents())
                {
                    // If the agent's goal is to save/kill someone.
                    if (agent.Value.GetGoal().goalTypeIsStatus == true)
                    {
                        // Unless the agent has the role of the killer.
                        if (agent.Key.GetRole() != AgentRole.KILLER)
                        {
                            // Then his goal is that the killer must be neutralized.
                            agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.KILLER, false);
                        }
                        // If the agent is the killer.
                        else if (agent.Key.GetRole() == AgentRole.KILLER)
                        {
                            int agentCounter = 0;
                            int playerCounter = 1;
                            int killerCounter = NumberOfKillers();

                            // Then we go through all the agents.
                            foreach (var anotherAgent in currentStoryState.GetAgents())
                            {
                                // And for everyone who is not a killer...
                                if (anotherAgent.Key.GetRole() != AgentRole.KILLER)
                                {
                                    agentCounter++;

                                    // ...add a new "victim" to the goals.
                                    if (agentCounter == (currentStoryState.GetAgents().Count() - playerCounter - killerCounter))
                                    {
                                        agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.PLAYER, false);
                                    }
                                    else { agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.USUAL, false); }
                                }
                            }
                        }
                    }
                }

                // === Beliefs === //

                // We go through all the agents in the world.
                foreach (var agent in currentStoryState.GetAgents())
                {
                    // For each separately.
                    foreach (var anotherAgent in currentStoryState.GetAgents())
                    {
                        // If the agent meets himself.
                        if (agent.Equals(anotherAgent))
                        {
                            continue;
                        }
                        else
                        {
                            // Otherwise, copies the name of the selected agent and by default does not consider him a killer.
                            agent.Value.GetBeliefs().AddAgentInWorld(anotherAgent, AgentRole.USUAL);
                        }
                    }
                }
            }
        }

        public int NumberOfKillers()
        {
            int counter = 0;

            foreach (var agent in currentStoryState.GetAgents())
            {
                if (agent.Key.GetRole() == AgentRole.KILLER)
                {
                    counter++;
                }
            }

            return counter;
        }

        /// <summary>
        /// We get info about agents from user input. From it we find out how many agents there are, what roles they have, their beliefs, 
        /// and we will have to design them and add them to the game world.
        /// </summary>
        /// <param name="names"></param>
        /// <param name="statuses"></param>
        /// <param name="roles"></param>
        /// <param name="goals"></param>
        /// <param name="beliefs"></param>
        /// <param name="spawnLocationName"></param>
        /// <param name="numbers"></param>
        public void CreateAgents(List<string> names, List<bool> statuses, List<AgentRole> roles, List<Goal> goals, 
                                 List<WorldContext> beliefs, string spawnLocationName, int numbers)
        {
            for (int i = 0; i < numbers; i++)
            {
                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i], spawnLocationName);
            }
        }

        /// <summary>
        /// This method creates a separate agent using the information passed to it. 
        /// Then it places the agent on the environment and passes information about it to it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="role"></param>
        /// <param name="goals"></param>
        /// <param name="beliefs"></param>
        /// <param name="spawnLocationName"></param>
        public void CreateAgent(string name, bool status, AgentRole role, Goal goals, WorldContext beliefs, string spawnLocationName)
        {
            // We clone locations from the world.
            Dictionary<LocationStatic, LocationDynamic> locations = currentStoryState.CloneLocations();

            // We construct a new agent, from static and dynamic parts.
            AgentStateStatic newAgentStateStatic = new AgentStateStatic(name, role);
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic(status, goals, beliefs, newAgentStateStatic);
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = 
               new KeyValuePair<AgentStateStatic, AgentStateDynamic>(newAgentStateStatic, newAgentStateDynamic);

            // Add the agent to the list of agents.
            currentStoryState.AddAgent(newAgentStateStatic, newAgentStateDynamic);

            // We transfer information about the locations in the world to the agent.
            newAgent.Value.GetBeliefs().SetLocationsInWorld(locations);

            // We clear information about the contents of locations.
            //newAgent.Value.GetBeliefs().ClearLocations();

            // We inform the location that an agent has been added to it.
            currentStoryState.AddAgentIntoLocation(currentStoryState.GetLocationByName(spawnLocationName), newAgent);


            // We inform the agent in which location it was created.
            newAgent.Value.GetBeliefs().SetMyLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
            newAgent.Value.GetBeliefs().AddAgentInWorld(newAgent, newAgent.Key.GetRole());
            newAgent.Value.GetBeliefs().GetAgentByName(newAgent.Key.GetName()).
                SetLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
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
                int temp = random.Next(1, 101);

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

        public Dictionary<LocationStatic, LocationDynamic> CreateLocationSet(List<string> locationNames, List<bool> locationsEvidences)
        {
            Dictionary<LocationStatic, LocationDynamic> locations = new Dictionary<LocationStatic, LocationDynamic>();

            for (int i = 0; i < locationNames.Count; i++)
            {
                LocationStatic newLocationStatic = new LocationStatic(locationNames[i]);
                LocationDynamic newLocationDynamic = new LocationDynamic(locationsEvidences[i], newLocationStatic);
                locations.Add(newLocationStatic, newLocationDynamic);
            }

            locations = LocationsConnection(locations);

            return locations;
        }

        public List<Goal> CreateGoalSet(List<AgentRole> roles)
        {
            List<Goal> goals = new List<Goal>();

            foreach (var role in roles)
            {
                WorldDynamic newGoalState = new WorldDynamic();

                switch (role)
                {
                    case AgentRole.USUAL:
                        Goal standardAgentGoal = new Goal(false, true, false, newGoalState);
                        goals.Add(standardAgentGoal);
                        break;
                    case AgentRole.KILLER:
                        Goal killerAgentGoal = new Goal(false, true, false, newGoalState);
                        goals.Add(killerAgentGoal);
                        break;
                    case AgentRole.PLAYER:
                        Goal playerGoal = new Goal(false, true, false, newGoalState);
                        goals.Add(playerGoal);
                        break;
                }
            }

            return goals;
        }

        public List<WorldContext> CreateBeliefsSet(int count)
        {
            List<WorldContext> newWorldBeliefsList = new List<WorldContext>();

            for (int i = 0; i < count; i++)
            {
                WorldContext newWorldBeliefs = new WorldContext();
                newWorldBeliefsList.Add(newWorldBeliefs);
            }

            return newWorldBeliefsList;
        }

        public Dictionary<LocationStatic, LocationDynamic> LocationsConnection(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            Random rand = new Random();

            foreach (var location in locations)
            {
                int locCounter = rand.Next(1, 3);

                foreach (var additionalLoc in locations)
                {
                    int add = rand.Next(0, 2);

                    if (locCounter > 0 && add == 0 && !location.Key.GetName().Equals(additionalLoc.Key.GetName()))
                    {
                        if (location.Key.ConnectionChecking(additionalLoc.Key))
                        {
                            locCounter--;
                        }
                        else
                        {
                            location.Key.AddConnection(additionalLoc.Key);
                            additionalLoc.Key.AddConnection(location.Key);
                            locCounter--;
                        }
                    }
                }
            }

            return locations;
        }

        public void Start()
        {
            // ReadUserSettingsInput();
            CreateStartState();
            CreateConstraints();

            // We create a start node (root) based on the start state of the world.
            newStoryGraph.GetRoot().SetWorldState(currentStoryState);
            newStoryGraph.GetRoot().SetActivePlayer(false);
            newStoryGraph.GetRoot().SetActiveAgent(currentStoryState.GetFirstAgent());

            // We go through all the agents and remember their goals.
            storyworldConvergence.ExtractGoals(currentStoryState);

            // The algorithm calculates a SPECIFIC story.
            newStoryGraph = CreateStoryGraph(newStoryGraph.GetRoot());

            // Create a visual graph.
            graph = rivers.CreateRiversGraph(newStoryGraph, "testgraph");
            rivers.CreateDotFile(graph);
            graphviz.Run("testgraphimage", "testgraph");

            // Create an HTML file including Twine engine and generated history.
            twineGraphConstructor.ConstructGraph(newStoryGraph);
            twineGraphConstructor.CreateHTMLFileWithGame();

            // SaveFile();
        }

        public StoryGraph CreateStoryGraph(StoryNode rootNode)
        {
            newStoryGraph.AddNode(rootNode);

            int nodesCounter = 0;

            // We continue to take steps until we reach some goal state.
            while (/*!reachedGoalState*/ goalsCounter > 0)
            {
                Step(newStoryGraph.GetNode(nodesCounter), ref nodesCounter);
            }

            return newStoryGraph;
        }

        /// <summary>
        /// Convergence in turn asks agents for actions, checks them, applies them, counteracts them, or does not.
        /// </summary>
        /// <param name="currentNode"></param>
        public void Step(StoryNode currentNode, ref int currentNodeNumber)
        {
            currentNode.GetWorldState().GetStaticWorldPart().IncreaseTurnNumber();

            currentStoryState = currentNode.GetWorldState();

            foreach (var agent in currentStoryState.GetAgents())
            {
                if (agent.Value.GetStatus())
                {
                    // Convergence assigns who is on the turn to the node and then applies the changes to the state of the world.
                    storyworldConvergence.ActionRequest(agent, ref newStoryGraph, ref currentStoryState, ref currentNodeNumber, ref goalsCounter);
                    reachedGoalState = storyworldConvergence.ControlToAchieveGoalState(currentStoryState);

                    if (!reachedGoalState) { currentNodeNumber++; }
                    if (reachedGoalState) { goalsCounter--; }
                }
            }
        }
    }
}