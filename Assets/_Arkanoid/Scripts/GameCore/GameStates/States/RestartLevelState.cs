using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.LevelGenerator;
using Arkanoid.PlayerPlatform;

namespace Arkanoid.GameStates
{
    /** */
    public class RestartLevelState : BaseGameState
    {
        private int status = -1;

        public RestartLevelState(IGameState prev) : base(prev)
        {
            Level.Instance.RestartLastLevel();
            Platform.Instance.GoToResetState();
            SetActiveGameActors();
            status = 0;

            // dropping score result in the last session 
            ResetResult();
        }
        protected override void SetStateName() { StateName = "InRestartLevelState"; }

        public override void Update()
        {
            // should wait for a while when the constructor work wiil be done
            if (status > -1)
            {
                if (status == 0)
                    GameState.Instance.GoToPlayState();
                status = -1;
            }
        }

        // drop score about last try
        private static void ResetResult()
        {
            GameData.ResetScore();
        }
    }

}
