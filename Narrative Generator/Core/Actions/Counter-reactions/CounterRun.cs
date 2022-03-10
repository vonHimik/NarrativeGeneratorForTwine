﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CounterRun : PlanAction
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

        public string OriginalAction
        {
            get
            {
                return (string)Arguments[3];
            }
        }

        public CounterRun(params Object[] args) : base(args) { }

        public CounterRun(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                          ref KeyValuePair<LocationStatic, LocationDynamic> from,
                          ref KeyValuePair<LocationStatic, LocationDynamic> to,
                          string originalAction)
        {
            Arguments.Add(agent);
            Arguments.Add(from);
            Arguments.Add(to);
            Arguments.Add(originalAction);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Value.GetStatus() && Agent.Value.CheckScared() && From.Value.SearchAgent(Agent.Key) && !To.Value.SearchAgent(Agent.Key);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<LocationStatic, LocationDynamic> stateFrom = state.GetLocationByName(From.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateTo = state.GetLocationByName(To.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            stateAgent.Value.ClearTempStates();

            stateFrom.Value.RemoveAgent(Agent);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).ClearLocation();

            stateTo.Value.AddAgent(Agent);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).SetLocation(stateTo.Key);

            if (stateTo.Key == stateAgent.Value.GetTargetLocation())
            {
                stateAgent.Value.ClearTargetLocation();
            }
        }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
