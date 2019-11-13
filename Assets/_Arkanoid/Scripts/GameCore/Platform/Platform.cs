using Arkanoid.Abilities;
using Arkanoid.PickUps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.PlayerPlatform
{
    [RequireComponent(typeof(Rigidbody))] // for gameplay staff need rigidbody
    public class Platform : MonoBehaviour
    {
        [SerializeField, Tooltip("Base settings about movement, health, ...")] // see GameData.cs
        protected PlatformSettings platformSettings = new PlatformSettings();
        //Base settings about movement and health
        public PlatformSettings GetPlatformSettings { get { return platformSettings; } private set { } }

        // platform state switcher
        private static IPlatformState _state;
        public static IPlatformState State { get { return _state; } private set { _state = value; } }

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

        private void OnEnable()
        {
            UpdateManager.SubscribeToUpdate(OnUpdate);
        }

        private void OnDisable()
        {
            UpdateManager.UnSubscribeFromUpdate(OnUpdate);
        }

        private void OnUpdate()
        {
            _state.Update(this);
        }

        /// <summary>
        ///  Stop moving, damage and ability
        /// </summary>
        public void GoToIdleState()
        {
            _state = new IdleState(this);
        }

        public void GoToDeadState()
        {
            _state = new IdleState(this);
        }

        /// <summary>
        /// Active input, damage and ability
        /// </summary>
        public void GoToActiveState()
        {
            _state = new ActionState(this);
        }

        public void GoToResetState()
        {
            _state = new ResetState(this);
        }

        public void MoveTo(int side)
        {
            _state.MoveTo(this, side);
        }

        public void AddDamage(int amount)
        {
            _state.AddDamage(amount);
        }

        public void AddAbility(Pickup pickup)
        {
            _state.AddAbility(this, pickup.AbilityContainer);

            pickup.DeActivateState();
        }

        private void OnDestroy()
        { 
            if(_state != null)
                _state.Disable();
        }
    }
}