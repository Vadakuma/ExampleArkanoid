using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    public class ReturnToMenuState : BaseGameState
    {
        public ReturnToMenuState(IGameState prev) : base(prev) { }
        protected override void SetStateName() { StateName = "InReturnToMenuState"; }
    }
}
