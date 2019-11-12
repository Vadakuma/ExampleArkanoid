using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PickUps
{

    /// <summary>
    /// Destroy staff state
    /// </summary>
    public class PickUpDeactiveState : PickUpState
    {
        public PickUpDeactiveState(Pickup pu)
        {
            GetSettings(pu);
            // set up deactivate stuff
            CollisionOff(parent.GetComponent<Collider>());
            HideEffectsOn();

            // Destroying by delay timer
            GameObject.Destroy(parent.gameObject, pus.DeathTime);
        }

        /** */
        public override void Update()
        {
            // to do some stuff
        }

        private void CollisionOff(Collider pucollider)
        {
            if (pucollider)
                pucollider.enabled = false;
        }
        private void HideEffectsOn() { }
    }

}
