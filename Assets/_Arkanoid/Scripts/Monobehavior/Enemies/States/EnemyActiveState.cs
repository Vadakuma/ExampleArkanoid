using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    public class EnemyActiveState : EnemyBaseState
    {
        public EnemyActiveState(Enemy e) : base(e)
        {
            ActivateEffects();
        }
        public override void Update() { }
        public override void AddDamage(Enemy e, int amount)
        {
            if (e == null)
            {
                Debug.Log("Enemy link is null");
                return;
            }

            if (bes.Health > 0)
            {

                bes.Health -= amount;
                if (bes.Health < 1)
                {
                    // we are dead!
                    e.ToDeadStateActivate();
                    // Player should get some points!
                    GameData.ApplyScore(bes.Score);
                }
            }
            else
            {
                //Debug.Log("(Health already 0!");
                e.ToDeadStateActivate();
            }
        }

        /** */
        protected void ActivateEffects() { }
    }
}
