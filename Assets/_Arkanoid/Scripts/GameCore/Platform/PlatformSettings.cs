using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.PlayerPlatform
{

    [System.Serializable]
    public class PlatformSettings
    {
        [SerializeField, Range(0, 10), Tooltip("Speed multiplier")]
        protected float speed;
        [SerializeField, Tooltip("How fast we will get zero speed")]
        protected float speedDampness;
        [SerializeField, Range(0, 1), Tooltip("How fast we will get max speed after input command")]
        protected float accel;
        [SerializeField, Tooltip("Initial platform player health")]
        protected int health;
        [SerializeField, Tooltip("Maximum platform player health")]
        protected int maxHealth;
        [SerializeField, Tooltip(" ")]
        protected Vector2 movementShiftLimits = new Vector2(-10, 10);

        // max platform speed
        public float Speed { get { return speed * SpeedUpFactor; } set { speed = value; } }
        // How fast we will get zero speed
        public float SpeedDampness { get { return speedDampness; } }
        // How fast we will get max speed after input command
        public float Acceleration { get { return accel; } }
        public Vector2 GetMovementShiftLimits { get { return movementShiftLimits; } }
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } }

        public float speedUpFactor = 1.0f;
        public float SpeedUpFactor { get { return speedUpFactor; } set { speedUpFactor = value; } }
    }
}