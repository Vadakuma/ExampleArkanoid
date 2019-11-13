using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{

    /// <summary>
    /// New Active State for Kamikadze Brick
    /// </summary>
    public abstract class KamikadzeActiveState : EnemyActiveState
    {
        protected KamikadzeBrickSettings kamikadzeBrickSettings;

        public KamikadzeActiveState(Enemy e) : base(e)
        {
            kamikadzeBrickSettings = e.GetEnemySettings.GetSpecialEnemySettings as KamikadzeBrickSettings;
        }
    }

}
