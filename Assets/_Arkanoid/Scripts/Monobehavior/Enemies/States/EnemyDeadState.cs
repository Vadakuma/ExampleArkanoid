using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    /// <summary>
    /// Death stuff state
    /// </summary>
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(Enemy e) : base(e)
        {
            DeadEffects();
            ReturnToThePool();
        }
        public override void Update() { }
        public override void AddDamage(Enemy e, int amount) { }

        /** */
        protected void DeadEffects() { }

        /** */
        protected void ReturnToThePool()
        {
            EnemyManager.Instance.RemoveFromActive(parent);
            parent.ToIdleStateActivate();
        }
    }

}