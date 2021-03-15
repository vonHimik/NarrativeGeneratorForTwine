using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class Goal : ICloneable
    {
        // Boolean variables define the type of target(s) of the agent. 
        // While checking whether the target state has been reached, 
        // only those sections will be compared between the current world state and the target world state 
        // that correspond to the Boolean variables set to True.

        public bool goalTypeIsLocation;
        public bool goalTypeIsStatus;
        public bool goalTypeIsPossession;
        private WorldDynamic goalState;

        public Goal()
        {
            goalTypeIsLocation = false;
            goalTypeIsStatus = false;
            goalTypeIsPossession = false;
            goalState = new WorldDynamic();
        }

        public Goal (bool goalTypeLocation, bool goalTypeStatus, bool goalTypePossession, WorldDynamic goalState)
        {
            this.goalTypeIsLocation = goalTypeLocation;
            this.goalTypeIsStatus = goalTypeStatus;
            this.goalTypeIsPossession = goalTypePossession;
            this.goalState = goalState;
        }

        public object Clone()
        {
            var clone = new Goal();

            clone.goalTypeIsLocation = goalTypeIsLocation;
            clone.goalTypeIsStatus = goalTypeIsStatus;
            clone.goalTypeIsPossession = goalTypeIsPossession;
            clone.goalState = (WorldDynamic)goalState.Clone();

            return clone;
        }

        public WorldDynamic GetGoalState()
        {
            return goalState;
        }
    }
}
