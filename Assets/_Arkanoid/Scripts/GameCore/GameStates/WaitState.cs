using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    /** */
    public class WaitState : BaseGameState
    {
        public WaitState(IGameState prev) : base(prev) { }
        protected override void SetStateName() { StateName = "InWaitState"; }
    }
}
