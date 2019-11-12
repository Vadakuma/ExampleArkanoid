using Arkanoid.LevelGenerator;
using Arkanoid.PlayerPlatform;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    public class GenerateLevelState : BaseGameState
    {
        private int status = -1;

        public GenerateLevelState(IGameState prev) : base(prev)
        {
            // trying to generate level and waiting of reaction from user
            status = TryToGenerateLevel();
            // make shure that platform in the good conditions
            Platform.Instance.GoToResetState();
            // drop score about last try
            ResetResult();
        }
        protected override void SetStateName() { StateName = "InGenerateLevelState"; }

        /// <summary>
        /// Go to waiting state for if success
        /// </summary>
        private static void OnLevelGeneratedSuccess()
        {
            GameState.Instance.GoToWaitState();
        }

        /// <summary>
        ///  Go to waiting state for if fail
        /// </summary>
        private static void OnLevelGeneratedFail()
        {
            GameState.Instance.GoToWaitState();
        }

        public override void Update()
        {
            // should wait for a while when the constructor work wiil be done
            if (status > -1)
            {
                if (status == 1)
                    OnLevelGeneratedSuccess();
                else if (status == 0)
                    OnLevelGeneratedFail();
                status = -1;
            }
        }

        private static int TryToGenerateLevel()
        {
            int success = -1;
            try
            {
                Level.Instance.GenerateLevel();
                success = 1;
            }
            catch (Exception e)
            {
                Debug.Log("generate level state fail!" + e);
                success = 0;
            }

            return success;
        }

        // drop score about last try
        private void ResetResult()
        {
            GameData.ResetScore();
        }
    }
}