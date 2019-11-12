using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public interface IEnemySettings
    {
        void Reset();
    }

    [System.Serializable]
    public class BaseEnemySettings : IEnemySettings
    {
        [SerializeField, Tooltip("Maximum platform health")]
        protected int maxHealth;
        [SerializeField, Tooltip(" ")]
        protected int score;
        [SerializeField, Tooltip(" ")]
        protected Weapon weapon;
        public int MaxHealth { get { return maxHealth; } }
        public int Health { get; set; }
        public int Score { get { return score; } private set { } }

        // Enemy weapon for some attack actions ()

        public Weapon GetWeapon { get { return weapon; } }

        //TODO: Pawn(for body control), AI, ... 

        private IEnemySettings specialEnemySettings;
        public IEnemySettings GetSpecialEnemySettings { get { return specialEnemySettings; } set { specialEnemySettings = value; } }

        public void Reset()
        {
            Health = MaxHealth;

            if (specialEnemySettings != null)
                specialEnemySettings.Reset();
        }
    }
}