using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    public class PauseState : BaseGameState
    {
        public PauseState(IGameState prev) : base(prev) { }
        protected override void SetStateName() { StateName = "InPauseState"; }
    }

}
