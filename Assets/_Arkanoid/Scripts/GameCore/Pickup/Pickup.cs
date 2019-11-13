using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.Abilities;
using Arkanoid.PlayerPlatform;

namespace Arkanoid.PickUps
{

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
        private void Start()
        {
            state = new PickUpActiveState(this);
        }

        private void OnEnable()
        {
            UpdateManager.SubscribeToUpdate(OnUpdate);
        }

        private void OnDisable()
        {
            UpdateManager.UnSubscribeFromUpdate(OnUpdate);
        }

        /// <summary>
        ///  Set ability to this pickup when spawning from PickUpManager.cs
        /// </summary>
        /// <param name="ab"></param>
        public void SetAbility(Ability ab)
        {
            abilityContainer = ab;
        }

        private void OnUpdate()
        {
            state.Update();
        }

        /** */
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Pickup Ability OnTriggerEnter");
            if(other.CompareTag("Player"))
            {
                Platform player = other.gameObject.GetComponent<Platform>();
                player.AddAbility(this);
            }
        }

        /// <summary>
        /// destroy staff and effects
        /// </summary>
        public void DeActivateState()
        {
            state = new PickUpDeactiveState(this);
        }
    }
}