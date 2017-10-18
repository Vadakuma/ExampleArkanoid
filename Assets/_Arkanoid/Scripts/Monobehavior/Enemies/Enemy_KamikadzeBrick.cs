using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    [System.Serializable]
    public class KamikadzeBrickSettings
    {
        [SerializeField, Tooltip("Wait time to attack")]
        protected float waitTime;
        [SerializeField, Tooltip("Speed multiplier")]
        protected float speed;

        // max speed
        public float Speed { get { return speed; } }
        public float WaitTime { get { return waitTime; } }
    }

    /**  */
    public interface IEnemyKamikadzeState
    {
        void GetSpecialSettings();
    }

    [System.Serializable]
    public abstract class EnemyKamikadzeState : EnemyActiveState, IEnemyKamikadzeState
    {
        protected KamikadzeBrickSettings kamikadzeBrickSettings;

        public EnemyKamikadzeState(Enemy e):base (e)
        {
            SetSettings(e);
            // temp
            GetSpecialSettings();
        }
        // temp
        public void GetSpecialSettings()
        {
            kamikadzeBrickSettings = (parent as Enemy_KamikadzeBrick).GetKamikadzeSettings;
        }
    }

    /** Main active life state*/
    [System.Serializable]
    public class EnemyKamikadzeWaitState : EnemyKamikadzeState
    {
        private float waitTime = 0;
        public EnemyKamikadzeWaitState(Enemy e) : base (e)
        {
            SetSettings(e);
            GetSpecialSettings();

            waitTime = kamikadzeBrickSettings.WaitTime;
        }


        public override void Update()
        {
            // wait staff
            waitTime -= Time.deltaTime;
            if (waitTime < 0.0f)
                parent.State = new EnemyKamikadzeAttackState(parent);
        }
    }


    /** Main active life state*/
    [System.Serializable]
    public class EnemyKamikadzeAttackState : EnemyKamikadzeState
    {
        public EnemyKamikadzeAttackState(Enemy e) : base (e)
        {
            SetSettings(e);
            GetSpecialSettings();
            //Debug.Log("EnemyKamikadzeAttackState");
        }

        public override void Update()
        {
            // move and attack staff
        }
    }

    /** */
    public class Enemy_KamikadzeBrick : Enemy
    {
        [SerializeField]
        protected KamikadzeBrickSettings kamikadzeSettings = new KamikadzeBrickSettings();

        // KamikadzeBrick settings about movement and health
        public KamikadzeBrickSettings GetKamikadzeSettings { get { return kamikadzeSettings; } private set { } }

        // Use this for initialization
        void Start()
        {
            state = new EnemyKamikadzeWaitState(this);
        }
    }
}