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

        /// <summary>
        /// A method that creates an initial state of the world based on user preferences.
        /// </summary>
        public void CreateInitialState()
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

                // We create sets of attributes for agents: names, states, roles, goals, and beliefs.
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

        /// <summary>
        /// A method that creates a list of names for locations.
        /// </summary>
        public List<string> CreateLocationsNamesList(int locationsCounter)
        {
            // We create an empty list.
            List<string> namesList = new List<string>();

            // We prepare a list with manually specified names.
            List<string> defaultNamesList = new List<string>
            {
                "kitchen", "dining-room", "hall", "garden", "bedroom", "guest-bedroom", "bathroom", "attic"
            };

            // We go through the empty list, assigning an entry from the list with prepared names to the corresponding cell.
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < locationsCounter; i++)
            {
                namesList.Add(defaultNamesList[i]);
            }

            // We return a list with the names of locations.
            return namesList;
        }

        /// <summary>
        /// A method that determines in which of the locations the evidence will be located.
        /// </summary>
        public List<bool> CreateLocationsEvidencesList(int evidenceCount)
        {
            // We create a list that stores information about the presence of evidence in locations.
            List<bool> locationsEvidences = new List<bool>();

            // We go through the list, adding information about the presence of evidence. At the moment - there is no evidence !!!
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < evidenceCount; i++)
            {
                locationsEvidences.Add(false);
            }

            // We return a list of evidence in locations.
            return locationsEvidences;
        }

        // A method that creates a list defining the states of agents.
        public List<bool> CreateStatusesList(int agentsCounter)
        {
            // We create an empty list.
            List<bool> statuses = new List<bool>();

            // We go through the list, assigning information about the state of agents. At the moment - everyone is alive !!!
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                statuses.Add(true);
            }

            // Returning a list with states.
            return statuses;
        }

        // A method that creates a list of names for agents.
        public List<string> CreateAgentsNamesList(int agentsCounter)
        {
            // We create an empty list.
            List<string> namesList = new List<string>();

            // Create a list with manually entered names.
            List<string> defaultNamesList = new List<string>()
            {               
                "Player", "Judge", "Journalist", "Mafia-boss", "Politician", "Rich", "Clerk"
            };

            // We go through the empty list, adding names from the list with names to the corresponding cells.
            // Until we reach the specified cell (there may be more names than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                namesList.Add(defaultNamesList[i]);
            }

            // We return a list with the names of the agents.
            return namesList;
        }

        /// <summary>
        /// A method that returns a randomly selected location name from a list of locations.
        /// </summary>
        public string GetRandomLocationName(List<string> namesList)
        {
            // We get a random number, from 0 to the number of elements in the list.
            Random random = new Random();
            int rand = random.Next(namesList.Count);

            // The resulting random number is used as the index of the element in the list of location names, which we will return.
            return namesList[rand];
        }

        /// <summary>
        /// A method that creates a list of roles for agents.
        /// </summary>
        public List<AgentRole> CreateAgentsRolesList(int agentsCounter)
        {
            // We create an empty list.
            List<AgentRole> rolesList = new List<AgentRole>();

            // Create a list with manually assigned roles.
            List<AgentRole> defaultRolesList = new List<AgentRole>()
            {                  
                AgentRole.PLAYER, AgentRole.KILLER, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL, AgentRole.USUAL
            };

            // We go through the empty list, assigning values from the list with roles to the corresponding cells.
            // Until we reach the specified cell (there may be more roles than necessary).
            for (int i = 0; i < agentsCounter; i++)
            {
                rolesList.Add(defaultRolesList[i]);
            }

            // Returning a list with agent roles.
            return rolesList;
        }

        /// <summary>
        /// A method that returns the number of agents with the "Killer" role.
        /// </summary>
        public int NumberOfKillers()
        {
            int counter = 0;

            // We go through the list of agents and increase the counter every time we meet an agent with the role of "Killer".
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

            // We inform the agent in which location it was created.
            newAgent.Value.GetBeliefs().SetMyLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
            newAgent.Value.GetBeliefs().AddAgentInBeliefs(newAgent, newAgent.Key.GetRole());
            newAgent.Value.GetBeliefs().GetAgentByName(newAgent.Key.GetName()).
                SetLocation(newAgent.Value.GetBeliefs().GetLocationByName(spawnLocationName));
        }

        /// <summary>
        /// A method that adds locations to the current state of the world.
        /// </summary>
        public void CreateEnviroment(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            // Add locations to the dynamic component of the current state of the world.
            currentStoryState.AddLocations(locations);

            // Create an empty list of static components of locations.
            List<LocationStatic> locationsList = new List<LocationStatic>();

            // We go through the list of locations, adding their static components to an empty list.
            foreach (var location in locations)
            {
                locationsList.Add(location.Key);
            }

            // Assign the list of static components of locations to the static component to the current state of the world.
            currentStoryState.GetStaticWorldPart().AddLocations(locationsList);
        }

        /// <summary>
        /// A method that determines of agents' initiative. The higher the score, the lower the agent will be in the list.
        /// </summary>
        public void DistributionOfInitiative()
        {
            // Create a list of random values (in the range from 0 to 99), equal to the number of agents.
            List<int> valuesOfInitiative = NumberGenerator(currentStoryState.GetNumberOfAgents());

            // We go through the list of agents in the current state.
            foreach (var agent in currentStoryState.GetAgents())
            {
                // Assign a random number to the agent from the list.
                agent.Value.SetInitiative(RandomSelectionFromTheList(ref valuesOfInitiative));

                // If the agent has the role of a player, then it is guaranteed to have an initiative of 100.
                if (agent.Key.GetRole() == AgentRole.PLAYER) { agent.Value.SetInitiative(100); }
            }
        }

        /// <summary>
        ///  A method that implements a random number generator, in the range 0 through 99, 
        ///     that returns a set of numbers no larger than the specified size.
        /// </summary>
        public List<int> NumberGenerator(int maximum)
        {
            // We will instantiate the random number generator.
            Random random = new Random();

            // Create an empty list of numbers.
            List<int> result = new List<int>();

            // We start a loop that will continue until it returns the specified number of values to us.
            while (result.Count != maximum)
            {
                // We get a random number in the range from 0 to 99.
                int temp = random.Next(0, 99);

                // If it is already in the list, then continue without doing anything.
                if (result.Contains(temp))
                {
                    continue;
                }
                // Otherwise, add it to the list.
                else
                {
                    result.Add(temp);
                }
            }

            // We return a list with a set of random numbers.
            return result;
        }

        /// <summary>
        /// A method that returns a randomly selected number from a specified list of numbers and removes it from the list.
        /// </summary>
        public int RandomSelectionFromTheList(ref List<int> list)
        {
            // We will instantiate the random number generator.
            Random random = new Random();

            // We get the index using a random number generator, in the range from 0 to the number of elements in the list.
            int index = random.Next(0, list.Count());

            // We read and store from the list the value of the cell with the previously obtained index.
            int result = list[index];

            // We remove this cell from the list.
            list.RemoveAt(index);

            return result;
        }

        /// <summary>
        /// A method that defines the constraints imposed on the story.
        /// </summary>
        public void CreateConstraints()
        {
            if (manualInput) // Constraints for demo-story
            {
                // The killer must be alive for the first 5 turns.
                ConstraintAlive killerMustBeAliveFiveTurns = new ConstraintAlive(true, false, currentStoryState.GetAgentByRole(AgentRole.KILLER), 5);
                storyworldConvergence.AddConstraint(killerMustBeAliveFiveTurns);
            }
        }

        /// <summary>
        /// A method that creates a set of ready-made locations.
        /// </summary>
        public Dictionary<LocationStatic, LocationDynamic> CreateLocationSet(List<string> locationNames, List<bool> locationsEvidences)
        {
            // Create an empty set of locations.
            Dictionary<LocationStatic, LocationDynamic> locations = new Dictionary<LocationStatic, LocationDynamic>();

            // We create a location the required number of times, using the constructors of its static and dynamic parts, and then add it to the set.
            for (int i = 0; i < locationNames.Count; i++)
            {
                LocationStatic newLocationStatic = new LocationStatic(locationNames[i]);
                LocationDynamic newLocationDynamic = new LocationDynamic(locationsEvidences[i], newLocationStatic);
                locations.Add(newLocationStatic, newLocationDynamic);
            }

            // Shuffle the locations in the set in random order.
            OrderLocationsRandom(ref locations);

            // We establish connections between locations.
            locations = LocationsConnection(locations);

            // We return the ready-made set of locations.
            return locations;
        }

        /// <summary>
        /// A method that creates a set of goals to pass to agents.
        /// </summary>
        public List<Goal> CreateGoalSet(List<AgentRole> roles)
        {
            // We create an empty list of goals.
            List<Goal> goals = new List<Goal>();

            // For each role, we define own goal type and add it to the list.
            foreach (var role in roles)
            {
                // We create an empty state of the world, which we will later transform into the goal state.
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

            // Returning a list of goals.
            return goals;
        }

        /// <summary>
        /// A method that creates a set of beliefs to convey to agents.
        /// </summary>
        public List<WorldContext> CreateBeliefsSet(int count)
        {
            // Create an empty list of beliefs.
            List<WorldContext> newWorldBeliefsList = new List<WorldContext>();

            // We create the required number of instances of the agent representing the belief class and add them to the list.
            // We will fill them in at a later stage.
            for (int i = 0; i < count; i++)
            {
                WorldContext newWorldBeliefs = new WorldContext();
                newWorldBeliefsList.Add(newWorldBeliefs);
            }

            // Returning a list of beliefs.
            return newWorldBeliefsList;
        }

        /// <summary>
        /// A method that shuffles locations in a transferred set of locations.
        /// </summary>
        public void OrderLocationsRandom(ref Dictionary<LocationStatic, LocationDynamic> locations)
        {
            // Sorting locations based on their hashcode values.
            locations = locations.OrderBy(x => x.Value.GetHashCode()).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// A method that verifies that all locations in the transferred set are connected (there is a way that can bypass all locations).
        /// </summary>
        public bool pathExistenceControlling(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool result = false;

            // We create a queue and a set of visited locations.
            Queue<LocationStatic> queue = new Queue<LocationStatic>();
            HashSet<LocationStatic> visitedLocations = new HashSet<LocationStatic>();

            // We will use the first location from the list of locations as the root.
            LocationStatic root = locations.First().Key;

            // Add the root to the queue and the list of visited locations.
            queue.Enqueue(root);
            visitedLocations.Add(root);

            // As long as there is a location in the queue.
            while (queue.Count > 0)
            {
                // We take the location from the queue.
                LocationStatic currentLocation = queue.Dequeue();

                // We go through all the locations associated with the checked location.
                foreach (LocationStatic nextLocation in currentLocation.GetConnectedLocations())
                {
                    // If we have already visited one of these locations, then we just continue.
                    if (visitedLocations.Contains(nextLocation)) continue;

                    // Otherwise, add a new "unknown" location to the queue and the list of visited locations.
                    queue.Enqueue(nextLocation);
                    visitedLocations.Add(nextLocation);
                }
            }

            // If the number of locations visited during the passage is equal to the total number of locations, then the check was successful.
            if (visitedLocations.Count == locations.Count) { result = true; }

            return result;
        }

        /// <summary>
        /// A method that randomly assigns connections between locations.
        /// </summary>
        public Dictionary<LocationStatic, LocationDynamic> LocationsConnection(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            bool pathExists = false;

            // Until the path is created.
            while (!pathExists)
            {
                // We go through all the locations in the list and clear their connections with other locations.
                foreach (var loc in locations)
                {
                    loc.Key.ClearAllConnections();
                }

                // We go through all the locations in the location list.
                foreach (var location in locations)
                {
                    Random random = new Random();

                    int connectionLimit = random.Next(1, 3);

                    // While the location has no connections with other locations.
                    while (location.Key.GetConnectedLocations().Count == 0)
                    {
                        // We get a random number.
                        int rand = random.Next(locations.Count);

                        // If the currently considered location is not equal to the location from the list, whose index is equal 
                        //   to the received random number, and these locations have no connection with each other, and the number of connections 
                        //   of the location in question is no more than three, as well as the number of links for a location obtained by an index 
                        //   equal to a random number.
                        if (!location.Equals(locations.ElementAt(rand)) && !location.Key.ConnectionChecking(locations.ElementAt(rand).Key)
                               && location.Key.GetConnectedLocations().Count < connectionLimit 
                               && locations.ElementAt(rand).Key.GetConnectedLocations().Count < connectionLimit)
                        {
                            // Then we connect these locations with each other.
                            location.Key.AddConnection(locations.ElementAt(rand).Key);
                            locations.ElementAt(rand).Key.AddConnection(location.Key);
                        }
                    }
                }

                // We check if there is a path that allows you to bypass all locations (i.e. are they all connected).
                pathExists = pathExistenceControlling(locations);
            }

            // Returns locations with established connections between them.
            return locations;
        }

        /// <summary>
        /// Method is an entry point that controls the operation of the algorithm (the sequence of launching other methods).
        /// </summary>
        public void Start()
        {
            // We read the settings and create the initial state of the world.
            // ReadUserSettingsInput();
            CreateInitialState();
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
            graphСonstructor.CreateGraph(newStoryGraph, @"D:\Graphviz\bin\newStoryGraph.dt");

            // Create an HTML file including Twine engine and generated history.
            //twineGraphConstructor.ConstructGraph(newStoryGraph);
            //twineGraphConstructor.CreateHTMLFileWithGame();

            // SaveFile();
        }

        /// <summary>
        /// A method in which we sequentially create a story graph, node by node, starting at the root, using the concept of Breadth First Search.
        /// </summary>
        public void BFSTraversing(StoryNode rootNode, bool root = true)
        {
            // We create a queue and a list of visited nodes.
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            // Flag for early completion of the verification stage and transition to the next.
            bool skip = false;

            // Counter of wrong states (dead ends, cycles, skips).
            int doomCounter = 0;

            // Добавляем узел в очередь и список посещённых узлов.
            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);

            // We initialize numeric variables storing the number of the current agent and the number of the last node in the sequence.
            int actualAgentNumber = 0;
            int globalNodeNumber = -1;

            // We will perform the action in a loop until the queue becomes empty or the number of consecutive errors reaches ten.
            while (queue.Count > 0 /*&& newStoryGraph.GetNodes().Count < 500*/ && doomCounter < 10)
            {
                skip = false;

                // We take the node in question from the queue.
                StoryNode currentNode = queue.Dequeue();

                // If we come across a node with a target state, then we do not expand it.
                if (storyworldConvergence.ControlToAchieveGoalState(currentNode.GetWorldState()))
                {
                    currentNode.goalState = true;
                    continue;
                }
                else
                {
                    // If the node in question is a root.
                    if (currentNode.Equals(rootNode) && root)
                    {
                        // We call the method for creating a new node.
                        Step(newStoryGraph.GetRoot(), actualAgentNumber, root, ref globalNodeNumber, ref skip, ref queue);

                        // Add the considered node back to the queue (in the case of the root, we only changed it and will consider it again),
                        //    and also to the list of visited nodes. We remove the flag indicating that we are considering the root node.
                        queue.Enqueue(currentNode);
                        visitedNodes.Add(currentNode);
                        root = false;
                    }
                    // If the node in question IS NOT a root.
                    else
                    {
                        // We determine the index of the agent, which will have to act when creating a new node.
                        actualAgentNumber = GetActualAgentNumber(currentNode.GetWorldState().GetIndexOfAgent(currentNode.GetActiveAgent()));

                        // We call the method to create a new node.
                        Step(newStoryGraph.GetNode(currentNode), actualAgentNumber, root, ref globalNodeNumber, ref skip, ref queue);
                    }
                }

                // If an error occurs while creating a node and the agent is forced to skip a turn.
                if (skip)
                {
                    // We get the index of the agent who was acting now.
                    actualAgentNumber = GetActualAgentNumber(currentNode.GetWorldState().GetIndexOfAgent(currentNode.GetActiveAgent()));

                    // We write it to the node in question, as if it performed an action in the previous step, overwriting information 
                    //    about that agent, who actually did it. This is necessary so that when we try to expand this node next time,
                    //    then the next agent in the queue was already acting, and not the one that just missed the move.
                    currentNode.SetActiveAgent(currentNode.GetWorldState().GetAgentByIndex(actualAgentNumber));

                    // Add the node in question back to the queue and to the list of visited nodes, if it is not already there.
                    queue.Enqueue(currentNode);
                    if (!visitedNodes.Contains(currentNode)) { visitedNodes.Add(currentNode); }

                    // We increase the counter of consecutive errors and move on to the next step.
                    doomCounter++;
                    continue;
                }

                // If no errors occurred at this step, reset their counter.
                doomCounter = 0;

                // We go through the nodes associated with the considered one.
                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    // If one of them has already been visited earlier, then we continue, moving on to the next.
                    if (visitedNodes.Contains(nextNode)) continue;

                    // Otherwise, we add them to the queue and the list of visited nodes.
                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }

                // We clear the link pointing to the node in question.
                currentNode = null;
            }
        }

        /// <summary>
        /// A method that traverses the graph according to the concept of breadth-first search 
        ///    and determines the presence of at least one target state in it.
        /// </summary>
        public bool BFSGoalAchieveControl(StoryNode rootNode)
        {
            // We create a queue and a list of visited nodes.
            Queue<StoryNode> queue = new Queue<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            // Add the root node to the queue and the list of visited nodes.
            queue.Enqueue(rootNode);
            visitedNodes.Add(rootNode);

            // We are in a loop until the queue is empty.
            while (queue.Count > 0)
            {
                // We take the current node under consideration from the queue.
                StoryNode currentNode = queue.Dequeue();

                // We check if the target state has been reached in the node under consideration.
                reachedGoalState = storyworldConvergence.ControlToAchieveGoalState(currentNode.GetWorldState());

                // If so, terminate the method and return true.
                if (reachedGoalState) { return true; }

                // Otherwise, we go through the nodes associated with the node in question.
                foreach (StoryNode nextNode in currentNode.GetLinks())
                {
                    // If they have already been visited earlier, then we continue, moving on to the next.
                    if (visitedNodes.Contains(nextNode)) continue;

                    // Otherwise, we add to the queue and the list of visited nodes.
                    queue.Enqueue(nextNode);
                    visitedNodes.Add(nextNode);
                }
            }

            // If no target state was found, then terminate the method and return false.
            return false;
        }

        /// <summary>
        /// The method that controls the creation of the history graph.
        /// </summary>
        public StoryGraph CreateStoryGraph(StoryNode rootNode)
        {
            // We clone the root into a separate variable.
            StoryNode originRoot = (StoryNode)rootNode.Clone();

            // We continue to work until at least one target state is reached.
            while (!reachedGoalState /*&& newStoryGraph.GetNodes().Count < 500*/)
            {
                // We create a new graph by starting to expand the root.
                BFSTraversing(newStoryGraph.GetRoot());

                // We go through the created graph, looking for target states in it.
                BFSGoalAchieveControl(newStoryGraph.GetRoot());

                // If it was not possible to find even one target state in the constructed graph.
                if (!reachedGoalState)
                {
                    // Then we clear the graph, and all the links added to the root.
                    newStoryGraph = new StoryGraph();
                    originRoot.GetEdges().Clear();
                    originRoot.GetLinks().Clear();

                    // After that, we reassign to the previously cleared column an indication of the root.
                    newStoryGraph.SetRoot((StoryNode)originRoot.Clone());
                }
            }

            // We return a graph that is guaranteed to have at least one target state.
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

            // We check if the agent from whom we are going to request an action is alive (i.e. capable of doing it).
            if (currentStoryState.GetAgentByIndex(agentIndex).Value.GetStatus())
            {
                // We create a request for action of the specified agent from the specified state.
                storyworldConvergence.ActionRequest(currentStoryState.GetAgentByIndex(agentIndex), ref newStoryGraph, ref currentStoryState, 
                                                     currentNode, root, ref globalNodeNumber, ref skip, ref queue);
            }
        }

        /// <summary>
        /// A method that returns the index of the agent that should perform the action.
        /// </summary>
        /// <param name="prevNumber">Index of the agent who performed the action in the previous state.</param>
        public int GetActualAgentNumber(int prevNumber)
        {
            // Flag for checking if the agent being checked is alive.
            bool aliveControl = false;

            // Determine how many agents exist in the current state.
            int maxNumber = currentStoryState.GetNumberOfAgents();

            // Default result.
            int result = 0;

            // Until the flag signaling the status of the agent being checked is omitted.
            while (!aliveControl)
            {
                // If the index of the previous agent is equal to the maximum possible index.
                if (prevNumber == maxNumber - 1)
                {
                    // Then we go back to the beginning of the index list and set 0 as the result (the circle is passed).
                    result = 0;
                    prevNumber = 0;
                }
                else
                {
                    // Otherwise, we increase the value of the index of the previous agent by one and write it down as a result.
                    prevNumber++;
                    result = prevNumber;
                }

                // We check if the agent with the received index is alive. If not, we continue the cycle.
                if (!currentStoryState.GetAgentByIndex(result).Value.GetStatus()) { continue; }

                // Otherwise, we raise the flag that the control has been passed.
                aliveControl = true;
            }

            // We return the result - the index of the guaranteed living agent acting after the specified in the parameters.
            return result;
        }
    }
}