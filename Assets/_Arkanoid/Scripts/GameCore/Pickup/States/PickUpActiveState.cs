using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Arkanoid.PickUps
{
    /// <summary>
    /// Main active state
    /// </summary>
    public class PickUpActiveState : PickUpState
    {
        private float _lifeTime;
        private bool _isPickUpLife = false;

        public PickUpActiveState(Pickup pu)
        {
            GetSettings(pu);
            SetInitialPosition();
            ShowEffectsOn();
            ActivateLifeTimer(); // this timer cheaper for GC
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetInitialPosition()
        {
            // set spawn position on the line with platform
            Vector3 pos = Platform.Instance.gameObject.transform.position;
            pos.x = Random.Range(pus.GetSpawnPosShiftLimits.x, pus.GetSpawnPosShiftLimits.y);
            parent.transform.position = pos + pus.PositionShift;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ActivateLifeTimer()
        {
            _lifeTime = pus.LifeTime;
            _isPickUpLife = true;
        }


        public override void Update()
        {
            // to do some stuff
            if (_isPickUpLife) // life time cicle
            {
                _lifeTime -= Time.deltaTime;
                if (_lifeTime < 0.0f) // when it is done go to PickUpDeactiveState
                {
                    _isPickUpLife = false;
                    if (parent)
                        parent.DeActivateState();
                }
            }
        }

        private void ShowEffectsOn() { }
    }
}