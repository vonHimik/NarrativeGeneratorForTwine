using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode
    {
        private World worldState;  // Story state.
        private bool activePlayer; // Active player = true, active agent = false.
        private Agent activeAgent; // If the player does not move, then one of the agents move - which?

        public void SetWorldState(World worldState)
        {
            this.worldState = worldState;
        }

        public World GetWorldState()
        {
            return worldState;
        }

        public void SetActivePlayer (bool activePlayer)
        {
            this.activePlayer = activePlayer;
        }

        public bool GetActivePlayer()
        {
            return activePlayer;
        }

        public void SetActiveAgent(Agent activeAgent)
        {
            this.activeAgent = activeAgent;
        }

        public Agent GetActiveAgent()
        {
            return activeAgent;
        }
    }
}
