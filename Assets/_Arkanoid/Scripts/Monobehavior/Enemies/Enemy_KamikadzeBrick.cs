using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    [System.Serializable]
    public class KamikadzeBrickSettings : IEnemySettings
    {
        [SerializeField, Tooltip("Max wait time to attack")]
        protected float     waitTime;
        [SerializeField, Tooltip("Speed multiplier")]
        protected float     speed;
        [SerializeField, Tooltip(" ")]
        protected float     minAttackDist = 0.5f;
        [SerializeField, Tooltip(" ")]
        protected int       damage = 1;
        // max speed
        public float    Speed { get { return speed; } }
        public float    WaitTime { get { return waitTime; } }
        public float    MinAttackDist { get { return minAttackDist; } }
        public int      Damage { get { return damage; } }

        public void Reset()
        {

        }
    }

    /** New Active State for Kamikadze Brick
     * */
    [System.Serializable]
    public abstract class EnemyKamikadzeActiveState : EnemyActiveState
    {
        protected KamikadzeBrickSettings kamikadzeBrickSettings;

        public EnemyKamikadzeActiveState(Enemy e) :base (e)
        {
            kamikadzeBrickSettings = e.GetEnemySettings.GetSpecialEnemySettings as KamikadzeBrickSettings;
        }
    }

    /** Main active life state*/
    [System.Serializable]
    public class EnemyKamikadzeWaitState : EnemyKamikadzeActiveState
    {
        private float waitTime = 0;
        public EnemyKamikadzeWaitState(Enemy e) : base (e)
        {
            waitTime = Random.Range(0, kamikadzeBrickSettings.WaitTime);
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
    public class EnemyKamikadzeAttackState : EnemyKamikadzeActiveState
    {
        private Transform   bricktr;
        private Transform   platformtr;
        private Vector3     dir = Vector3.zero;
        private float       dist;
        private bool        isActive = false;

        public EnemyKamikadzeAttackState(Enemy e) : base (e)
        {
            bricktr = e.transform;
            platformtr = Platform.Instance.transform;
            isActive = true; // let stat attack!
        }

        /** */
        public override void Update()
        {
            
            if (isActive)
            {
                // move and attack staff
                dist = Vector3.Distance(platformtr.position, bricktr.position);
                if (dist < kamikadzeBrickSettings.MinAttackDist)
                    ExplodeAction();
                // direction to the platform
                dir = platformtr.position - bricktr.position;
                // move on last calculate direction
                bricktr.Translate(dir.normalized * Time.deltaTime * kamikadzeBrickSettings.Speed);
            }
        }

        /** */
        private void ExplodeAction()
        {
            Debug.Log("ExplodeAction!");
            isActive = false;
            if (Platform.Instance)
                Platform.Instance.AddDamage(kamikadzeBrickSettings.Damage);
            // dying stuff
            parent.ToDeadStateActivate();
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
            // include special settings for this  enemy class 
            GetEnemySettings.GetSpecialEnemySettings = GetKamikadzeSettings;

            state = new EnemyKamikadzeWaitState(this);
        }

        /** */
        public override void ToActiveStateActivate()
        {
            state = new EnemyKamikadzeWaitState(this);
        }
    }
}