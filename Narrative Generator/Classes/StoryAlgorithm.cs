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
        public World currentStoryState = new World();

        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();

        public void ReadUserSettingsInput()
        {

        }

        public void GenerateNewPDDLDomain()
        {

        }

        public void GenerateNewPDDLProblem()
        {

        }

        public void CreateStartState()
        {
            if (manualInput) // Demo story
            {
                Location kitchen = new Location("Kitchen");
                Location diningRoom = new Location("Dining room");
                Location hall = new Location("Hall");
                Location garden = new Location("Garden");
                Location bedroom = new Location("Bedroom");
                Location guestBedroom = new Location("Guest bedroom");
                Location bathroom = new Location("Bathroom");
                Location attic = new Location("Attic");

                List<Location> locations = new List<Location>
                {
                    kitchen, diningRoom, hall, garden, bedroom, guestBedroom, bathroom, attic
                };

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

                List<string> roles = new List<string>()
                {
                    "usual", "usual", "usual", "usual", "usual", "killer"
                };

                World standardAgentsGoalState = new World();
                Goal standardAgentGoal = new Goal(false, true, false, standardAgentsGoalState);

                World killersGoalState = new World();
                Goal killerAgentGoal = new Goal(false, true, false, killersGoalState);

                List<Goal> goals = new List<Goal>()
                {
                    standardAgentGoal, standardAgentGoal, standardAgentGoal, standardAgentGoal, standardAgentGoal, killerAgentGoal
                };

                World usualAgentsBeliefs = new World();
                World killerBeliefs = new World();
                List<World> beliefs = new List<World>
                {
                    usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, usualAgentsBeliefs, killerBeliefs
                };

                // The second step in creating the initial state is the creation of agents, initially with empty goals and beliefs, 
                // since they are highly dependent on the agents themselves existing in the "world". We'll finish setting this up in the next step.
                CreateAgents(names, statuses, roles, goals, beliefs, 6);

                DistributionOfInitiative();
                currentStoryState.OrderByInitiative();

                // The third step in creating an initial state is assigning to agents their goals and beliefs.
                // Goals
                for (int i = 0; i < currentStoryState.GetAgents().Count(); i++)
                {
                    if (currentStoryState.GetAgent(i).GetGoal().goalTypeStatus == true)
                    {
                        if (currentStoryState.GetAgent(i).GetRole() != "killer")
                        {
                            for (int j = 0; j < currentStoryState.GetAgents().Count(); j++)
                            {
                                if ((j + 1) != (currentStoryState.GetAgents().Count()))
                                {
                                    currentStoryState.GetAgent(i).GetGoal().goalState.AddEmptyAgent();
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).AssignRole("usual");
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).SetStatus(true);
                                }
                                else 
                                {
                                    currentStoryState.GetAgent(i).GetGoal().goalState.AddEmptyAgent();
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).AssignRole("killer");
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).SetStatus(false);
                                }
                            }
                        }
                        else if (currentStoryState.GetAgent(i).GetRole() == "killer")
                        {
                            for (int j = 0; j < currentStoryState.GetAgents().Count(); j++)
                            {
                                if ((j + 1) != (currentStoryState.GetAgents().Count()))
                                {
                                    currentStoryState.GetAgent(i).GetGoal().goalState.AddEmptyAgent();
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).AssignRole("usual");
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).SetStatus(false);
                                }
                                else
                                {
                                    currentStoryState.GetAgent(i).GetGoal().goalState.AddEmptyAgent();
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).AssignRole("killer");
                                    currentStoryState.GetAgent(i).GetGoal().goalState.GetAgent(j).SetStatus(true);
                                }
                            }
                        }
                    }
                }
                // Beliefs
                for (int i = 0; i < currentStoryState.GetAgents().Count(); i++)
                {
                    for (int j = 0; j < currentStoryState.GetAgents().Count(); j++)
                    {
                        if (i == j)
                        {
                            currentStoryState.GetAgent(i).GetBeliefs().AddAgent(currentStoryState.GetAgent(i));
                        }
                        else
                        {
                            currentStoryState.GetAgent(i).GetBeliefs().AddEmptyAgent();
                            currentStoryState.GetAgent(i).GetBeliefs().GetAgent(j).AssignRole("usual");
                            currentStoryState.GetAgent(i).GetBeliefs().GetAgent(j).SetName(currentStoryState.GetAgent(j).GetName());
                        }
                    }
                }
                // Locations
                for (int i = 0; i < currentStoryState.GetAgents().Count(); i++)
                {
                    currentStoryState.GetAgent(i).GetBeliefs().AddLocations(currentStoryState.GetLocations());
                }
            }
        }

        public void CreateAgents (List<string> names, List<bool> statuses, List<string> roles, List<Goal> goals, List<World> beliefs, int numbers)
        {
            // We get info about agents from user input.
            // From it we find out how many agents there are, what roles they have, their beliefs, 
            //    and we will have to design them and add them to the game world.

            for (int i = 0; i < numbers; i++)
            {
                CreateAgent(names[i], statuses[i], roles[i], goals[i], beliefs[i]);
            }
        }

        public void CreateAgent(string name, bool status, string role, Goal goals, World beliefs)
        {
            Agent newAgent = new Agent(name, status, role, goals, beliefs);
            currentStoryState.AddAgent(newAgent);
        }

        public void CreateEnviroment(List<Location> locations)
        {
            currentStoryState.AddLocations(locations);
        }

        public void DistributionOfInitiative()
        {
            List<int> valuesOfInitiative = NumberGenerator(currentStoryState.GetNumberOfAgents());

            for (int i = 0; i < currentStoryState.GetNumberOfAgents(); i++)
            {
                currentStoryState.GetAgent(i).SetInitiative(valuesOfInitiative[i]);
            }
        }

        public List<int> NumberGenerator (int maximum)
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

        public void CreateConstraints()
        {
            if (manualInput) // Constraints for demo-story
            {
                Constraint killerMustBeAliveFiveTurns = new Constraint(true, false, currentStoryState.GetAgentByRole("killer"), 5);
                storyworldConvergence.AddConstraint(killerMustBeAliveFiveTurns);
            }
        }

        public void Start()
        {
            // ReadUserSettingsInput();
            CreateStartState();
            CreateConstraints();

            newStoryGraph.startNode.SetWorldState(currentStoryState);
            newStoryGraph.startNode.SetActivePlayer(false);
            newStoryGraph.startNode.SetActiveAgent(currentStoryState.GetAgent(0));
            newStoryGraph = CreateStoryGraph(newStoryGraph.startNode);
            GenerateGraphForTwine(newStoryGraph);
        }

        public StoryGraph CreateStoryGraph(StoryNode rootNode)
        {
            StoryNode currentNode = rootNode;

            while (!reachedGoalState)
            {
                StoryNode nextNode = Step(currentNode);
            }

            return newStoryGraph;
        }

        public StoryNode Step (StoryNode currentNode)
        {
            // newStoryGraph.ExpandNode();
            return currentNode;
        }

        public void GenerateGraphForTwine(StoryGraph storyGraph)
        {

        }
    }
}
