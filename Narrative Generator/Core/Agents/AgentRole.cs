using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// Enumerator for agent roles.
    /// </summary>
    public enum AgentRole
    {
        /// <summary>
        /// Value to indicate the role of an ordinary agent.
        /// </summary>
        USUAL,
        /// <summary>
        /// Value to indicate the role of the story's antagonist.
        /// </summary>
        ANTAGONIST,
        /// <summary>
        /// Value to indicate the player's role.
        /// </summary>
        PLAYER,
        /// <summary>
        /// Value to indicate the role of a ordinary enemy.
        /// </summary>
        ENEMY
    }

    /// <summary>
    /// A class that facilitates interaction with roles and their use.
    /// </summary>
    public static class AgentRoleUtils
    {
        /// <summary>
        /// A method that returns the name of the specified role.
        /// </summary>
        /// <param name="role">The role whose name is the desired to get.</param>
        /// <returns>Role name.</returns>
        public static string GetName (AgentRole role)
        {
            return Enum.GetName(typeof(AgentRole), role);
        }

        /// <summary>
        /// A method that returns a role that matches the passed name.
        /// </summary>
        /// <param name="name">The name of the role to get.</param>
        /// <returns>The value of the role that matches the passed name.</returns>
        public static AgentRole GetEnum (string name)
        {
            if (name == "usual") return AgentRole.USUAL;
            if (name == "killer" || name == "antagonist") return AgentRole.ANTAGONIST;
            if (name == "player") return AgentRole.PLAYER;
            if (name == "boss" || name == "enemy") return AgentRole.ENEMY;
            throw new Exception("UNRECOGNIZED AGENT ROLE: " + name);
        }
    }
}
