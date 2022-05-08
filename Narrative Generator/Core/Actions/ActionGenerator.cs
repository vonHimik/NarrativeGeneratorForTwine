using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that generates a list of possible actions for the agent at the moment.
    /// </summary>
    public class ActionGenerator
    {
        public static readonly ActionGenerator INSTANCE = new ActionGenerator();

        /// <summary>
        /// A method that returns all valid NOW actions for the agent, given the context.
        /// </summary>
        public List<PlanAction> GetAvailableActions (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentState)
        {
            // We initialize a variable that stores a list of valid actions. We will add actions that meet the conditions to it.
            List<PlanAction> result = new List<PlanAction>();

            Talk talk = new Talk();
            ToBeAWitness toBeAWitness = new ToBeAWitness();
            Reassure reassure = new Reassure();
            Run run = new Run();
            Kill kill = new Kill();
            Entrap entrap = new Entrap();
            TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
            NothingToDo nothingToDo = new NothingToDo();
            Move move = new Move();
            NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
            Fight fight = new Fight();
            InvestigateRoom investigateRoom = new InvestigateRoom();
            HelpElfs helpElfs = new HelpElfs();
            HelpLordHarrowmont helpLordHarrowmont = new HelpLordHarrowmont();
            HelpMages helpMages = new HelpMages();
            HelpPrinceBelen helpPrinceBelen = new HelpPrinceBelen();
            HelpTemplars helpTemplars = new HelpTemplars();
            HelpWerewolves helpWerewolves = new HelpWerewolves();

<<<<<<< Updated upstream
            if (talk.PreCheckPrecondition(currentState, agent)) { result.Add(talk); }
            if (toBeAWitness.PreCheckPrecondition(currentState, agent)) { result.Add(toBeAWitness); }
            if (reassure.PreCheckPrecondition(currentState, agent)) { result.Add(reassure); }
            if (run.PreCheckPrecondition(currentState, agent)) { result.Add(run); }
            if (kill.PreCheckPrecondition(currentState, agent)) { result.Add(kill); }
            if (entrap.PreCheckPrecondition(currentState, agent)) { result.Add(entrap); }
            if (tellAboutASuspicious.PreCheckPrecondition(currentState, agent)) { result.Add(tellAboutASuspicious); }
            if (nothingToDo.PreCheckPrecondition(currentState, agent)) { result.Add(nothingToDo); }
            if (move.PreCheckPrecondition(currentState, agent)) { result.Add(move); }
            if (neutralizeKiller.PreCheckPrecondition(currentState, agent)) { result.Add(neutralizeKiller); }
            if (fight.PreCheckPrecondition(currentState, agent)) { result.Add(fight); }
            if (investigateRoom.PreCheckPrecondition(currentState, agent)) { result.Add(investigateRoom); }
            if (helpElfs.PreCheckPrecondition(currentState, agent)) { result.Add(helpElfs); }
            if (helpLordHarrowmont.PreCheckPrecondition(currentState, agent)) { result.Add(helpLordHarrowmont); }
            if (helpMages.PreCheckPrecondition(currentState, agent)) { result.Add(helpMages);  }
            if (helpPrinceBelen.PreCheckPrecondition(currentState, agent)) { result.Add(helpPrinceBelen); }
            if (helpTemplars.PreCheckPrecondition(currentState, agent)) { result.Add(helpTemplars); }
            if (helpWerewolves.PreCheckPrecondition(currentState, agent)) { result.Add(helpWerewolves); }
=======
                // If the agent thinks that one of the other agents is angry, then he can try to calm him down.
                if (agent.Value.ThinksThatSomeoneIsAngry())
                {
                    Reassure reassure = new Reassure();
                    result.Add(reassure);
                }

                // If the agent is scared, then he can start running away.
                if (agent.Value.CheckScared())
                {
                    Run run = new Run();
                    result.Add(run);
                }

                // If the role of the agent is a killer.
                if (agent.Key.GetRole() == AgentRole.KILLER)
                {
                    // Then he can kill.
                    Kill kill = new Kill();
                    result.Add(kill);

                    // He can also lure into a trap (being in a place where there is no one).
                    Entrap entrap = new Entrap();
                    result.Add(entrap);

                    // Or lure into a trap in another way (telling another agent that a certain place is suspicious, forcing him to be sent there).
                    TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
                    result.Add(tellAboutASuspicious);
                }

                if (agent.Key.GetRole() == AgentRole.USUAL)
                {
                    // If he is angry, then he can start fighting.
                    if (agent.Value.GetObjectOfAngry().AngryCheck())
                    {
                        Fight fight = new Fight();
                        result.Add(fight);
                    }

                    // And he can do nothing.
                    NothingToDo nothingToDo = new NothingToDo();
                    result.Add(nothingToDo);
                }

                // If the agent role is usual or player.
                if (agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER)
                {
                    // If he is angry, then he can start fighting.
                    if (agent.Value.GetObjectOfAngry().AngryCheck())
                    {
                        // And if he has evidence against the killer, he can try to neutralize him.
                        if (agent.Value.GetEvidenceStatus().CheckEvidence())
                        {
                            NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
                            result.Add(neutralizeKiller);
                        }
                    }

                    if (!agent.Value.CheckIfLocationIsExplored(agent.Value.GetMyLocation()))
                    {
                        // Can look for evidence against the killer in the room where he is.
                        InvestigateRoom investigateRoom = new InvestigateRoom();
                        result.Add(investigateRoom);
                    }
                }

                // Then he can move.
                Move move = new Move();
                result.Add(move);
            }
>>>>>>> Stashed changes

            // Returning a list of valid actions.
            return result;
        }
    }
}