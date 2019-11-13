using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    /// <summary>
    ///  Main active life state
    /// </summary>
    public class KamikadzeWaitState : KamikadzeActiveState
    {
        private float _waitTime = 0;
        public KamikadzeWaitState(Enemy e) : base(e)
        {
            _waitTime = Random.Range(0, kamikadzeBrickSettings.WaitTime);
        }

        public override void Update()
        {
            // wait staff
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0.0f)
                parent.State = new KamikadzeAttackState(parent);
        }
    }
}
