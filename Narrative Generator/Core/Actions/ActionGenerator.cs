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

            // Returning a list of valid actions.
            return result;
        }
    }
}