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
        private WaitForSeconds      _waitbrforecheck = new WaitForSeconds(0.15f);
        private bool                _active = false;
        private SessionCondition    _loseCondition;
        // cach for win conditions checking
        private GameObject          _projectileLink;
        private int                 _enemiesAmount;
        private int                 _platformHealh;

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

            _loseCondition = GameState.gameData.GetLoseCondition;
            _active = true;
            GameState.Instance.StartCoroutine(CheckConditions());

            SetActiveGameActors();
        }

        /// <summary>
        /// Check gameplay win conditions
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckConditions()
        {
            while (_active)
            {
                yield return _waitbrforecheck;
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
                _projectileLink = Ball.Instance.gameObject;
            else
                _projectileLink = null;
            _enemiesAmount = EnemyManager.Instance.GetActiveEnemiesAmount();
            _platformHealh = Platform.Instance.GetPlatformSettings.Health;

            // SessionCondition from GameData
            return _loseCondition.CheckConditions(_projectileLink, _enemiesAmount, _platformHealh);
        }

        /** */
        public override void Disable()
        {
            base.Disable();
            _active = false; // deactivate Coroutine
        }
    }
}