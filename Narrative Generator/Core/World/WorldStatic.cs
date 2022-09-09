using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the static part (rarely changed) of the state of the storyworld, which stores information about the locations and settings in it.
    /// </summary>
    [Serializable]
    public class WorldStatic : ICloneable, IEquatable<WorldStatic>
    {
        // List of locations
        private HashSet<LocationStatic> locations;

        // Properties
        private int turn;
        private Setting setting;

        // Settings
        private bool locationsAreConnected;
        private bool randomBattlesResults;
        private bool uniqWaysToKill;
        private bool strOrdVicSec;
        private bool eachAgentsHasUG;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public WorldStatic()
        {
            locations = new HashSet<LocationStatic>();
            turn = 0;
            setting = Setting.DefaultDemo;
            locationsAreConnected = false;
            randomBattlesResults = false;
            uniqWaysToKill = false;
            strOrdVicSec = false;
            eachAgentsHasUG = false;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the WorldStatic, which creates a new instance of the WorldStatic based on the passed clone.
        /// </summary>
        /// <param name="clone">A WorldStatic instance that will serve as the basis for creating a new instance.</param>
        public WorldStatic (WorldStatic clone)
        {
            locations = new HashSet<LocationStatic>(clone.locations);
            turn = clone.turn;
            setting = clone.setting;
            locationsAreConnected = clone.locationsAreConnected;
            randomBattlesResults = clone.randomBattlesResults;
            uniqWaysToKill = clone.uniqWaysToKill;
            strOrdVicSec = clone.strOrdVicSec;
            eachAgentsHasUG = clone.eachAgentsHasUG;
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// Method for cloning an WorldStatic instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new WorldStatic();

            clone.locations = new HashSet<LocationStatic>(locations);
            clone.turn = turn;
            clone.setting = setting;
            clone.locationsAreConnected = locationsAreConnected;
            clone.randomBattlesResults = randomBattlesResults;
            clone.uniqWaysToKill = uniqWaysToKill;
            clone.strOrdVicSec = strOrdVicSec;
            clone.eachAgentsHasUG = eachAgentsHasUG;

            return clone;
        }

        /// <summary>
        /// Adds the given locations to the list of locations.
        /// </summary>
        /// <param name="locations">Locations to be added.</param>
        public void AddLocations (List<LocationStatic> locations)
        {
            foreach (var location in locations) { AddLocation(location); }
            UpdateHashCode();
        }

        /// <summary>
        /// Adds the specified location to the list of locations.
        /// </summary>
        /// <param name="newLocation">Information about the added location.</param>
        public void AddLocation (LocationStatic newLocation)
        {
            locations.Add(newLocation);
            UpdateHashCode();
        }

        /// <summary>
        /// Returns a list of locations.
        /// </summary>
        /// <returns>List of locations.</returns>
        public HashSet<LocationStatic> GetLocations() { return locations; }

        /// <summary>
        /// Iterates through the list of locations and returns the one whose name matches the given name, up to the first match.
        /// </summary>
        /// <param name="name">The name of the required location.</param>
        /// <returns>Required location.</returns>
        public LocationStatic GetLocation (string name)
        {
            foreach (var location in locations)
            {
                if (location.GetName() == name) { return location; }
            }

            throw new MissingMemberException();
        }

        /// <summary>
        /// Returns the turn number.
        /// </summary>
        /// <returns>The numeric value of the turn number.</returns>
        public int GetTurnNumber() { return turn; }

        /// <summary>
        /// Increases the move turn by one.
        /// </summary>
        public void IncreaseTurnNumber() { turn++; }

        /// <summary>
        /// Sets the specified setting.
        /// </summary>
        /// <param name="setting">The setting to be set.</param>
        public void SetSetting (Setting setting) { this.setting = setting; }

        /// <summary>
        /// Returns information about the current setting.
        /// </summary>
        /// <returns>Current setting.</returns>
        public Setting GetSetting() { return setting; }

        /// <summary>
        /// Enables the rule about the presence of paths (connections) between locations.
        /// </summary>
        public void ConnectionOn() { locationsAreConnected = true; }

        /// <summary>
        /// Disables the rule about the presence of paths (connections) between locations.
        /// </summary>
        public void ConnectionOff() { locationsAreConnected = false; }

        /// <summary>
        /// Returns information about whether the rule about the presence of paths (connections) between locations is enabled or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetConnectionStatus() { return locationsAreConnected; }

        /// <summary>
        /// Enables the rule about random battle results.
        /// </summary>
        public void RandomBattlesResultsOn() { randomBattlesResults = true; }

        /// <summary>
        /// Disables the rule about random battle results.
        /// </summary>
        public void RandomBattlesResultsOff() { randomBattlesResults = false; }

        /// <summary>
        /// Returns information about whether the rule about random battle results is enabled or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetRandomBattlesResultsStatus() { return randomBattlesResults; }

        /// <summary>
        /// Enables a rule to ensure that each agent's kills by the antagonist have unique descriptions.
        /// </summary>
        public void UniqWaysToKillOn() { uniqWaysToKill = true; }

        /// <summary>
        /// Disables the rule for killing each agent by the antagonist to have unique descriptions.
        /// </summary>
        public void UniqWaysToKillOff() { uniqWaysToKill = false; }

        /// <summary>
        /// Returns whether the unique kill descriptions rule is enabled or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetUniqWaysToKillStatus() { return uniqWaysToKill; }

        /// <summary>
        /// Enables the rule that the antagonist must kill agents in a specific order.
        /// </summary>
        public void StrictOrderOfVictimSelectionOn() { strOrdVicSec = true; }

        /// <summary>
        /// Disables the rule that the antagonist must kill agents in a specific order.
        /// </summary>
        public void StrictOrderOfVictimSelectionOff() { strOrdVicSec = false; }

        /// <summary>
        /// Returns whether the rule that the antagonist must kill agents in a specific order is enabled or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetStrictOrderOfVictimSelection() { return strOrdVicSec; }

        /// <summary>
        /// Enables the rule that each agent has its own unique goals.
        /// </summary>
        public void AgentsHasUniqGoalsOn() { eachAgentsHasUG = true; }

        /// <summary>
        /// Disables the rule that each agent has its own unique goals.
        /// </summary>
        public void AgentsHasUniqGoalsOff() { eachAgentsHasUG = false; }

        /// <summary>
        /// Returns whether the rule that each agent has its own unique targets is enabled or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetAgentsHasUniqGoals() { return eachAgentsHasUG; }

        /// <summary>
        /// Method for comparing two WorldStatic instance.
        /// </summary>
        /// <param name="anotherWorld">Another WorldStatic instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (WorldStatic anotherWorld)
        {
            if (anotherWorld == null) { return false; }

            bool locationsEquals = true;
            bool locationsReferenceEquals = true;

            for (int i = 0; i < locations.Count; i++)
            {
                if (!locations.ElementAt(i).Equals(anotherWorld.locations.ElementAt(i)))
                {
                    locationsEquals = false;
                }
                if (!object.ReferenceEquals(locations.ElementAt(i), anotherWorld.locations.ElementAt(i)))
                {
                    locationsReferenceEquals = false;
                }
            }

            bool turnEquals = turn.Equals(anotherWorld.GetTurnNumber());
            bool turnReferenceEquals = object.ReferenceEquals(turn, anotherWorld.turn);

            bool settingEquals = setting.Equals(anotherWorld.setting);
            bool settingReferenceEquals = object.ReferenceEquals(setting, anotherWorld.setting);

            bool locationsGlobal = locationsReferenceEquals || locationsEquals;
            bool turnGlobal = turnEquals || turnReferenceEquals;

            bool equal = locationsGlobal /*&& turnGlobal*/;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the WorldStatic.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            foreach (var location in locations)
            {
                hashcode = hashcode * 42 + location.GetHashCode();
            }

            hashcode = hashcode * 42 + setting.GetHashCode();

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