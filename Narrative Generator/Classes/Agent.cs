using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Agent
    {
        FastDownward fastDownward;
        Plan myCurrentPlan;
        List<Action> myAvailableActions;
        List<Goal> myGoals;
        World myWorld; // Beliefs

        private bool alive;
        private string role;

        public Agent(bool alive, string role, List<Goal> goals, World world)
        {
            fastDownward = new FastDownward();
            myCurrentPlan = new Plan();
            myAvailableActions = new List<Action>();
            myGoals = goals;
            myWorld = world;
            this.alive = alive;
            this.role = role;
        }

        public void AssignRole(string role)
        {
            this.role = role;
        }

        public string GetRole()
        {
            return role;
        }

        public void RefreshKnowledgeAboutTheWorld(World currentWorldState) // Should depend on factors such as role, location of the agent, and so on. 
                                                                           // The system shouldn't tell the agent who the killer is.
        {
            myWorld = currentWorldState;
        }

        public void CalculatePlan()
        {
            fastDownward.Run();

            if (fastDownward.isSuccess)
            {
                myCurrentPlan = fastDownward.GetResultPlan();
            }
        }

        public void GetAvailableActions()
        {

        }

        public void ChooseAction(List<Action> availableActions)
        {
            CalculatePlan();
        }

        public void SendAction(Action choosedAction)
        {

        }

        public bool GetStatus()
        {
            return alive;
        }

        public void Die()
        {
            alive = false;
        }
    }
}
