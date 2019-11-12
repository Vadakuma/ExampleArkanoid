using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{

    public class LoseState : BaseGameState
    {
        public LoseState(IGameState prev) : base(prev) { ResetResult(); }
        protected override void SetStateName() { StateName = "InLoseState"; }

        // drop all score
        private void ResetResult()
        {
            GameData.ResetScore();
        }
    }



}
