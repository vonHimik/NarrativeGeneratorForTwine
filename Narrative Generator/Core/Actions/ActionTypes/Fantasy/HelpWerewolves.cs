using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class HelpWerewolves : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[1];
            }
        }

        public HelpWerewolves (WorldDynamic state) { DefineDescription(state); }

        public HelpWerewolves (params Object[] args) : base(args) { }

        public HelpWerewolves (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                               ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(location);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) && agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && agent.Value.GetStatus() && state.SearchAgentAmongLocations(agent.Key).GetName().Equals("BrecilianForest")
                && state.GetLocationByName(state.SearchAgentAmongLocations(agent.Key).GetName()).Value.SearchAgent(agent.Key);
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) && Agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && Agent.Value.GetStatus() && Location.Key.GetName().Equals("BrecilianForest") && Location.Value.SearchAgent(Agent.Key);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            stateAgent.Value.CompleteQuest();
            state.helpWerewolves = true;
        }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
