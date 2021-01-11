using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Run : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> From
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> To
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        public Run(params Object[] args) : base(args) { }

        public Run(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                   ref KeyValuePair<LocationStatic, LocationDynamic> from, 
                   ref KeyValuePair<LocationStatic, LocationDynamic> to)
        {
            Arguments.Add(agent);
            Arguments.Add(from);
            Arguments.Add(to);
        }

        public override bool CheckPreconditions(WorldBeliefs state)
        {
            return Agent.Value.GetStatus() && Agent.Value.CheckScared() && From.Value.SearchAgent(Agent.Key) && !To.Value.SearchAgent(Agent.Key);
        }

        public override void ApplyEffects(WorldBeliefs state)
        {
            From.Value.RemoveAgent(Agent);
            To.Value.AddAgent(To.Key, Agent);

            if (To.Key == Agent.Value.GetTargetLocation())
            {
                Agent.Value.ClearTargetLocation();
            }
        }
    }
}
