using Arkanoid.Enemies;
using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.GameStates
{
    /** */
    public class PlayState : BaseGameState
    {
        private WaitForSeconds waitbrforecheck = new WaitForSeconds(0.15f);
        private bool active = false;
        private SessionCondition loseCondition;
        // cach for win conditions checking
        private GameObject projectileLink;
        private int enemiesAmount;
        private int platformHealh;

        public PlayState(IGameState prev) : base(prev)
        {
            StateName = "PlayState";

            if (Ball.Instance)
                Ball.Instance.StartMoving();

            // activate checking win conditions
            if (!GameState.Instance)
            {
                Debug.LogWarning("Problem with GameState.Instance");
                return;
            }

            if (GameState.gameData == null)
            {
                Debug.LogWarning("Problem with GameData");
                return;
            }

            loseCondition = GameState.gameData.GetLoseCondition;
            active = true;
            GameState.Instance.StartCoroutine(CheckConditions());

            SetActiveGameActors();
        }

        /// <summary>
        /// Check gameplay win conditions
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckConditions()
        {
            while (active)
            {
                yield return waitbrforecheck;
                switch (CheckWinConditions())
                {
                    case GameStatus.GS_LOSE:
                        if (GameState.Instance)
                            GameState.Instance.GoToLoseState();
                        break;
                    case GameStatus.GS_WIN:
                        if (GameState.Instance)
                            GameState.Instance.GoToWinState();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Prepare all data for checking and make a check
        /// </summary>
        /// <returns></returns>
        public GameStatus CheckWinConditions()
        {
            // update value
            if (Ball.Instance)
                projectileLink = Ball.Instance.gameObject;
            else
                projectileLink = null;
            enemiesAmount = EnemyManager.Instance.GetActiveEnemiesAmount();
            platformHealh = Platform.Instance.GetPlatformSettings.Health;

            // SessionCondition from GameData
            return loseCondition.CheckConditions(projectileLink, enemiesAmount, platformHealh);
        }

        /** */
        public override void Disable()
        {
            base.Disable();
            active = false; // deactivate Coroutine
        }
    }
}