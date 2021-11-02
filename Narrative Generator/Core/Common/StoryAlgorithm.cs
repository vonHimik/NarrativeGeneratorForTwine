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
        public Graphviz graphviz = new Graphviz();

        // Internal components
        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();
        public GraphСonstructor graphСonstructor = new GraphСonstructor();
        public TwineGraphConstructor twineGraphConstructor = new TwineGraphConstructor();

        // Start --> current state
        public WorldDynamic currentStoryState = new WorldDynamic();
        public int goalsCounter = 1;
        public int agentsCounter = 3; // 7
        public int locationsCounter = 8; // 8

        // Output graphs
        public StoryGraph newStoryGraph = new StoryGraph();

        // TODO
        public void ReadUserSettingsInput()
        {
            throw new NotImplementedException();
        }

        // TODO
        public void GenerateNewPDDLDomains()
        {
            //CreateAgentPDDLDomain();
            CreateKillerPDDLDomain();
        }

        // TODO
        public void CreateAgentPDDLDomain()
        {
            throw new NotImplementedException();
        }

        public void CreateKillerPDDLDomain()
        {
            string fileName = "";
            string domainName = "";
            string predicates = "";
            string actions = "";

            int numberOfKillers = 1;
            int numberOfVictims = 1;

            fileName = "KillerDomainTEST";

            domainName = "detective-domain";

            predicates = "(ROOM ?x) (AGENT ?x) (KILLER ?x) (alive ?x) (died ?x) (in-room ?x ?y) (connected ?x ?y)";

            // Action - Kill
            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Kill" + Environment.NewLine + ":parameters (?k ?victim ?r");
            for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
            {
                actions = actions.Insert(actions.Length, " ?a" + i);
            }
            actions = actions.Insert(actions.Length, ")" + Environment.NewLine);
            actions = actions.Insert(actions.Length, ":precondition (and (ROOM ?r) (KILLER ?k) (AGENT ?victim) ");
            for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
            {
                actions = actions.Insert(actions.Length, "(AGENT ?a" + i + ") ");
            }
            actions = actions.Insert(actions.Length, Environment.NewLine);
            actions = actions.Insert(actions.Length, " (alive ?k) (alive ?victim) " + Environment.NewLine);
            actions = actions.Insert(actions.Length, "(in-room ?k ?r) (in-room ?victim ?r)" + Environment.NewLine);
            for (int i = 1; i <= agentsCounter - numberOfKillers - numberOfVictims; i++)
            {
                actions = actions.Insert(actions.Length, " (or (and (alive ?a" + i + ")" + "(not (in-room ?a" + i + " ?r)))  (died ?a" + i + "))");
            }
            actions = actions.Insert(actions.Length, Environment.NewLine + ")" + Environment.NewLine);
            actions = actions.Insert(actions.Length, ":effect (and (died ?victim) (not(alive ?victim))))" + Environment.NewLine);

            // Action - Move
            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action killer_move" + Environment.NewLine 
                + " :parameters (?k ?room-from ?room-to)" + Environment.NewLine 
                + " :precondition (and (ROOM ?room-from) (ROOM ?room-to) (KILLER ?k) (alive ?k)" + Environment.NewLine 
                + " (in-room ?k ?room-from) (not(died ?k)) (not (in-room ?k ?room-to)) (connected ?room-from ?room-to))" + Environment.NewLine 
                + " :effect (and (in-room ?k ?room-to) (not(in-room ?k ?room-from))))" + Environment.NewLine);

            // Action - Entrap
            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action Entrap" + Environment.NewLine 
               + " :parameters(?k ?a ?place)" + Environment.NewLine
               + " :precondition (and (ROOM ?place) (KILLER ?k) (AGENT ?a) (alive ?k) (alive ?a)" + Environment.NewLine
               + " (in-room ?k ?place) (not (in-room ?a ?place)))" + Environment.NewLine
               + " :effect (and (in-room ?a ?place)))" + Environment.NewLine);

            // Action - Tell about a suspicious
            actions = actions.Insert(actions.Length, Environment.NewLine + "(:action TellAboutASuspicious" + Environment.NewLine 
                + " :parameters (?k ?a ?place ?suspicious-place)" + Environment.NewLine 
                + " :precondition (and (ROOM ?place) (ROOM ?suspicious-place) (KILLER ?k)(AGENT ?a) (alive ?k) (alive ?a) " + Environment.NewLine
                + "(in-room ?k ?place) (in-room ?a ?place) (not (= ?place ?suspicious-place)))" + Environment.NewLine 
                + " :effect (and (in-room ?a ?suspicious-place)))" + Environment.NewLine);

           FileStream file = new FileStream(fileName + ".pddl", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            streamWriter.WriteLine("(define (domain " + domainName + ")");
            streamWriter.WriteLine("(:predicates " + predicates + ")");
            streamWriter.WriteLine(actions);
            streamWriter.WriteLine(")");

            streamWriter.Close();
        }

        public void CreateStartState()
        {
            if (manualInput) // Demo story
            {

                // === LOCATIONS CREATING ===

                // We create locations, determine their names and the presence of evidence in them.
                List<string> locationNames = CreateLocationsNamesList(locationsCounter);
                List<bool> locationsEvidences = CreateLocationsEvidencesList(locationsCounter);
                Dictionary<LocationStatic, LocationDynamic> locations = CreateLocationSet(locationNames, locationsEvidences);

                // The first step in creating the initial state is setting up the environment, that is - locations.
                CreateEnviroment(locations);

                // === AGENTS CREATING ===

                List<string> names = CreateAgentsNamesList(agentsCounter);
                List<bool> statuses = CreateStatusesList(agentsCounter);
                List<AgentRole> roles = CreateAgentsRolesList(agentsCounter);
                List<Goal> goals = CreateGoalSet(roles);
                List<WorldContext> beliefs = CreateBeliefsSet(agentsCounter);

                // The second step in creating the initial state is the creation of agents, initially with empty goals and beliefs, 
                //    since they are highly dependent on the agents themselves existing in the "world". We'll finish setting this up in the next step.
                CreateAgents(names, statuses, roles, goals, beliefs, GetRandomLocationName(locationNames), agentsCounter);

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
                            agent.Value.GetBeliefs().AddAgentInBeliefs(anotherAgent, AgentRole.USUAL);
                        }
                    }
                }
            }
        }

        public List<string> CreateLocationsNamesList(int locationsCounter)
        {
            List<string> namesList = new List<string>();

            List<string> defaultNamesList = new List<string>
            {
                "kitchen", "dining-room", "hall", "garden", "bedroom", "guest-bedroom", "bathroom", "attic"
            };

            for (int i = 0; i < locationsCounter; i++)
            {
                namesList.Add(defaultNamesList[i]);
            }

            return namesList;
        }

        public List<bool> CreateLocationsEvidencesList(int evidenceCount)
        {
            List<bool> locationsEvidences = new List<bool>();

            for(int i = 0; i < evidenceCount; i++)
            {
                locationsEvidences.Add(false);
            }

            return locationsEvidences;
        }

        public List<bool> CreateStatusesList(int agentsCounter)
        {
            List<bool> statuses = new List<bool>();

            for (int i = 0; i < agentsCounter; i++)
            {
                statuses.Add(true);
            }

            return statuses;
        }

        public List<string> CreateAgentsNamesList(int agentsCounter)
        {
            List<string> namesList = new List<string>();

            List<string> defaultNamesList = new List<string>()
            {               
                "Player",
                "Judge",
                "Journalist",
                "Mafia-boss",
                "Politician",
                "Rich",
                "Clerk"
            };

            for (int i = 0; i < agentsCounter; i++)
            {
                namesList.Add(defaultNamesList[i]);
            }

            return namesList;
        }

        public string GetRandomLocationName(List<string> namesList)
        {
            Random random = new Random();
            int rand = random.Next(namesList.Count);
            return namesList[rand];
        }

        public List<AgentRole> CreateAgentsRolesList(int agentsCounter)
        {
            List<AgentRole> rolesList = new List<AgentRole>();

            List<AgentRole> defaultRolesList = new List<AgentRole>()
            {                  
                AgentRole.PLAYER,
                AgentRole.KILLER,
                AgentRole.USUAL,
                AgentRole.USUAL,
                AgentRole.USUAL,
                AgentRole.USUAL,
                AgentRole.USUAL,
            };

            for (int i = 0; i < agentsCounter; i++)
            {
                rolesList.Add(defaultRolesList[i]);
            }

            return rolesList;
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
            currentStoryState.AddAgent(newAgent, currentStoryState.GetLocationByName(spawnLocationName));

            // We transfer information about the locations in the world to the agent.
            newAgent.Value.GetBeliefs().SetLocationsInWorld(locations);

            // We clear information about the contents of locations.
            //newAgent.Value.GetBeliefs().ClearLocations();

            // We inform the location that an agent has been added to it.
            //currentStoryState.AddAgentIntoLocation(currentStoryState.GetLocationByName(spawnLocationName), newAgent);

            // We inform the agent in which location it was created.
            newAgent.Value.GetBeliefs().SetMyLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
            newAgent.Value.GetBeliefs().AddAgentInBeliefs(newAgent, newAgent.Key.GetRole());
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

                if (agent.Key.GetRole() == AgentRole.PLAYER) { agent.Value.SetInitiative(100); }
            }
        }

        public List<int> NumberGenerator(int maximum)
        {
            Random random = new Random();
            List<int> result = new List<int>();

            while (result.Count != maximum)
            {
                int temp = random.Next(0, 99);

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

            OrderLocationsRandom(ref locations);

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

        public void OrderLocationsRandom(ref Dictionary<LocationStatic, LocationDynamic> locations)
        {
            locations = locations.OrderBy(x => x.Value.GetHashCode()).ToDictionary(x => x.Key, x => x.Value);
        }

        public bool pathExistenceControlling(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool result = false;

            Queue<LocationStatic> queue = new Queue<LocationStatic>();
            HashSet<LocationStatic> visitedLocations = new HashSet<LocationStatic>();

            LocationStatic root = locations.First().Key;

            queue.Enqueue(root);
            visitedLocations.Add(root);

            while (queue.Count > 0)
            {
                LocationStatic currentLocation = queue.Dequeue();

                foreach (LocationStatic nextLocation in currentLocation.GetConnectedLocations())
                {
                    if (visitedLocations.Contains(nextLocation)) continue;

                    queue.Enqueue(nextLocation);
                    visitedLocations.Add(nextLocation);
                }
            }

            if (visitedLocations.Count == locations.Count) { result = true; }

            return result;
        }

        /// <summary>
        /// A method that randomly assigns connections between locations.
        /// </summary>
        public Dictionary<LocationStatic, LocationDynamic> LocationsConnection(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool pathExists = false;

            while (!pathExists)
            {
                foreach (var loc in locations)
                {
                    loc.Key.ClearAllConnections();
                }

                foreach (var location in locations)
                {
                    Random random = new Random();

                    while (location.Key.GetConnectedLocations().Count == 0)
                    {
                        int rand = random.Next(locations.Count);

                        if (!location.Equals(locations.ElementAt(rand)) && !location.Key.ConnectionChecking(locations.ElementAt(rand).Key)
                               && location.Key.GetConnectedLocations().Count < 3 && locations.ElementAt(rand).Key.GetConnectedLocations().Count < 3)
                        {
                            location.Key.AddConnection(locations.ElementAt(rand).Key);
                            locations.ElementAt(rand).Key.AddConnection(location.Key);
                        }
                    }
                }

                pathExists = pathExistenceControlling(locations);
            }

            // Returns locations with established connections between them.
            return locations;
        }

        public void Start()
        {
            // ReadUserSettingsInput();
            CreateStartState();
            CreateConstraints();
            GenerateNewPDDLDomains();

            // We create a start node (root) based on the start state of the world.
            StoryNode root = new StoryNode();
            root.SetWorldState(currentStoryState);
            root.SetActivePlayer(false);
            root.SetActiveAgent(currentStoryState.GetFirstAgent());
            newStoryGraph.SetRoot(root);

            // We go through all the agents and remember their goals.
            storyworldConvergence.ExtractGoals(currentStoryState);

            // The algorithm calculates a SPECIFIC story.
            newStoryGraph = CreateStoryGraph(newStoryGraph.GetRoot());

            // Create a visual graph.
            graphСonstructor.CreateGraph(newStoryGraph, "newStoryGraph");

            // Create an HTML file including Twine engine and generated history.
            //twineGraphConstructor.ConstructGraph(newStoryGraph);
            //twineGraphConstructor.CreateHTMLFileWithGame();

            // SaveFile();
        }

        public void BFSTraversing(StoryNode rootNode, bool root = true)
        {
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();
            bool skip = false;
            int doomCounter = 0;

            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);
            int actualAgentNumber = 0;
            int globalNodeNumber = -1;

            while (queue.Count > 0 /*&& newStoryGraph.GetNodes().Count < 500*/ && doomCounter < 10)
            {
                skip = false;

                StoryNode currentNode = queue.Dequeue();

                // If we come across a node with a target state, then we do not expand it.
                if (storyworldConvergence.ControlToAchieveGoalState(currentNode.GetWorldState()))
                {
                    continue;
                }
                else
                {
                    if (currentNode.Equals(rootNode) && root)
                    {
                        Step(newStoryGraph.GetRoot(), actualAgentNumber, root, ref globalNodeNumber, ref skip, ref queue);
                        queue.Enqueue(currentNode);
                        visitedNodes.Add(currentNode);
                        root = false;
                    }
                    else
                    {
                        actualAgentNumber = GetActualAgentNumber(currentNode.GetWorldState().GetIndexOfAgent(currentNode.GetActiveAgent()));
                        Step(newStoryGraph.GetNode(currentNode), actualAgentNumber, root, ref globalNodeNumber, ref skip, ref queue);
                    }
                }
                
                if (skip)
                {
                    actualAgentNumber = GetActualAgentNumber(currentNode.GetWorldState().GetIndexOfAgent(currentNode.GetActiveAgent()));
                    currentNode.SetActiveAgent(currentNode.GetWorldState().GetAgentByIndex(actualAgentNumber));
                    queue.Enqueue(currentNode);
                    if (!visitedNodes.Contains(currentNode)) { visitedNodes.Add(currentNode); }
                    doomCounter++;
                    continue;
                }

                doomCounter = 0;

                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    if (visitedNodes.Contains(nextNode)) continue;

                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }

                currentNode = null;
            }
        }

        public bool BFSGoalAchieveControl(StoryNode rootNode)
        {
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);

            while (queue.Count > 0)
            {
                StoryNode currentNode = queue.Dequeue();

                reachedGoalState = storyworldConvergence.ControlToAchieveGoalState(currentNode.GetWorldState());
                if (reachedGoalState) { return true; }

                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    if (visitedNodes.Contains(nextNode)) continue;

                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }
            }

            return false;
        }

        public StoryGraph CreateStoryGraph(StoryNode rootNode)
        {
            StoryNode originRoot = (StoryNode)rootNode.Clone();

            while (!reachedGoalState /*&& newStoryGraph.GetNodes().Count < 500*/)
            {
                BFSTraversing(newStoryGraph.GetRoot());
                BFSGoalAchieveControl(newStoryGraph.GetRoot());
                if (!reachedGoalState)
                {
                    newStoryGraph = new StoryGraph();
                    originRoot.GetEdges().Clear();
                    originRoot.GetLinks().Clear();
                    newStoryGraph.SetRoot((StoryNode)originRoot.Clone());
                }
            }

            return newStoryGraph;
        }

        /// <summary>
        /// Convergence in turn asks agents for actions, checks them, applies them, counteracts them, or does not.
        /// </summary>
        public void Step (StoryNode currentNode, int agentIndex, bool root, ref int globalNodeNumber, ref bool skip, ref Queue<StoryNode> queue)
        {
            // Convergence assigns who is on the turn to the node and then applies the changes to the state of the world.
            currentStoryState = currentNode.GetWorldState();
            currentStoryState.GetStaticWorldPart().IncreaseTurnNumber();

            if (currentStoryState.GetAgentByIndex(agentIndex).Value.GetStatus())
            {
                storyworldConvergence.ActionRequest(currentStoryState.GetAgentByIndex(agentIndex), ref newStoryGraph, ref currentStoryState, 
                                                     currentNode, root, ref globalNodeNumber, ref skip, ref queue);
            }
        }

        public int GetActualAgentNumber(int prevNumber)
        {
            bool aliveControl = false;
            int maxNumber = currentStoryState.GetNumberOfAgents();
            int result = 0;

            while (!aliveControl)
            {
                if (prevNumber == maxNumber - 1)
                {
                    result = 0;
                    prevNumber = 0;
                }
                else
                {
                    prevNumber++;
                    result = prevNumber;
                }

                if (!currentStoryState.GetAgentByIndex(result).Value.GetStatus()) { continue; }

                aliveControl = true;
            }

            return result;
        }
    }
}