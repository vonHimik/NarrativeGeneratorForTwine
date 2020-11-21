using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode
    {
        World worldState;
        bool activePlayer; // Active player = true, active agent = false.
        Agent ActiveAgent;
    }
}
