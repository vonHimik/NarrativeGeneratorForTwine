using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Fight : PlanAction
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

        public Fight (WorldDynamic state) { DefineDescription(state); }

        public Fight (params Object[] args) : base (args) { }

        public Fight (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1, 
                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2, 
                      ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(location);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus()
                    && agent.Value.GetObjectOfAngryComponent().AngryCheck()
                    && state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetStatus()
                    && state.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Key.
                       Equals(state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetBeliefs().GetMyLocation());
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return    (Agent1.Key.GetRole() == AgentRole.USUAL || Agent1.Key.GetRole() == AgentRole.PLAYER) && Agent1.Value.GetStatus() 
                      && Agent2.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key) 
                      && Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Agent2.Key);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent2.Value.SetStatus(false);

            stateAgent.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state)
        {
            fail = true;

            //if (state.GetStaticWorldPart().GetRandomBattlesResultsStatus())
            //{
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent1.Key.GetName());
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

                stateAgent.Value.ClearTempStates();
                stateAgent2.Value.ClearTempStates();

                stateAgent.Value.SetStatus(false);
            //}
        }
    }
}
