using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that stores static (rarely changed) agent properties and methods for interacting with them.
    /// </summary>
    public class AgentStateStatic : ICloneable
    {
        // Agent properties.
        private string name;
        private AgentRole role;

        /// <summary>
        /// Method-constructor of the static part of the agent (without input parameters).
        /// </summary>
        public AgentStateStatic()
        {
            name = "";
            role = AgentRole.USUAL;
        }

        /// <summary>
        /// Method-constructor of the static part of the agent (with input parameters).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="role"></param>
        public AgentStateStatic(string name, AgentRole role)
        {
            // We assign the specified name and role.
            SetName(name);
            AssignRole(role);
        }

        /// <summary>
        /// Method for cloning an agent.
        /// </summary>
        public object Clone()
        {
            // Create an empty clone.
            var clone = new AgentStateStatic();

            // Assign a name and role to it.
            clone.name = name;
            clone.role = role;

            // We return the clone.
            return clone;
        }

        /// <summary>
        /// A method that assigns the name of the agent.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// A method that returns the name of the agent.
        /// </summary>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// A method that assigns a role to an agent.
        /// </summary>
        /// <param name="role"></param>
        public void AssignRole(AgentRole role)
        {
            this.role = role;
        }

        /// <summary>
        /// A method that assigns a role to an agent.
        /// </summary>
        /// <param name="role"></param>
        public void AssignRole(string role)
        {
            this.role = AgentRoleUtils.GetEnum(role);
        }

        /// <summary>
        /// A method that returns the role of the agent.
        /// </summary>
        public AgentRole GetRole()
        {
            return role;
        }
    }
}
