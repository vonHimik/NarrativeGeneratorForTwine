using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class Goal : ICloneable, IEquatable<Goal>
    {
        // Boolean variables define the type of target(s) of the agent. 
        // While checking whether the target state has been reached, 
        // only those sections will be compared between the current world state and the target world state 
        // that correspond to the Boolean variables set to True.

        public bool goalTypeIsLocation;
        public bool goalTypeIsStatus;
        public bool goalTypeIsPossession;
        private WorldDynamic goalState;

        private bool hasHashCode;
        private int hashCode;

        public Goal()
        {
            goalTypeIsLocation = false;
            goalTypeIsStatus = false;
            goalTypeIsPossession = false;
            goalState = new WorldDynamic();
            hasHashCode = false;
            hashCode = 0;
        }

        public Goal (Goal clone)
        {
            goalTypeIsLocation = clone.goalTypeIsLocation;
            goalTypeIsStatus = clone.goalTypeIsStatus;
            goalTypeIsPossession = clone.goalTypeIsPossession;
            goalState = (WorldDynamic)clone.goalState.Clone();
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public Goal (bool goalTypeLocation, bool goalTypeStatus, bool goalTypePossession, WorldDynamic goalState)
        {
            this.goalTypeIsLocation = goalTypeLocation;
            this.goalTypeIsStatus = goalTypeStatus;
            this.goalTypeIsPossession = goalTypePossession;
            this.goalState = goalState;
            hasHashCode = false;
            hashCode = 0;
        }

        public object Clone()
        {
            var clone = new Goal();

            clone.goalTypeIsLocation = goalTypeIsLocation;
            clone.goalTypeIsStatus = goalTypeIsStatus;
            clone.goalTypeIsPossession = goalTypeIsPossession;
            clone.goalState = new WorldDynamic(goalState);
            //goalState.CloneAgents(clone.goalState.GetAgents());

            return clone;
        }

        public WorldDynamic GetGoalState() { return goalState; }

        public bool Equals(Goal anotherGoal)
        {
            if (anotherGoal == null) { return false; }

            bool goalTypeIsLocationEquals = (goalTypeIsLocation == anotherGoal.goalTypeIsLocation);
            bool goalTypeIsLocationReferenceEquals = object.ReferenceEquals(goalTypeIsLocation, anotherGoal.goalTypeIsLocation);

            bool goalTypeIsStatusEquals = (goalTypeIsStatus == anotherGoal.goalTypeIsStatus);
            bool goalTypeIsStatusReferenceEquals = object.ReferenceEquals(goalTypeIsStatus, anotherGoal.goalTypeIsStatus);

            bool goalTypeIsPossessionEquals = (goalTypeIsPossession == anotherGoal.goalTypeIsPossession);
            bool goalTypeIsPossessionReferenceEquals = object.ReferenceEquals(goalTypeIsPossession, anotherGoal.goalTypeIsPossession);

            bool goalStateEquals = goalState.Equals(anotherGoal.goalState);
            bool goalStateReferenceEquals = object.ReferenceEquals(goalState, anotherGoal.goalState);

            bool goalTypeIsLocationGlobal = goalTypeIsLocationEquals || goalTypeIsLocationReferenceEquals;
            bool goalTypeIsStatusGlobal = goalTypeIsStatusEquals || goalTypeIsStatusReferenceEquals;
            bool goalTypeIsPossessionGlobal = goalTypeIsPossessionEquals || goalTypeIsPossessionReferenceEquals;
            bool goalStateGlobal = goalStateEquals || goalStateReferenceEquals;

            bool equal = goalTypeIsLocationGlobal && goalTypeIsStatusGlobal && goalTypeIsPossessionGlobal && goalStateGlobal;

            return equal;
        }

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + goalTypeIsLocation.GetHashCode();
            hashcode = hashcode * 42 + goalTypeIsPossession.GetHashCode();
            hashcode = hashcode * 42 + goalTypeIsStatus.GetHashCode();
            hashcode = hashcode * 42 + goalState.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }
    }
}
