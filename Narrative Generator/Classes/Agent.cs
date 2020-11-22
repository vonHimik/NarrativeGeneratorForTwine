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
        World myWorld;

        private bool alive;
        private string role;

        public Agent(bool alive, string role)
        {
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

        public void RefreshKnowledgeAboutTheWorld(World currentWorldState)
        {
            this.myWorld = currentWorldState;
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
