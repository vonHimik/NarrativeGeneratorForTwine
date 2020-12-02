using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Goal
    {
        // Boolean variables define the type of target(s) of the agent. 
        // While checking whether the target state has been reached, 
        // only those sections will be compared between the current world state and the target world state 
        // that correspond to the Boolean variables set to True.

        public bool goalTypeLocation;
        public bool goalTypeStatus;
        public bool goalTypePossession;
        public World goalState;

        public Goal (bool goalTypeLocation, bool goalTypeStatus, bool goalTypePossession, World goalState)
        {
            this.goalTypeLocation = goalTypeLocation;
            this.goalTypeStatus = goalTypeStatus;
            this.goalTypePossession = goalTypePossession;
            this.goalState = goalState;
        }
    }
}
