using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.Enemies;
using System.Runtime.CompilerServices;

namespace Arkanoid
{
    /// <summary>
    /// Unique projectile actor as easy bouncy ball.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        protected int   damage;
        [SerializeField]
        protected float speed;
        [SerializeField]
        protected float destroyTime;
        [SerializeField, Tooltip("Not stable")]
        protected bool  useStickToplatformEffect = false;

        public int      DamageAmount { get { return damage; } private set { } }
        public float    Speed { get { return speed; } private set { } }

        private Rigidbody   _projectileRigidbody;
        private Rigidbody   _playerRigidbody;
        private Collider    _projectileCollider;
        private Vector3     _direction      = Vector3.zero;
        private Vector3     _lastDirection  = Vector3.zero;
        private Vector3     _velocity       = Vector3.zero;

        private readonly Vector3 initdir = new Vector3(1, 1, 1);
        private          Vector3 _initpos;


        private WaitForSeconds _stickingwait = new WaitForSeconds(0.05f);

        private static Ball _instance;
        public static Ball Instance { get { return _instance; } private set { _instance = value; } }


        private void Awake()
        {
            Instance = this;
            _projectileRigidbody = GetComponent<Rigidbody>();

            _initpos = transform.position;
        }

        // Use this for initialization
        private void Start()
        {
            // initial inpulse

            StopMoving();
        }

        private void OnEnable()
        {
            UpdateManager.SubscribeToFixedUpdate(OnFixedUpdate);
        }

        private void OnDisable()
        {
            UpdateManager.UnSubscribeFromFixedUpdate(OnFixedUpdate);
        }


        private void OnFixedUpdate()
        {
            _direction = _projectileRigidbody.velocity;
        }

        /// <summary>
        /// Set to initial position,reactivate components, stop moving
        /// </summary>
        public void ResetProjectile()
        {
            gameObject.SetActive(true);
            transform.position = _initpos;
            _lastDirection = initdir;
            if (_projectileCollider)
                _projectileCollider.enabled = true;
            StopMoving();
        }

        public void StopMoving()
        {
            if (_projectileRigidbody != null)
            { 
                if (_projectileRigidbody.velocity != Vector3.zero){
                    _lastDirection = _projectileRigidbody.velocity;
                    _projectileRigidbody.velocity = Vector3.zero;
                }
            }
        }

        public void StartMoving()
        {
            if (_projectileRigidbody.velocity == Vector3.zero)
            {
                if (_lastDirection == Vector3.zero)
                    _lastDirection = initdir;

                _projectileRigidbody.velocity = _lastDirection.normalized * speed;
            }
        }

        /// <summary>
        /// Colliding for scene object - wall or player platform
        /// </summary>
        /// <param name="collisionInfo"></param>
        private void OnCollisionEnter(Collision collisionInfo)
        {
            //Debug.Log("Projectile OnCollisionEnter");
            ContactPoint cp = collisionInfo.contacts[0];

            // Checking and calculate the next direction to move
            if (useStickToplatformEffect && collisionInfo.gameObject.CompareTag("Player"))
            {
                _lastDirection = _projectileRigidbody.velocity;
                if (_playerRigidbody == null)
                {
                    _playerRigidbody = collisionInfo.gameObject.GetComponent<Rigidbody>();
                }
                // about 0.15f sec
                StartCoroutine(StickingToPlatformEffect(_playerRigidbody, cp.normal));
            }
            else
            {
                // calculate with Vector3.Reflect
                _velocity = Vector3.Reflect(_direction, cp.normal);
                // bounce effect to speed up ball
                _projectileRigidbody.velocity = _velocity.normalized * speed;
            }

            // Checking type of colliding object (may be it is enemy)
            // TODO: checking only tag and use GetComponent if CompareTag is true
            // during moving/copy paste to an another PC project folder some tag can be disappear! 
            Enemy e = collisionInfo.gameObject.GetComponent<Enemy>();
            if (collisionInfo.gameObject.CompareTag("Enemy") || e != null)
            {
                //Debug.Log("AddDamage: " + e);
                e.AddDamage(DamageAmount);
            }

            // Touch the back wall is the end of game session!
            if (collisionInfo.gameObject.CompareTag("BackWall"))
            {
                DestroyEffects();
                DestroyState();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DestroyState()
        {
            if (_projectileCollider == null)
            {
                Collider c = gameObject.GetComponent<Collider>();
                if (c != null)
                {
                    _projectileCollider = c;
                }
            }
            if (_projectileCollider)
                _projectileCollider.enabled = false;

            // initiate destroy procedure
            //Destroy(gameObject, destroyTime);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Spawn some destroy effects 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DestroyEffects()
        {
            //TODO: implement destroy effect
        }


        /// <summary>
        /// Moving with platform direction for a while time
        ///  This is helping to set move direction to projectile
        /// </summary>
        /// <param name="r"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        private IEnumerator StickingToPlatformEffect(Rigidbody r, Vector3 normal)
        {
            // bumper effect to speed up ball
            _projectileRigidbody.velocity += r.velocity;

            yield return _stickingwait;

            // calculate with Vector3.Reflect
            _velocity = Vector3.Reflect(_lastDirection, normal);
            // bumper effect to speed up ball
            _projectileRigidbody.velocity = _velocity.normalized * speed;
        }
    }
}