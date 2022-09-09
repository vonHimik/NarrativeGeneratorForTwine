using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CounterTalk : PlanAction
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

        public string OriginalAction
        {
            get
            {
                return (string)Arguments[2];
            }
        }

        public CounterTalk (WorldDynamic state) { DefineDescription(state); }

        public CounterTalk (params Object[] args) : base(args) { }

        public CounterTalk (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                            ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2,
                            string originalAction)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(originalAction);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Value.GetStatus() && Agent2.Value.GetStatus() /*&& !(Agent1.Key.GetName() == Agent2.Key.GetName())*/
                   && /*(Agent1.Value.GetMyLocation().Equals(Agent2.Value.GetMyLocation()))*/
                   Agent1.Value.GetMyLocation().GetName() == Agent2.Value.GetMyLocation().GetName();
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent1.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent1.Value.SetInterlocutor(stateAgent2);
            stateAgent2.Value.SetInterlocutor(stateAgent1);

            if (stateAgent1.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent1.Value.GetEvidenceStatus().CheckEvidence() &&
                stateAgent2.Key.GetRole() != AgentRole.ANTAGONIST)
            {
                stateAgent2.Value.SetObjectOfAngry(stateAgent1.Value.GetObjectOfAngryComponent());
            }
            else if (stateAgent2.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent2.Value.GetEvidenceStatus().CheckEvidence() &&
                     stateAgent1.Key.GetRole() != AgentRole.ANTAGONIST)
            {
                stateAgent1.Value.SetObjectOfAngry(stateAgent2.Value.GetObjectOfAngryComponent());
            }

            stateAgent1.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
