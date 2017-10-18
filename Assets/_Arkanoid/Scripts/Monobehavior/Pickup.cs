using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /***************************************************************************************************
     * PICKUP STATES
     * *************************************************************************************************/
    public abstract class PickUpState
    {
        protected  PickUpSettings pus;
        protected Pickup parent;

        public abstract void Update();

        public void GetSettings(Pickup pu)
        {
            parent = pu;
            pus = parent.GetPickUpSettings;
        }
    }

    /** Main active state*/
    [System.Serializable]
    public class PickUpActiveState : PickUpState
    {
        private float lifeTime;
        private bool isPickUpLife = false;

        public PickUpActiveState(Pickup pu)
        {
            GetSettings(pu);
            SetInitialPosition();
            ShowEffectsOn();
            ActivateLifeTimer(); // this timer cheaper for GC
        }

        /** */
        private void SetInitialPosition()
        {
            // set spawn position
            Vector3 pos = Platform.Instance.gameObject.transform.position;
            pos.x = Random.Range(pus.GetMovementShiftLimits.x, pus.GetMovementShiftLimits.y);
            parent.transform.position = pos + pus.PositionShift;
        }

        /** */
        private void ActivateLifeTimer()
        {
            lifeTime = pus.LifeTime;
            isPickUpLife = true;
        }

        /** */
        public override void Update()
        {
            // to do some staff
            if(isPickUpLife) // life time cicle
            {
                lifeTime -= Time.deltaTime;
                if(lifeTime < 0.0f) // when it is done go to PickUpDeactiveState
                {
                    isPickUpLife = false;
                    if (parent)
                        parent.DeActivateState();
                }
            }
        }

        private void ShowEffectsOn() { }
    }

    /** Destroy staff state*/
    [System.Serializable]
    public class PickUpDeactiveState : PickUpState
    {
        public PickUpDeactiveState(Pickup pu)
        {
            GetSettings(pu);
            // set up deactivate staff
            CollisionOff(parent.GetComponent<Collider>());
            HideEffectsOn();

            // Destroying by delay timer
            GameObject.Destroy(parent.gameObject, pus.DeathTime);
        }

        /** */
        public override void Update() {
            // to do some staff
        }

        private void CollisionOff(Collider pucollider) {
            if (pucollider)
                pucollider.enabled = false;
        }
        private void HideEffectsOn() { }
    }


    /***************************************************************************************************
     * PICKUP SETTINGS
     * *************************************************************************************************/
    [System.Serializable]
    public class PickUpSettings
    {
        [SerializeField]
        protected float lifeTime = 5;
        [SerializeField, Tooltip("Death delay")]
        protected float deathTime = 1;
        [SerializeField, Tooltip(" ")]
        protected Vector2 movementShiftLimits = new Vector2(-10, 10);
        [SerializeField, Tooltip("Spawn position shift")]
        protected Vector2 spawnPositionShift = new Vector3(0,0,0);

        // max platform speed
        public float LifeTime { get { return lifeTime; } set { lifeTime = value; } }
        public float DeathTime { get { return deathTime; } set { deathTime = value; } }
        public Vector2 GetMovementShiftLimits { get { return movementShiftLimits; } }
        public Vector3 PositionShift { get { return spawnPositionShift; } }
    }

    /***************************************************************************************************
     * PICLUP MONO -  pickup control actor
     * *************************************************************************************************/
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        protected PickUpSettings    pickUpSettings = new PickUpSettings();

        private PickUpState         state;
        private Ability             abilityContainer;
        public  Ability             AbilityContainer { get { return abilityContainer; } private set { } }


        //Base settings about
        public PickUpSettings GetPickUpSettings { get { return pickUpSettings; } private set { } }

        // Use this for initialization
        void Start()
        {
            state = new PickUpActiveState(this);
        }

        /** Set ability to this pickup when spawning from Level.cs*/
        public void SetAbility(Ability ab)
        {
            AbilityContainer = ab;
        }

        // Update is called once per frame
        void Update() { state.Update();  }

        /** */
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pickup Ability OnTriggerEnter");
            if(other.CompareTag("Player"))
            {
                Platform player = other.gameObject.GetComponent<Platform>();
                player.AddAbility(this);
            }
        }

        /** destroy staff and effects */
        public void DeActivateState()
        {
            state = new PickUpDeactiveState(this);
        }
    }
}