using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Reassure : PlanAction
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

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent3
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[2];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[3];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[4];
            }
        }

        public Reassure (WorldDynamic state) { DefineDescription(state); }

        public Reassure(params Object[] args) : base (args) { }

        public Reassure(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent3, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                        ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(agent3);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return state.GetStaticWorldPart().Equals(Setting.DefaultDemo) && (agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER))
                && agent.Value.GetStatus() && agent.Value.ThinksThatSomeoneIsAngry();
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus() 
                      && Agent2.Key.GetRole() == AgentRole.USUAL && Agent2.Value.GetStatus()
                      && Agent3.Key.GetRole() == AgentRole.USUAL 
                      && Killer.Key.GetRole() == AgentRole.ANTAGONIST 
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key)
                      && (Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Agent3.Key) || Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Killer.Key))
                      && !Agent1.Value.GetBeliefs().GetAgentByRole(AgentRole.ANTAGONIST).Equals(Killer);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent3 = state.GetAgentByName(Agent3.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateAgent1.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();
            stateAgent3.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateAgent1.Value.CalmDown();

            stateAgent2.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
