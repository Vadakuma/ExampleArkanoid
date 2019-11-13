using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    [System.Serializable]
    public class KamikadzeBrickSettings : IEnemySettings
    {
        [SerializeField, Tooltip("Max wait time to attack")]
        protected float waitTime;
        [SerializeField, Tooltip("Speed multiplier")]
        protected float speed;
        [SerializeField, Tooltip(" ")]
        protected float minAttackDist = 0.5f;
        [SerializeField, Tooltip(" ")]
        protected int damage = 1;
        // max speed
        public float Speed { get { return speed; } }
        public float WaitTime { get { return waitTime; } }
        public float MinAttackDist { get { return minAttackDist; } }
        public int Damage { get { return damage; } }

        public void Reset()
        {

        }
    }
}
