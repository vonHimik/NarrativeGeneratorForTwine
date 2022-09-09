using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the agent's beliefs about whether he wants to entrap (move) some other agent to some location, which agent and where.
    /// </summary>
    [Serializable]
    public class WantToEntrap : AgentProperty, ICloneable, IEquatable<WantToEntrap>
    {
        private bool entraping;
        private AgentStateStatic whom;
        private LocationStatic where;

        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public WantToEntrap()
        {
            entraping = false;
            whom = new AgentStateStatic();
            where = new LocationStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the WantToEntrap, which creates a new instance of the WantToEntrap based on the passed clone.
        /// </summary>
        /// <param name="clone">A WantToEntrap instance that will serve as the basis for creating a new instance.</param>
        public WantToEntrap (WantToEntrap clone)
        {
            entraping = clone.entraping;
            if (clone.whom != null) { whom = (AgentStateStatic)clone.whom.Clone(); }
            else { whom = new AgentStateStatic(); }
            if (clone.where != null) { where = (LocationStatic)clone.where.Clone(); }
            else { where = new LocationStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// A constructor with parameters that takes a status indicator of the desire to entrap this agent, 
        ///    information about the agent that needs to be moved to another location, and information about the target location.
        /// </summary>
        /// <param name="entraping">Status indicator of the desire to entrap this agent.</param>
        /// <param name="whom">Information about the agent that needs to be moved to another location.</param>
        /// <param name="where">Information about the target location.</param>
        public WantToEntrap (bool entraping, AgentStateStatic whom, LocationStatic where)
        {
            this.entraping = entraping;
            this.whom = whom;
            this.where = where;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning an WantToEntrap instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new WantToEntrap();

            clone.entraping = entraping;
            if (whom != null) { clone.whom = new AgentStateStatic(whom); }
            if (where != null) { clone.where = new LocationStatic(where); }

            return clone;
        }

        /// <summary>
        /// A method that sets the entrap desire status for this agent to True.
        /// </summary>
        public void EntrapingStart()
        {
            entraping = true;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that sets the entrap desire status for this agent to False.
        /// </summary>
        public void EntrapingEnd()
        {
            entraping = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Sets the agent that this agent will want to move to some location.
        /// </summary>
        /// <param name="victim">Agent that this agent will want to move to some location.</param>
        public void SetVictim (AgentStateStatic victim)
        {
            whom = victim;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns information about the agent that this agent would like to move to some location.
        /// </summary>
        /// <returns>Information about the agent that this agent would like to move to some location.</returns>
        public AgentStateStatic GetVictim() { return whom; }

        /// <summary>
        /// Sets the target location to which this agent wants to move some other agent.
        /// </summary>
        /// <param name="location">Location to which this agent wants to move some other agent.</param>
        public void SetLocation (LocationStatic location)
        {
            where = location;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns the target location to which this agent would like to move some other agent.
        /// </summary>
        /// <returns>Location to which this agent would like to move some other agent.</returns>
        public LocationStatic GetLocation() { return where; }

        /// <summary>
        /// Sets the agent that this agent will want to move to some location AND the target location to which this agent wants to move some other agent.
        /// </summary>
        /// <param name="victim">Information about the agent that this agent would like to move to some location.</param>
        /// <param name="location">Location to which this agent would like to move some other agent.</param>
        public void SetVictimAndLocation (AgentStateStatic victim, LocationStatic location)
        {
            SetVictim(victim);
            SetLocation(location);
        }

        /// <summary>
        /// Returns the desire status of this agent to entrap (move) some other agent to some location.
        /// </summary>
        /// <returns>True if the desire to entrap (move) exists, otherwise false.</returns>
        public bool EntrapCheck() { return entraping; }

        /// <summary>
        /// Checks if this agent wants to entrap (move to another location) the specified agent.
        /// </summary>
        /// <param name="agent">Information about the checked agent.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool EntrapingCheckAtAgent (AgentStateStatic agent)
        {
            if (entraping && agent == whom) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Checks if this agent wants to entrap (move) another agent to the specified location.
        /// </summary>
        /// <param name="location">Information about the checked location.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool EntrapingCheckAtLocation (LocationStatic location)
        {
            if (entraping && location == where) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Method for comparing two WantToEntrap instance.
        /// </summary>
        /// <param name="anotherWantToEntrap">Another WantToEntrap instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals(WantToEntrap anotherWantToEntrap)
        {
            if (anotherWantToEntrap == null) { return false; }

            bool entrapingEquals = (entraping == anotherWantToEntrap.entraping);
            bool entrapingReferenceEquals = object.ReferenceEquals(entraping, anotherWantToEntrap.entraping);

            bool whomEquals;
            bool whomReferenceEquals;
            if (whom == null && anotherWantToEntrap.whom == null)
            {
                whomEquals = true;
                whomReferenceEquals = true;
            }
            else if ((whom == null && anotherWantToEntrap.whom != null) || (whom != null && anotherWantToEntrap.whom == null))
            {
                whomEquals = false;
                whomReferenceEquals = false;
            }
            else
            {
                whomEquals = whom.Equals(anotherWantToEntrap.whom);
                whomReferenceEquals = object.ReferenceEquals(whom, anotherWantToEntrap.whom);
            }

            bool whereEquals;
            bool whereReferenceEquals;
            if (where == null && anotherWantToEntrap.where == null)
            {
                whereEquals = true;
                whereReferenceEquals = true;
            }
            else if ((where == null && anotherWantToEntrap.where != null) || (where != null && anotherWantToEntrap.where == null))
            {
                whereEquals = false;
                whereReferenceEquals = false;
            }
            else
            {
                whereEquals = where.Equals(anotherWantToEntrap.where);
                whereReferenceEquals = object.ReferenceEquals(where, anotherWantToEntrap.where);
            }

            bool entrapingGlobal = entrapingEquals || entrapingReferenceEquals;
            bool whomGlobal = whomEquals || whomReferenceEquals;
            bool whereGlobal = whereEquals || whereReferenceEquals;

            bool equal = entrapingGlobal && whomGlobal && whereGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the WantToEntrap.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + entraping.GetHashCode();
            if (whom != null)
            {
                whom.ClearHashCode();
                hashcode = hashcode * 42 + whom.GetHashCode();
            }
            if (where != null)
            {
                where.ClearHashCode();
                hashcode = hashcode * 42 + where.GetHashCode();
            }

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
