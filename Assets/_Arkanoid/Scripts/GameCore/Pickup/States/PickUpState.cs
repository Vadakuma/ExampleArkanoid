using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PickUps
{
    public abstract class PickUpState
    {
        protected PickUpSettings pus;
        protected Pickup parent;

        public abstract void Update();

        public void GetSettings(Pickup pu)
        {
            parent = pu;
            pus = parent.GetPickUpSettings;
        }
    }
}
