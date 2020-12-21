using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Reassure : PlanAction
    {
        public Agent Agent1
        {
            get
            {
                return (Agent)Arguments[0];
            }
        }

        public Agent Agent2
        {
            get
            {
                return (Agent)Arguments[1];
            }
        }

        public Agent Agent3
        {
            get
            {
                return (Agent)Arguments[2];
            }
        }

        public Agent Killer
        {
            get
            {
                return (Agent)Arguments[3];
            }
        }

        public Location Location
        {
            get
            {
                return (Location)Arguments[4];
            }
        }

        public Reassure(params Object[] args) : base(args) { }

        public Reassure(ref Agent agent1, ref Agent agent2, ref Agent agent3, ref Agent killer, ref Location location)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(agent3);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override bool CheckPreconditions(WorldBeliefs state)
        {
            return Agent1.GetRole() == "usual" && Agent1.GetStatus() && Agent2.GetRole() == "usual" && Agent2.GetStatus()
                      && Agent3.GetRole() == "usual" && Killer.GetRole() == "killer" && Location.SearchAgent(Agent1) && Location.SearchAgent(Agent2)
                      && (Agent1.GetObjectOfAngry().AngryCheckAtAgent(Agent3) || Agent1.GetObjectOfAngry().AngryCheckAtAgent(Killer))
                      && Agent1.GetBeliefs().GetAgentByRole("killer") != Killer;
        }

        public override void ApplyEffects(WorldBeliefs state)
        {
            Agent1.CalmDown();
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
