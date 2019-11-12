using Arkanoid.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.PlayerPlatform
{

    /***************************************************************************************************
     * PLATFORM MONO -  player control actor
     * *************************************************************************************************/
    [RequireComponent(typeof(Rigidbody))] // for gameplay staff need rigidbody
    public class Platform : MonoBehaviour
    {
        [SerializeField, Tooltip("Base settings about movement, health, ...")] // see GameData.cs
        protected PlatformSettings platformSettings = new PlatformSettings();
        //Base settings about movement and health
        public PlatformSettings GetPlatformSettings { get { return platformSettings; } private set { } }

        // platform state switcher
        private static IPlatformState state;
        public static IPlatformState State { get { return state; } private set { state = value; } }

        private static Platform _instance;
        public static Platform Instance { get { return _instance; } private set { _instance = value; } }


        // initial position at start scene
        public static Vector3 initpos;
        public static Vector3 Initpos { get { return initpos; } private set { initpos = value; } }

        private void Awake()
        {
            Instance = this;
            initpos = transform.position;
        }

        // Use this for initialization
        private void Start()
        {
            // starting  
            GoToIdleState();
        }

        //TODO: subscribe  to General UpdateManager instead using that!
        private void Update()
        {
            state.Update(this);
        }

        /// <summary>
        ///  Stop moving, damage and ability
        /// </summary>
        public void GoToIdleState()
        {
            state = new IdleState(this);
        }

        public void GoToDeadState()
        {
            state = new IdleState(this);
        }

        /// <summary>
        /// Active input, damage and ability
        /// </summary>
        public void GoToActiveState()
        {
            state = new ActionState(this);
        }

        public void GoToResetState()
        {
            state = new ResetState(this);
        }

        public void MoveTo(int side)
        {
            state.MoveTo(this, side);
        }

        public void AddDamage(int amount)
        {
            state.AddDamage(amount);
        }

        public void AddAbility(Pickup pickup)
        {
            state.AddAbility(this, pickup.AbilityContainer);

            pickup.DeActivateState();
        }

        private void OnDestroy()
        { 
            if(state != null)
                state.Disable();
        }
    }
}