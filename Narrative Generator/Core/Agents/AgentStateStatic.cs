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
    public class AgentStateStatic : IEquatable<AgentStateStatic>, ICloneable
    {
        // Agent properties
        /// <summary>
        /// Agent name (must be unique).
        /// </summary>
        private string name;
        /// <summary>
        /// The role of the agent.
        /// </summary>
        private AgentRole role;

        // Hashcode
        /// <summary>
        /// An indicator of whether a hashcode has been generated for this component.
        /// </summary>
        private bool hasHashCode;
        /// <summary>
        /// The hashcode of this component.
        /// </summary>
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

        /// <summary>
        /// The conditional constructor of the static part of the agent, which creates a new instance of the agent based on the passed clone.
        /// </summary>
        /// <param name="clone">An agent instance that will serve as the basis for creating a new instance.</param>
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
        /// <param name="name">The name of the agent to construct.</param>
        /// <param name="role">The role of the constructed agent.</param>
        public AgentStateStatic (string name, AgentRole role)
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
        /// <returns>A new instance that is a copy of the current one.</returns>
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
        /// <param name="name">New name of the agent.</param>
        public void SetName (string name)
        {
            this.name = name;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that returns the name of the agent.
        /// </summary>
        public string GetName() { return name; }

        /// <summary>
        /// A method that assigns a role to an agent.
        /// </summary>
        /// <param name="role">New role of the agent.</param>
        public void AssignRole (AgentRole role)
        {
            this.role = role;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that assigns a role to an agent.
        /// </summary>
        /// <param name="role">Name of new role of the agent.</param>
        public void AssignRole (string role)
        {
            this.role = AgentRoleUtils.GetEnum(role);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that returns the role of the agent.
        /// </summary>
        public AgentRole GetRole() { return role; }

        /// <summary>
        /// Method for comparing two static parts of agent states.
        /// </summary>
        /// <param name="anotherState">Another static state of the agent, for comparison.</param>
        /// <returns>True if both states are the same, false otherwise.</returns>
        public bool Equals (AgentStateStatic anotherState)
        {
            if (anotherState == null) { return false; }

            bool nameEquals = (name == anotherState.name);
            bool nameReferenceEquals = object.ReferenceEquals(name, anotherState.name);

            bool roleEquals = (role == anotherState.role);
            bool roleReferenceEquals = object.ReferenceEquals(role, anotherState.role);

            bool nameGlobal = nameEquals || nameReferenceEquals;
            bool roleGlobal = roleEquals || roleReferenceEquals;

            bool equal = nameGlobal && roleGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the static part of the agent's state.
        /// </summary>
        /// <returns>Hash code.</returns>
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

        /// <summary>
        /// Clears the current hash code value.
        /// </summary>
        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Updates (refresh) the current hash code value.
        /// </summary>
        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
