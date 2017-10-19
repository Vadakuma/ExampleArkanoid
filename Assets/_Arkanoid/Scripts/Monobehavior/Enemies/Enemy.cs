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

        void Update();
        void AddDamage(Enemy e, int amount);
    }

    [System.Serializable]
    public abstract class EnemyBaseState : IEnemyState
    {
        protected Enemy parent;
        protected BaseEnemySettings bes;

        public virtual void Update() { }
        public virtual void AddDamage(Enemy e, int amount) { }

        protected void SetSettings(Enemy e)
        {
            parent = e;
            if (parent)
            {
                bes = parent.GetEnemySettings;
                bes.Init();
            }
        }
    }

    /** Main active life state*/
    [System.Serializable]
    public class EnemyActiveState : EnemyBaseState
    {
        public EnemyActiveState(Enemy e)
        {
            SetSettings(e);
            ActivateEffects();
        }
        public override void Update() { }
        public override void AddDamage(Enemy e, int amount) {
            if (e == null) {
                Debug.Log("Enemy link is null");
                return;
            }

            if (bes.Health > 0)
            {
                bes.Health -= amount;
                if (bes.Health < 1)
                    e.ToDeadStateActivate();
            }
            else
            {
                e.ToDeadStateActivate();
            }
        }

        /** */
        protected void ActivateEffects() {   }
    }

    /** Death staff state*/
    [System.Serializable]
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(Enemy e) {
            SetSettings(e);
            DeadEffects();
            ReturnToThePool();
        }
        public override void Update() {  }
        public override void AddDamage(Enemy e, int amount) { }

        /** */
        protected void DeadEffects() {       }

        /** */
        protected void ReturnToThePool()
        {
            EnemyManager.Instance.RemoveFromActive(parent);
            //Debug.Log("ReturnToThePool");
            parent.ParentPool.ReturnEnemy(parent);
            parent.ToIdleStateActivate();
        }
    }


    /** Death staff state*/
    [System.Serializable]
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(Enemy e)
        {
            SetSettings(e);
        }
        public override void Update() { }
        public override void AddDamage(Enemy e, int amount) { }
    }

    /***************************************************************************************************
    * Enemy SETTINGS
    * *************************************************************************************************/
    [System.Serializable]
    public class BaseEnemySettings
    {
        [SerializeField, Tooltip("Maximum platform health")]
        protected int   maxHealth;
        [SerializeField, Tooltip(" ")]
        protected int   score;

        public int MaxHealth { get { return maxHealth; } }
        public int Health { get;  set; }
        public int Score { get { return score; } private set { } }

        public void Init()
        {
            Health = MaxHealth;
        }
    }


    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        protected BaseEnemySettings baseEnemySettings = new BaseEnemySettings();

        protected   IEnemyState state;
        protected   EnemyPool   parentpool;

        public IEnemyState State { get { return state; } set { state = value; } }
        public EnemyPool ParentPool { get { return parentpool; } set { parentpool = value; } }
        //Base settings about movement and health
        public BaseEnemySettings GetEnemySettings { get { return baseEnemySettings; } private set { } }

        // Use this for initialization
        void Start()
        {
            state = new EnemyActiveState(this);
        }

        // Update is called once per frame
        void Update()
        {
            state.Update();
        }

        public void AddDamage(int amount)
        {
            //Debug.Log("Damage: " + amount + "||" + gameObject.name + "||" + state);
            state.AddDamage(this, amount);
        }

        /** deactivating command from pool*/
        public void DeActivate(EnemyPool ep)
        {
            if (parentpool == null)
                parentpool = ep;

            gameObject.SetActive(false);
        }

        /** deactivating command from pool*/
        public void Activate(EnemyPool ep)
        {
            gameObject.SetActive(true);
           // ResetSettings();
        }

        /** */
        public void ToActiveStateActivate()
        {
            state = new EnemyActiveState(this);
        }

        /** */
        public void ToDeadStateActivate()
        {
            //Debug.Log("ToDeadStateActivate: " + gameObject.name);
            state = new EnemyDeadState(this);
        }

        /** */
        public void ToIdleStateActivate()
        {
            state = new EnemyIdleState(this);
        }
    }
}