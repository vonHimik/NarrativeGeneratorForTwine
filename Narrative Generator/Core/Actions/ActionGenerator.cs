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
        /// <summary>
        /// A pre-created class instance.
        /// </summary>
        public static readonly ActionGenerator INSTANCE = new ActionGenerator();

        /// <summary>
        /// A method that returns all valid NOW actions for the agent, given the context.
        /// </summary>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <returns>List of valid actions.</returns>
        public List<PlanAction> GetAvailableActions (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentState)
        {
            // We initialize a variable that stores a list of valid actions. We will add actions that meet the conditions to it.
            List<PlanAction> result = new List<PlanAction>();

            Talk talk = new Talk(currentState);
            ToBeAWitness toBeAWitness = new ToBeAWitness(currentState);
            Reassure reassure = new Reassure(currentState);
            Run run = new Run(currentState);
            Kill kill = new Kill(currentState);
            Entrap entrap = new Entrap(currentState);
            TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious(currentState);
            NothingToDo nothingToDo = new NothingToDo(currentState);
            Move move = new Move(currentState);
            NeutralizeKiller neutralizeKiller = new NeutralizeKiller(currentState);
            Fight fight = new Fight(currentState);
            InvestigateRoom investigateRoom = new InvestigateRoom(currentState);
            HelpElfs helpElfs = new HelpElfs(currentState);
            HelpLordHarrowmont helpLordHarrowmont = new HelpLordHarrowmont(currentState);
            HelpMages helpMages = new HelpMages(currentState);
            HelpPrinceBelen helpPrinceBelen = new HelpPrinceBelen(currentState);
            HelpTemplars helpTemplars = new HelpTemplars(currentState);
            HelpWerewolves helpWerewolves = new HelpWerewolves(currentState);
            CompleteQuest completeQuest = new CompleteQuest(currentState);

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
            if (completeQuest.PreCheckPrecondition(currentState, agent)) { result.Add(completeQuest); }

            return result;
        }
    }
}