﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Run : PlanAction
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

        public Run(params Object[] args) : base(args) { }

        public Run(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                   ref KeyValuePair<LocationStatic, LocationDynamic> from, 
                   ref KeyValuePair<LocationStatic, LocationDynamic> to)
        {
            Arguments.Add(agent);
            Arguments.Add(from);
            Arguments.Add(to);
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

            stateFrom.Value.RemoveAgent(Agent);
            stateAgent.Value.GetBeliefs().GetLocationByName(stateFrom.Key.GetName()).Value.RemoveAgent(stateAgent);

            stateTo.Value.AddAgent(Agent);
            stateAgent.Value.GetBeliefs().GetLocationByName(stateTo.Key.GetName()).Value.AddAgent(stateAgent);

            if (stateTo.Key == stateAgent.Value.GetTargetLocation())
            {
                stateAgent.Value.ClearTargetLocation();
            }
        }
    }
}