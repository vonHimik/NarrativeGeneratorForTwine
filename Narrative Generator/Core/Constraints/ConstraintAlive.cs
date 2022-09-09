using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements constraints imposed by story on the lifetime of an agent.
    /// </summary>
    public class ConstraintAlive : WorldConstraint
    {
        /// <summary>
        /// If true, protects the agent from changing their status to deads, it is required to set a time limit.
        /// </summary>
        public bool temporaryInvulnerability;
        /// <summary>
        /// If true, protects the agent from changing their status to dead, without time limit.
        /// </summary>
        public bool permanentInvulnerability;
        /// <summary>
        /// If true, then an instance of this class will monitor the state of the specified agent and return false if it dies.
        /// </summary>
        public bool endIfDied;
        /// <summary>
        /// Information about the agent to which this constraint will apply.
        /// </summary>
        public AgentStateStatic targetAgent;
        /// <summary>
        /// The period for applying this restriction, in turns.
        /// </summary>
        public int termOfProtection;

        /// <summary>
        /// The conditional constructor of the given constraint (there is no unconditional constructor).
        /// </summary>
        /// <param name="temporaryInvulnerability">If true, protects the agent from changing their status to deads, it is required to set a time limit.</param>
        /// <param name="permanentInvulnerability">If true, protects the agent from changing their status to dead, without time limit.</param>
        /// <param name="endIfDied">If true, then an instance of this class will monitor the state of the specified agent and return false if it dies.</param>
        /// <param name="targetAgent">Information about the agent to which this constraint will apply.</param>
        /// <param name="termOfProtection">The period for applying this restriction, in turns.</param>
        public ConstraintAlive(bool temporaryInvulnerability, 
                               bool permanentInvulnerability, 
                               bool endIfDied,
                               AgentStateStatic targetAgent, 
                               int termOfProtection)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.endIfDied = endIfDied;
            this.targetAgent = targetAgent;
            this.termOfProtection = termOfProtection;
        }

        /// <summary>
        /// A method that changes the agent's protection duration using the given limit.
        /// </summary>
        /// <param name="newTerm">The numerical value of the new protection period, in turns.</param>
        public void ChangeTermOfProtection (int newTerm)
        {
            this.termOfProtection = newTerm;
        }

        /// <summary>
        /// Overloading a method that checks whether the conditions of the specified constraints are met.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph.</param>
        /// <param name="currentAction">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="newNode">A graph node that stores the future state.</param>
        /// <returns>Returns the result of the check.</returns>
        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode)
        {
            if (temporaryInvulnerability && !permanentInvulnerability && targetAgent != null && termOfProtection != 0)
            {
                return ((newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus() 
                        && newState.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) 
                        || newState.GetStaticWorldPart().GetTurnNumber() > termOfProtection);

            }
            else if (permanentInvulnerability && !temporaryInvulnerability && targetAgent != null)
            {
                return (newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus());
            }
            else if (endIfDied)
            {
                if (!currentState.GetAgentByName(targetAgent.GetName()).Value.GetStatus())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
