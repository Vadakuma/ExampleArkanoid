using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(Enemy e) : base(e) { }
        public override void Update() { }
        public override void AddDamage(Enemy e, int amount) { }
    }
}