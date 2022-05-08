﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CounterNeutralizeKiller : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
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

        public CounterNeutralizeKiller(params Object[] args) : base(args) { }

        public CounterNeutralizeKiller(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                       ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer,
                                       ref LocationStatic location,
                                       string originalAction)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
            Arguments.Add(originalAction);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                      && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key)
                      && Agent.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Killer.Key);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateKiller.Value.SetStatus(false);

            stateAgent.Value.DecreaseTimeToMove();
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}