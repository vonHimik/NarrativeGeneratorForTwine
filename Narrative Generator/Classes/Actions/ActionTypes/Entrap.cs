﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Entrap : PlanAction
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

        public Entrap(params Object[] args) : base(args) { }

        public Entrap(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                      ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override bool CheckPreconditions(WorldBeliefs state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus() 
                   && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                   && !Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key) && Location.Value.CountAgents() == 1;
        }

        public override void ApplyEffects(WorldBeliefs state)
        {
            Agent.Value.SetTargetLocation(Location.Key);
        }
    }
}
