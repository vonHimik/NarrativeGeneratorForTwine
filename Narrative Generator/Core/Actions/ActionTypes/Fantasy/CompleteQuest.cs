﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CompleteQuest : PlanAction
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

        public CompleteQuest (params Object[] args) : base(args) { }

        public CompleteQuest (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                              ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(location);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy) && agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && agent.Value.GetStatus() && state.GetLocationByName(state.SearchAgentAmongLocations(agent.Key).GetName()).Value.SearchAgent(agent.Key);
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy) && Agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && Agent.Value.GetStatus() && Location.Value.SearchAgent(Agent.Key);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            stateAgent.Value.CompleteQuest();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
