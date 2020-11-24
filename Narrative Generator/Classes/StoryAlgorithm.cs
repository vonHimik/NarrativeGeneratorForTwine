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
        public bool reachedGoalState = false;
        public StoryGraph newStoryGraph = new StoryGraph();

        public World currentStoryState = new World();
        public StoryworldConvergence storyworldConvergence = new StoryworldConvergence();
        public List<Agent> agents = new List<Agent>();

        public void ReadUserSettingsInput()
        {

        }

        public void GenerateNewPDDLDomain()
        {

        }

        public void GenerateNewPDDLProblem()
        {

        }

        // We reading predicates and actions from domain.pddl file.
        public void ReadPDDLDomain()
        {
           // newStoryGraph.startNode = ...
        }

        // We reading objects, init state and goals from problem.pddl file.
        public void ReadPDDLProblem()
        {
            
        }

        public void CreateWorld()
        {

        }

        public void CreateStoryworldConvergence()
        {
            storyworldConvergence.SetNewStoryState(currentStoryState);
            storyworldConvergence.SetListOfAgents(agents);
        }

        public void CreateAgents()
        {
            // We get info about agents from user input.
            // From it we find out how many agents there are, what roles they have, their beliefs, 
            //    and we will have to design them and add them to the game world.
        }

        public void CreateAgent()
        {

        }

        public void CreateEnviroment()
        {

        }

        public void DistributionOfInitiative()
        {

        }

        public void Start()
        {
            // ReadUserSettingsInput();
            CreateWorld();
            CreateStoryworldConvergence();
            CreateAgents();

            StoryNode rootNode = newStoryGraph.startNode;
            newStoryGraph = CreateStoryGraph(rootNode);
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
