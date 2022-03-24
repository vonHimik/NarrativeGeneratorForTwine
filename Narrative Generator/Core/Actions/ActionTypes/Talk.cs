using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Talk : PlanAction
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

        public Talk(params Object[] args) : base(args) { }

        public Talk  (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent1.Value.GetStatus() && Agent2.Value.GetStatus() && !(Agent1.Key.GetName() == Agent2.Key.GetName())
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

            if (stateAgent1.Value.GetObjectOfAngry().AngryCheck() && stateAgent1.Value.GetEvidenceStatus().CheckEvidence())
            {
                stateAgent2.Value.SetObjectOfAngry(stateAgent1.Value.GetObjectOfAngry());
            }
            else if (stateAgent2.Value.GetObjectOfAngry().AngryCheck() && stateAgent2.Value.GetEvidenceStatus().CheckEvidence())
            {
                stateAgent1.Value.SetObjectOfAngry(stateAgent2.Value.GetObjectOfAngry());
            }

            stateAgent1.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
