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
    [Serializable]
    public class AgentStateStatic : /*IEquatable<AgentStateStatic>,*/ ICloneable
    {
        // Agent properties.
        private string name;
        private AgentRole role;

        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Method-constructor of the static part of the agent (without input parameters).
        /// </summary>
        public AgentStateStatic()
        {
            name = "";
            role = AgentRole.USUAL;
            hasHashCode = false;
            hashCode = 0;
        }

        public AgentStateStatic (AgentStateStatic clone)
        {
            name = clone.name;
            role = clone.role;
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
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
            hasHashCode = false;
            hashCode = 0;
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
            UpdateHashCode();
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
            UpdateHashCode();
        }

        /// <summary>
        /// A method that assigns a role to an agent.
        /// </summary>
        /// <param name="role"></param>
        public void AssignRole(string role)
        {
            this.role = AgentRoleUtils.GetEnum(role);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that returns the role of the agent.
        /// </summary>
        public AgentRole GetRole()
        {
            return role;
        }

        /*public bool Equals(AgentStateStatic other)
        {
            throw new NotImplementedException();
        }*/

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + name.GetHashCode();
            hashcode = hashcode * 42 + role.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
