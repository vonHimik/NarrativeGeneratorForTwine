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

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            if (state.GetLocation(state.SearchAgentAmongLocations(agent.Key)).Value.CountAliveAgents() == 2)
            {
                foreach (var person in state.GetLocation(state.SearchAgentAmongLocations(agent.Key)).Value.GetAgents())
                {
                    if (!person.Key.Equals(agent.Key) && agent.Value.GetStatus() && person.Key.GetRole().Equals(AgentRole.BOSS))
                    {
                        return false;
                    }
                }

                return agent.Value.GetStatus();
            }
            else
            {
                return state.GetLocation(state.SearchAgentAmongLocations(agent.Key)).Value.CountAliveAgents() >= 2 && agent.Value.GetStatus();
            }
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent1.Value.GetStatus() && Agent2.Value.GetStatus() && !(Agent1.Equals(Agent2)) && !Agent2.Key.GetRole().Equals(AgentRole.BOSS)
                   && (Agent1.Value.GetMyLocation().Equals(Agent2.Value.GetMyLocation()));
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent1.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent1.Value.SetInterlocutor(stateAgent2);
            stateAgent2.Value.SetInterlocutor(stateAgent1);
<<<<<<< Updated upstream

            if (stateAgent1.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent1.Value.GetEvidenceStatus().CheckEvidence() &&
                stateAgent2.Key.GetRole() != AgentRole.KILLER)
            {
                stateAgent2.Value.AddEvidence(stateAgent1.Value.GetObjectOfAngryComponent().GetObjectOfAngry());
                stateAgent2.Value.GetBeliefs().GetAgentByName(
                    stateAgent1.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).AssignRole(AgentRole.KILLER);
                stateAgent2.Value.SetObjectOfAngry(stateAgent1.Value.GetObjectOfAngryComponent());
            }
            else if (stateAgent2.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent2.Value.GetEvidenceStatus().CheckEvidence() &&
                stateAgent1.Key.GetRole() != AgentRole.KILLER)
            {
                stateAgent1.Value.AddEvidence(stateAgent2.Value.GetObjectOfAngryComponent().GetObjectOfAngry());
                stateAgent1.Value.GetBeliefs().GetAgentByName(
                    stateAgent2.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).AssignRole(AgentRole.KILLER);
                stateAgent1.Value.SetObjectOfAngry(stateAgent2.Value.GetObjectOfAngryComponent());
            }

            stateAgent1.Value.DecreaseTimeToMove();
=======
>>>>>>> Stashed changes
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
