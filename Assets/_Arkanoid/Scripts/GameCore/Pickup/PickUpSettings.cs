using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.PickUps
{

    [System.Serializable]
    public class PickUpSettings
    {
        [SerializeField]
        protected float lifeTime = 5;
        [SerializeField, Tooltip("Death delay")]
        protected float deathTime = 1;
        [SerializeField, Tooltip("Max shift in the spawn line")]
        protected Vector2 spawnPosShiftLimits = new Vector2(-5, 5);
        [SerializeField, Tooltip("Spawn position shift")]
        protected Vector3 spawnPositionShift = new Vector3(0, 0, 0);

        // max platform speed
        public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }
        public float DeathTime { get { return deathTime; } set { deathTime = value; } }
        public Vector2 GetSpawnPosShiftLimits { get { return spawnPosShiftLimits; } }
        public Vector3 PositionShift { get { return spawnPositionShift; } }
    }
}
