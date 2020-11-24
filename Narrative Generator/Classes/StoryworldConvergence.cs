using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryworldConvergence
    {
        private World currentStoryState; // This is absolutely true and complete information about the world, unlike other agents. 

        List<Constraint> constraints;

        private List<Agent> agents;

        public void SetNewStoryState(World newStoryState)
        {
            currentStoryState = newStoryState;
        }

        public World GetCurrentStoryState()
        {
            return currentStoryState;
        }

        public void SetListOfAgents(List<Agent> agents)
        {
            this.agents = agents;
        }

        public List<Agent> GetListOfAgents()
        {
            return agents;
        }

        public void ActionRequest()
        {

        }

        public void ApplyAction()
        {

        }
    }
}
