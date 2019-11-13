using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.Enemies;

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

        private Rigidbody   projectileRigidbody;
        private Rigidbody   playerRigidbody;
        private Collider    projectileCollider;
        private Vector3     direction = Vector3.zero;
        private Vector3     lastDirection = Vector3.zero;
        private Vector3     velocity = Vector3.zero;

        private readonly Vector3 initdir = new Vector3(1, 1, 1);
        private          Vector3 initpos;


        private WaitForSeconds stickingwait = new WaitForSeconds(0.05f);

        private static Ball _instance;
        public static Ball Instance { get { return _instance; } private set { _instance = value; } }


        private void Awake()
        {
            Instance = this;
            projectileRigidbody = GetComponent<Rigidbody>();

            initpos = transform.position;
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
            direction = projectileRigidbody.velocity;
        }

        /// <summary>
        /// Set to initial position,reactivate components, stop moving
        /// </summary>
        public void ResetProjectile()
        {
            gameObject.SetActive(true);
            transform.position = initpos;
            lastDirection = initdir;
            if (projectileCollider)
                projectileCollider.enabled = true;
            StopMoving();
        }

        public void StopMoving()
        {
            if (projectileRigidbody != null)
            { 
                if (projectileRigidbody.velocity != Vector3.zero){
                    lastDirection = projectileRigidbody.velocity;
                    projectileRigidbody.velocity = Vector3.zero;
                }
            }
        }

        public void StartMoving()
        {
            if (projectileRigidbody.velocity == Vector3.zero)
            {
                if (lastDirection == Vector3.zero)
                    lastDirection = initdir;

                projectileRigidbody.velocity = lastDirection.normalized * speed;
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
                lastDirection = projectileRigidbody.velocity;
                if (playerRigidbody == null)
                {
                    playerRigidbody = collisionInfo.gameObject.GetComponent<Rigidbody>();
                }
                // about 0.15f sec
                StartCoroutine(StickingToPlatformEffect(playerRigidbody, cp.normal));
            }
            else
            {
                // calculate with Vector3.Reflect
                velocity = Vector3.Reflect(direction, cp.normal);
                // bounce effect to speed up ball
                projectileRigidbody.velocity = velocity.normalized * speed;
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

        private void DestroyState()
        {
            if (projectileCollider == null)
            {
                Collider c = gameObject.GetComponent<Collider>();
                if (c != null)
                {
                    projectileCollider = c;
                }
            }
            if (projectileCollider)
                projectileCollider.enabled = false;

            // initiate destroy procedure
            //Destroy(gameObject, destroyTime);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Spawn some destroy effects 
        /// </summary>
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
            projectileRigidbody.velocity += r.velocity;

            yield return stickingwait;

            // calculate with Vector3.Reflect
            velocity = Vector3.Reflect(lastDirection, normal);
            // bumper effect to speed up ball
            projectileRigidbody.velocity = velocity.normalized * speed;
        }
    }
}