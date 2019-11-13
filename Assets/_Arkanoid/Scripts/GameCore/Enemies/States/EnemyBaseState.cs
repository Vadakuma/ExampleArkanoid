using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    public interface IEnemyState
    {
        void Update();
        void AddDamage(Enemy e, int amount);
    }

    public abstract class EnemyBaseState : IEnemyState
    {
        protected Enemy parent;
        protected BaseEnemySettings bes;

        public EnemyBaseState(Enemy e)
        {
            SetSettings(e);
        }

        public virtual void Update() { }
        public virtual void AddDamage(Enemy e, int amount) { }

        /// <summary>
        /// set and save general links and settings
        /// </summary>
        /// <param name="e"></param>
        protected void SetSettings(Enemy e)
        {
            parent = e;
            if (parent)
            {
                bes = parent.GetEnemySettings;
                bes.Reset();
            }
        }
    }
}
