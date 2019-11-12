using Arkanoid.GameStates;
using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    /// <summary>
    /// show info to user interface
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [SerializeField, Tooltip("Rand from 0 to value")]
        private float     updatenFrequency = 0.5f;
        [SerializeField]
        private Text      hightscore;
        [SerializeField]
        private Text      roundindex;
        [SerializeField]
        private Text      health;

        private WaitForSeconds      _updatewait;
        private bool                _isActive = false;
        private PlatformSettings    _platformSettings;
        private PlayerData          _playerData;

        // Use this for initialization
        private void Start()
        {
            // for Coroutine
            _updatewait = new WaitForSeconds(updatenFrequency);

            _platformSettings = Platform.Instance.GetPlatformSettings;
            _playerData = GameData.SessionPlayerData;

            _isActive = true;
     
            StartCoroutine(UpdateHUD());
        }

        private IEnumerator UpdateHUD()
        {
            int sc = 0; // find last total score result
            while (_isActive)
            {
                yield return _updatewait;
                // update hud
                if (health)
                    health.text = _platformSettings.Health.ToString();

                if (hightscore)
                {
                    if (GameState.gameData != null && GameState.gameData.SessionsResults.Count > 0)
                        sc = GameState.gameData.SessionsResults[GameState.gameData.SessionsResults.Count - 1].TotalScore;

                    hightscore.text = (_playerData.Score + sc).ToString();
                }
                    
                if (roundindex)
                    roundindex.text = (_playerData.MaxRoundCounter + 1).ToString();
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}