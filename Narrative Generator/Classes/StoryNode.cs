using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode
    {
        World worldState;  // Story state.
        bool activePlayer; // Active player = true, active agent = false.
        Agent ActiveAgent; // If the player does not move, then one of the agents move - which?
    }
}
