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
        /// Marker to the type of constraint to apply: protects the agent from changing their status to deads, it is required to set a time limit.
        /// </summary>
        public bool temporaryInvulnerability;
        /// <summary>
        /// Marker to the type of constraint to apply: protects the agent from changing their status to dead, without time limit.
        /// </summary>
        public bool permanentInvulnerability;
        /// <summary>
        /// Marker to the type of constraint to apply: an instance of this class will monitor the state of the specified agent and return false if it dies.
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
        /// A method that checks whether the specified world state satisfies constraints.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph, which is a collection of nodes connected by oriented edges.</param>
        /// <param name="currentAction">The action currently performed by the agent, and whose influence on changing the world is being checked.</param>
        /// <param name="currentNode">A node that stores the current world state.</param>
        /// <param name="newNode">A node that stores the future world state, resulting from applying the current action on the current node.</param>
        /// <param name="succsessControl">A variable into which the overridden result (succes or fail) of an action can be written.</param>
        /// <returns>Result of the check.</returns>
        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          ref PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode,
                                          ref bool succsessControl)
        {
            if (temporaryInvulnerability && !permanentInvulnerability && targetAgent != null && termOfProtection != 0)
            {
                return ((newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus() 
                        && newState.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) 
                        || newState.GetStaticWorldPart().GetTurnNumber() > termOfProtection);

            }
            else if (permanentInvulnerability && !temporaryInvulnerability && targetAgent != null)
            {
                if (!newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus())
                {
                    currentAction.Fail(ref currentState);
                    succsessControl = false;
                }

                return true;
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
