using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PickUps
{
    /// <summary>
    /// Main active state
    /// </summary>
    public class PickUpActiveState : PickUpState
    {
        private float lifeTime;
        private bool isPickUpLife = false;

        public PickUpActiveState(Pickup pu)
        {
            GetSettings(pu);
            SetInitialPosition();
            ShowEffectsOn();
            ActivateLifeTimer(); // this timer cheaper for GC
        }

        private void SetInitialPosition()
        {
            // set spawn position on the line with platform
            Vector3 pos = Platform.Instance.gameObject.transform.position;
            pos.x = Random.Range(pus.GetSpawnPosShiftLimits.x, pus.GetSpawnPosShiftLimits.y);
            parent.transform.position = pos + pus.PositionShift;
        }


        private void ActivateLifeTimer()
        {
            lifeTime = pus.LifeTime;
            isPickUpLife = true;
        }


        public override void Update()
        {
            // to do some stuff
            if (isPickUpLife) // life time cicle
            {
                lifeTime -= Time.deltaTime;
                if (lifeTime < 0.0f) // when it is done go to PickUpDeactiveState
                {
                    isPickUpLife = false;
                    if (parent)
                        parent.DeActivateState();
                }
            }
        }

        private void ShowEffectsOn() { }
    }
}