using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class NothingToDo : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public NothingToDo (WorldDynamic state) { DefineDescription(state); }

        public NothingToDo (params Object[] args) : base (args) { }

        public NothingToDo (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            Arguments.Add(agent);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.ANTAGONIST)) && agent.Value.GetStatus();
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Value.GetStatus();
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            stateAgent.Value.IncreaseSkipedTurns();

            stateAgent.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
