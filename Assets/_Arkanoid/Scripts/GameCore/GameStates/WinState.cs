using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    /** */
    public class WinState : BaseGameState
    {
        public WinState(IGameState prev) : base(prev)
        {
            // See GameData.cs
            GameState.gameData.AddSessionsResult(SaveResult());
        }
        protected override void SetStateName() { StateName = "InWinState"; }


        protected PlayerData SaveResult()
        {
            return new PlayerData(GameData.SessionPlayerData.Score,
                GameData.SessionPlayerData.MaxRoundCounter++);
        }
    }

}
