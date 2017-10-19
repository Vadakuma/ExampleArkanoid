using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    /***************************************************************************************************
     * PLATFORM STATES
     * *************************************************************************************************/
    public interface IPlatformState
    { 
        void Init(Platform platform);
        void Update(Platform platform);
        void MoveTo(Platform platform, int side);
        void AddDamage(int amount);

        void AddAbility(Platform platform, IAbility ability);
        void Disable();
        void GoToNextState();
    }

    /**
        * 
        * */
    public abstract class InitialState : IPlatformState
    {
        // link to current tick command
        private     IPlatformCommand    command;
        // 
        private     InputControl        inputcontrol;
        // base settings about movement and health
        protected   PlatformSettings    platformSettings;
        protected   Platform            parent;

        public InitialState(Platform platform)
        {
            Init(platform); // setup settings
        }

        public virtual void Init(Platform platform)
        {
            parent = platform;
            platformSettings = platform.GetPlatformSettings;
            inputcontrol = new InputControl();
        }

        public virtual void MoveTo(Platform platform, int side) {  }

        public virtual void Update(Platform platform) {

            // checking and  get command from input control
            command = inputcontrol.InputUpdater();
            if (command != null)
            {
                command.execute(platform);
            }
        }

        public virtual void AddDamage(int amount) {        }

        public void AddAbility(Platform platform, IAbility ability)
        {
            try
            {
                ability.Apply(platform);
            }
            catch
            {

            }
        }

        public void GoToNextState() { }

        public void Disable() {
            inputcontrol.OnDispose();
        }
    }

    /*
     * MAIN GAME STATE 
     * Active input, set and get damage, set ability
     */
    public class ActionState : InitialState
    {
        private Vector3     pos = Vector3.zero; // next platform position
        private Vector3     curpos = Vector3.zero; // next platform position
        private float       platformShift;      // mavement shift amount
        private float       nextpos;
        private int         lastside = 0;

        public ActionState(Platform platform) : base (platform)
        {
            curpos = platform.transform.position; // set initial position
            pos = platform.transform.position; // set initial position

        }

        /** */
        public override void Update(Platform platform) 
        {
            base.Update(platform); // base checking commands from input
           
            // moving the platform
            // TODO: clamping algorithm optimization need
            platformShift = Mathf.Lerp(platformShift, 0.0f, platformSettings.SpeedDampness * Time.deltaTime);
            if (platformShift != 0)
            {
                curpos = Vector3.Lerp(curpos, pos, platformSettings.SpeedDampness * Time.deltaTime);

                nextpos = Mathf.Clamp(curpos.x,
                     platformSettings.GetMovementShiftLimits.x,
                     platformSettings.GetMovementShiftLimits.y);
                 curpos.x = nextpos;


                 platform.transform.position = curpos;
            }
        }

        /** TODO: clamping algorithm optimization need */
        public override void MoveTo(Platform platform, int side) {

            platformShift = side * platformSettings.Speed;

            // reset posx if switching movement direction 
            if (lastside != side) {
                lastside = side;
                // to save inversion effect we do this only on edges of level
                if(platform.transform.position.x == platformSettings.GetMovementShiftLimits.x ||
                   platform.transform.position.x == platformSettings.GetMovementShiftLimits.y)
                pos.x = platform.transform.position.x;
            }

            pos.x = Mathf.Lerp(pos.x, platform.transform.position.x + platformShift, platformSettings.Acceleration * Time.deltaTime);
        }

        public override void AddDamage( int amount)
        {
            if (platformSettings.Health > 0)
            {
                platformSettings.Health -= amount;
                if (platformSettings.Health < 1)
                    parent.GoToDeadState();
            }
        }
    }

    /*
     * Stop moving, get damage and ability
     */
    public class IdleState : InitialState
    {
        public IdleState(Platform platform) : base(platform) { }

        public new void Init(Platform platform) { }
        public new void Update(Platform platform) {   }
        public new void MoveTo(Platform platform, int side) { }
    }

    /*
     * Stop moving, get damage and ability restore data and postion
     */
    public class ResetState : InitialState
    {
        public ResetState(Platform platform) : base(platform) {
            platform.transform.position = Platform.Initpos;
            platformSettings.Health = platformSettings.MaxHealth;
        }

        public new void Init(Platform platform) { }
        public new void Update(Platform platform) {
            if (parent)
                parent.GoToActiveState();
        }
        public new void MoveTo(Platform platform, int side) { }
    }

    /***************************************************************************************************
     * PLATFORM SETTINGS
     * *************************************************************************************************/
    [System.Serializable]
    public class PlatformSettings
    {
        [SerializeField, Range(0, 10), Tooltip("Speed multiplier")]
        protected float     speed;
        [SerializeField, Tooltip("How fast we will get zero speed")]
        protected float     speedDampness;
        [SerializeField, Range(0, 1), Tooltip("How fast we will get max speed after input command")]
        protected float     accel;
        [SerializeField, Tooltip("Initial platform player health")]
        protected int       health;
        [SerializeField, Tooltip("Maximum platform player health")]
        protected int       maxHealth;
        [SerializeField, Tooltip(" ")]
        protected Vector2   movementShiftLimits = new Vector2(-10, 10);


       

        // max platform speed
        public float    Speed { get { return speed * SpeedUpFactor; } set { speed = value; } }
        // How fast we will get zero speed
        public float    SpeedDampness { get { return speedDampness; } }
        // How fast we will get max speed after input command
        public float    Acceleration { get { return accel; } }
        public Vector2  GetMovementShiftLimits { get { return movementShiftLimits; } }
        public int      Health{ get { return health; } set { health = value; } }
        public int      MaxHealth { get { return maxHealth; } }

        public float   speedUpFactor = 1.0f;
        public float    SpeedUpFactor { get { return speedUpFactor; } set { speedUpFactor = value; }  }
    }


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

        void Awake()
        {
            Instance = this;
            initpos = transform.position;
        }

        // Use this for initialization
        void Start()
        {
            // starting  
            GoToIdleState();
        }

        // Update is called once per frame
        void Update()
        {
            state.Update(this);
        }

        /** Stop moving, damage and ability*/
        public void GoToIdleState()
        {
            state = new IdleState(this);
        }

        public void GoToDeadState()
        {
            state = new IdleState(this);
        }

        /** Active input, damage and ability */
        public void GoToActiveState()
        {
            state = new ActionState(this);
        }

        public void GoToResetState()
        {
            state = new ResetState(this);
        }

        /** moving*/
        public void MoveTo(int side)
        {
            state.MoveTo(this, side);
        }

        /** apply damage */
        public void AddDamage(int amount)
        {
            state.AddDamage(amount);
        }

        /** apply Ability*/
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