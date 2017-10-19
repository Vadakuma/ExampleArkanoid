using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    /** show info to user interface*/
    public class HUD : MonoBehaviour
    {
        [SerializeField, Tooltip("Rand from 0 to value")]
        protected float     updatenFrequency = 0.1f;
        [SerializeField]
        protected Text      hightscore;
        [SerializeField]
        protected Text      roundindex;
        [SerializeField]
        protected Text      health;

        private WaitForSeconds      updatewait;
        private bool                isActive = false;
        private PlatformSettings    ps; // cash
        private PlayerData          pd;
        // Use this for initialization
        void Start()
        {
            // for Coroutine
            updatewait = new WaitForSeconds(updatenFrequency);

            ps = Platform.Instance.GetPlatformSettings;
            pd = GameData.SessionPlayerData;

            isActive = true;
            StartCoroutine(UpdateHUD());
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator UpdateHUD()
        {
            while (isActive)
            {
                yield return updatewait;
                // update hud
                if (health)
                    health.text = ps.Health.ToString();
                if (hightscore)
                    hightscore.text = pd.Score.ToString();
                if (roundindex)
                    roundindex.text = pd.MaxRoundCounter.ToString();
            }
        }

    }
}