using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /***************************************************************************************************
     * Enemy STATES
     * *************************************************************************************************/
    public interface IEnemyState
    {
        void Update(Enemy platform);
        void AddDamage(int amount);
    }

    [System.Serializable]
    public class ActiveState : IEnemyState
    {
        public void Update(Enemy platform) { }
        public void AddDamage(int amount) { }
    }

    [System.Serializable]
    public class DeadState : IEnemyState
    {
        public void Update(Enemy platform) { }
        public void AddDamage(int amount) { }
    }

    [System.Serializable]
    public class IAttackState
    {

    }

    /***************************************************************************************************
        * Enemy SETTINGS
        * *************************************************************************************************/
    [System.Serializable]
    public class BaseEnemySettings
    {
        [SerializeField, Range(0, 10), Tooltip("Speed multiplier")]
        protected float speed;
        [SerializeField, Tooltip("Maximum platform health")]
        protected int   maxHealth;

        // max speed
        public float Speed { get { return speed; } }
        public int MaxHealth { get { return maxHealth; } }
    }


    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected BaseEnemySettings enemySettings = new BaseEnemySettings();

        private IEnemyState state;
        protected EnemyPool parentpool;


        public EnemyPool ParentPool { get { return parentpool; } set { parentpool = value; } }

        // Use this for initialization
        void Start()
        {
            state = new ActiveState();
        }

        // Update is called once per frame
        void Update()
        {
            state.Update(this);
        }

        public void AddDamage(int amount)
        {
            Debug.Log("Damage: " + amount + "||" + gameObject.name);
            state.AddDamage(amount);
        }

        /*private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
            Projectile pr = other.gameObject.GetComponent<Projectile>();
            if (pr != null)
                AddDamage(pr.DamageAmount);
        }*/




        /** deactivating command from pool*/
        public void DeActivate()
        {
            state = new DeadState();
        }
    }
}