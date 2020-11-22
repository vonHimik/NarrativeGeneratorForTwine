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

        public void CreateStoryworldConvergence()
        {

        }

        public void CreateAgents(int number)
        {
            for (int i = 0; i < number; i++)
            {
                CreateAgent();
            }
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
            ReadPDDLDomain();
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
            newStoryGraph.ExpandNode();
            return currentNode;
        }

        public void GenerateGraphForTwine(StoryGraph storyGraph)
        {

        }
    }
}
