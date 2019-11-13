using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    public class Enemy_KamikadzeBrick : Enemy
    {
        [SerializeField]
        protected KamikadzeBrickSettings kamikadzeSettings = new KamikadzeBrickSettings();

        // KamikadzeBrick settings about movement and health
        public KamikadzeBrickSettings GetKamikadzeSettings { get { return kamikadzeSettings; } private set { } }

        // Use this for initialization
        private void Start()
        {
            // include special settings for this  enemy class 
            GetEnemySettings.GetSpecialEnemySettings = GetKamikadzeSettings;

            state = new KamikadzeWaitState(this);
        }

        public override void ToActiveStateActivate()
        {
            state = new KamikadzeWaitState(this);
        }
    }
}