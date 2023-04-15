using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the extraction from the planner and storage of the action plan for the agent.
    /// </summary>
    [Serializable]
    public class Plan : ICloneable
    {
        /// <summary>
        /// List of actions in the plan.
        /// </summary>
        private List<PlanAction> actions;

        /// <summary>
        /// Indicator of the success of the plan finding process. True if the plan is found, false otherwise.
        /// </summary>
        public bool planReceived;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public Plan()
        {
            actions = new List<PlanAction>();
            planReceived = false;
        }

        /// <summary>
        /// Constructor with parameters of the Plan, which creates a new instance of the Plan based on the passed clone.
        /// </summary>
        /// <param name="clone">A clone of a plan instance that will be copied to create a new instance.</param>
        public Plan (Plan clone)
        {
            actions = new List<PlanAction>(clone.actions);
            planReceived = clone.planReceived;
        }

        /// <summary>
        /// Method for creating a plan clone instance.
        /// </summary>
        /// <returns>Plan clone object.</returns>
        public object Clone()
        {
            var clone = new Plan();
            clone.actions = new List<PlanAction>(actions);
            clone.planReceived = planReceived;
            return clone;
        }

        /// <summary>
        /// Clears the plan of actions stored in it and switches the instance to the "plan not found" state.
        /// </summary>
        public void Clear()
        {
            actions.Clear();
            planReceived = false;
        }

        /// <summary>
        /// The method that adds the required action to the plan.
        /// </summary>
        /// <param name="actionName">The name of the action readed from the planner.</param>
        /// <param name="parameters">Action parameters readed from the planner.</param>
        /// <param name="state">The current state of the storyworld.</param>
        public void AddAction (string actionName, List<string> parameters, WorldDynamic state)
        {
            if (actionName.Contains("move"))
            {
                Move move = new Move(state);
                
                foreach (var parameter in parameters)
                {
                    move.Arguments.Add(parameter);
                }

                actions.Add(move);
            }
            if (actionName.Contains("kill") && !actionName.Contains("move") && !actionName.Contains("neutralize"))
            {
                Kill kill = new Kill(state);
                actions.Add(kill);
            }
            if (actionName == "entrap")
            {
                Entrap entrap = new Entrap(state);
                actions.Add(entrap);
            }
            if (actionName.Contains("fight"))
            {
                Fight fight = new Fight(state);
                actions.Add(fight);
            }
            if (actionName == "investigate-room")
            {
                InvestigateRoom investigateRoom = new InvestigateRoom(state);
                actions.Add(investigateRoom);
            }
            if (actionName == "neutralize-killer" || actionName == "neutralizekiller")
            {
                NeutralizeKiller neutralizeKiller = new NeutralizeKiller(state);
                actions.Add(neutralizeKiller);
            }
            if (actionName == "nothing-to-do")
            {
                NothingToDo nothingToDo = new NothingToDo(state);
                actions.Add(nothingToDo);
            }
            if (actionName == "reassure")
            {
                Reassure reassure = new Reassure(state);
                actions.Add(reassure);
            }
            if (actionName == "run")
            {
                Run run = new Run(state);
                actions.Add(run);
            }
            if (actionName == "tell-about-killer")
            {
                TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious(state);
                actions.Add(tellAboutASuspicious);
            }
            if (actionName == "talk")
            {
                Talk talk = new Talk(state);
                actions.Add(talk);
            }
            if (actionName == "to-be-a-witness")
            {
                ToBeAWitness toBeAWitness = new ToBeAWitness(state);
                actions.Add(toBeAWitness);
            }
            if (actionName == "help-mages")
            {
                HelpMages helpMages = new HelpMages(state);
                actions.Add(helpMages);
            }
            if (actionName == "help-templars")
            {
                HelpTemplars helpTemplars = new HelpTemplars(state);
                actions.Add(helpTemplars);
            }
            if (actionName == "help-elfs")
            {
                HelpElfs helpElfs = new HelpElfs(state);
                actions.Add(helpElfs);
            }
            if (actionName == "help-werewolves")
            {
                HelpWerewolves helpWerewolves = new HelpWerewolves(state);
                actions.Add(helpWerewolves);
            }
            if (actionName == "help-prinec")
            {
                HelpPrinceBelen helpPrinceBelen = new HelpPrinceBelen(state);
                actions.Add(helpPrinceBelen);
            }
            if (actionName == "help-lord")
            {
                HelpLordHarrowmont helpLordHarrowmont = new HelpLordHarrowmont(state);
                actions.Add(helpLordHarrowmont);
            }
            if (actionName == "complete-quest")
            {
                CompleteQuest completeQuest = new CompleteQuest(state);
                actions.Add(completeQuest);
            }
        }

        /// <summary>
        /// A method that returns the specified action from the plan.
        /// </summary>
        /// <param name="index">The index under which the action is stored in the plan.</param>
        /// <returns>Information about the requested action.</returns>
        public PlanAction GetAction (int index)
        {
            return actions[index];
        }
    }
}
