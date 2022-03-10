using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CounterFight : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent1
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent2
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        public string OriginalAction
        {
            get
            {
                return (string)Arguments[3];
            }
        }

        public CounterFight(params Object[] args) : base(args) { }

        public CounterFight(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                            ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2,
                            ref KeyValuePair<LocationStatic, LocationDynamic> location,
                            string originalAction)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(location);
            Arguments.Add(originalAction);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus()
                      && Agent2.Key.GetRole() == AgentRole.KILLER && Agent2.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key)
                      && Agent1.Value.GetObjectOfAngry().AngryCheckAtAgent(Agent2.Key);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent2.Value.SetStatus(false);
        }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
