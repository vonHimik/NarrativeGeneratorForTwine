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

        public Agent()
        {

        }

        public void AssignRole()
        {

        }

        public string GetRole()
        {
            return role;
        }

        public void RefreshKnowledgeAboutTheWorld()
        {

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

        public void ChooseAction()
        {

        }

        public void SendAction()
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
