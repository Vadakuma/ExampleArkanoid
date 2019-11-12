using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.Enemies
{
    public enum EnemyType
    {
        Simple = 0,
        Armor = 1,
        Kamikasze = 2 
    };

    public class Enemy : MonoBehaviour, IDisposable
    {
        // set from inspector
        public EnemyType _enemyType;
        public EnemyType EnemyType { get { return _enemyType; }  protected set { _enemyType = value; } }

        [SerializeField]
        protected BaseEnemySettings enemySettings = new BaseEnemySettings();
        //Base settings about movement and health
        public BaseEnemySettings    GetEnemySettings { get { return enemySettings; } private set { } }

        protected   IEnemyState state;
        public      IEnemyState State { get { return state; } set { state = value; } }

        protected   EnemyPool parentpool;
        public      EnemyPool ParentPool { get { return parentpool; } set { parentpool = value; } }


        // Use this for initialization
        private void Start()
        {
            State = new EnemyActiveState(this);
        }

        // Update is called once per frame
        private void Update()
        {
            state.Update();
        }

        /** apply damage to this enemy */
        public void AddDamage(int amount)
        {
            state.AddDamage(this, amount);
        }

        /// <summary>
        /// deactivating command from pool
        /// TODO: add activating state
        /// </summary>
        /// <returns></returns>
        public Enemy DeActivate()
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
            
            return this;
        }

        /// <summary>
        /// deactivating command from pool
        /// TODO: add deactivating state
        /// </summary>
        /// <returns></returns>
        public Enemy Activate()
        {
            if(!gameObject.activeSelf)
                gameObject.SetActive(true);

            return this;
        }

        /// <summary>
        /// virtual because the most demanding state for the chnges action
        /// </summary>
        public virtual void ToActiveStateActivate()
        {
            state = new EnemyActiveState(this);
        }

        /** */
        public void ToDeadStateActivate()
        {
            state = new EnemyDeadState(this);
        }

        /** */
        public void ToIdleStateActivate()
        {
            state = new EnemyIdleState(this);
        }

        public void Dispose()
        {
            //TODO: implement disposing!
        }
    }
}