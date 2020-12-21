using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class InvestigateRoom : PlanAction
    {
        public Agent Agent
        {
            get
            {
                return (Agent)Arguments[0];
            }
        }

        public Agent Killer
        {
            get
            {
                return (Agent)Arguments[1];
            }
        }

        public Location Location
        {
            get
            {
                return (Location)Arguments[2];
            }
        }

        public InvestigateRoom(params Object[] args) : base(args) { }

        public InvestigateRoom(ref Agent agent, ref Agent killer, ref Location location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override bool CheckPreconditions(WorldBeliefs state)
        {
            return Agent.GetRole() == "usual" && Agent.GetStatus() && Killer.GetRole() == "killer" && Killer.GetStatus()
                      && Location.SearchAgent(Agent) && !Agent.SearchAmongExploredLocations(Location) && Location.CheckEvidence();
        }

        public override void ApplyEffects(WorldBeliefs state)
        {
            Agent.AddEvidence(Killer);
            Agent.GetBeliefs().GetAgentByName(Killer.GetName()).AssignRole("killer");
            Agent.SetObjectOfAngry(Killer);
            Agent.AddExploredLocation(Location);
        }

        public static void GetPossibleActions(Agent agent, List<PlanAction> result)
        {
            // Based on agent beliefs, generate possible actions that are valid in its world and output to result

            if (agent.GetStatus())
            {
                Move move = new Move();
                result.Add(move);

                NothingToDo nothingToDo = new NothingToDo();
                result.Add(nothingToDo);

                if (agent.ThinksThatSomeoneIsAngry())
                {
                    Reassure reassure = new Reassure();
                    result.Add(reassure);
                }

                if (agent.CheckScared())
                {
                    Run run = new Run();
                    result.Add(run);
                }

                if (agent.GetRole() == "killer")
                {
                    Kill kill = new Kill();
                    result.Add(kill);

                    Entrap entrap = new Entrap();
                    result.Add(entrap);

                    TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
                    result.Add(tellAboutASuspicious);
                }

                if (agent.GetRole() == "usual")
                {
                    if (agent.GetObjectOfAngry().AngryCheck())
                    {
                        Fight fight = new Fight();
                        result.Add(fight);

                        if (agent.GetEvidenceStatus().CheckEvidence())
                        {
                            NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
                            result.Add(neutralizeKiller);
                        }
                    }

                    if (!agent.SearchAmongExploredLocations(agent.GetBeliefs().GetStaticWorldPart().SearchAgentAmongLocations(agent)))
                    {
                        InvestigateRoom investigateRoom = new InvestigateRoom();
                        result.Add(investigateRoom);
                    }
                }
            }
        }
    }
}
