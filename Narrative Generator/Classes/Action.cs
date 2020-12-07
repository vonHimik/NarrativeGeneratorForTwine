using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Action
    {
        private string name;

        public bool forAllAgents = false;
        public bool forUsualAgents = false;
        public bool forKillers = false;

        public Action (string name, bool forAllAgents, bool forUsualAgents, bool forKillers)
        {
            SetName(name);
            this.forAllAgents = forAllAgents;
            this.forUsualAgents = forUsualAgents;
            this.forKillers = forKillers;
        }

        public virtual void Clear()
        {
            name = null;
        }

        public virtual void SetName (string name)
        {
            this.name = name;
        }

        public virtual string GetName()
        {
            return name;
        }

        // Agent Move
        public bool CheckingPreconditions (ref Agent agent, ref Location from, ref Location to)
        {
            if (agent.GetStatus() && from.SearchForAnAgent(agent) && !to.SearchForAnAgent(agent))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ApplyEffects (ref Agent agent, ref Location from, ref Location to)
        {
            from.RemoveAgent(agent);
            to.AddAgent(agent);
        }
        public void TakeAction (ref Agent agent, ref Location from, ref Location to)
        {
            if (CheckingPreconditions (ref agent, ref from, ref to))
            {
                ApplyEffects (ref agent, ref from, ref to);
            }
        }

        // Kill
        public bool CheckingPreconditions (ref Agent agent, ref Agent killer, ref Location location)
        {
            if (agent.GetRole() == "usual" && agent.GetStatus() && killer.GetRole() == "killer" && killer.GetStatus() 
                   && location.SearchForAnAgent(agent) && location.SearchForAnAgent(killer) && location.CountAgents() == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ApplyEffects(ref Agent agent, ref Agent killer, ref Location location)
        {
            agent.SetStatus(false);
        }
        public void TakeAction(ref Agent agent, ref Agent killer, ref Location location)
        {
            if (CheckingPreconditions (ref agent, ref killer, ref location))
            {
                ApplyEffects(ref agent, ref killer, ref location);
            }
        }
    }
}
