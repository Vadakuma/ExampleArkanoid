using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    /** Unique projectile actor as easy bouncy ball.
     * */
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
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

        private static Projectile _instance;
        public static Projectile Instance { get { return _instance; } private set { _instance = value; } }


        void Awake()
        {
            Instance = this;
            projectileRigidbody = GetComponent<Rigidbody>();

            initpos = transform.position;
        }

        // Use this for initialization
        void Start()
        {
              // found spawn point and set position


            // initial inpulse
            //StartMoving();
            StopMoving();
        }


        void FixedUpdate()
        {
            direction = projectileRigidbody.velocity;
        }

        // Update is called once per frame
        void Update() {     }

        /** Set to initial position,reactivate components, stop moving  */
        public void ResetProjectile()
        {
            gameObject.SetActive(true);
            transform.position = initpos;
            if (projectileCollider)
                projectileCollider.enabled = true;
            StopMoving();
        }

        /** */
        public void StopMoving()
        {
            if (projectileRigidbody != null)
            {
                lastDirection = projectileRigidbody.velocity;
                projectileRigidbody.velocity = Vector3.zero;
            }
        }

        /** */
        public void StartMoving()
        {
            if(lastDirection != Vector3.zero)
                projectileRigidbody.velocity = lastDirection;
            else
                projectileRigidbody.velocity = Vector3.Reflect(initdir, Vector3.zero).normalized * speed;
        }

        /** Colliding for scene object - wall or player platform*/
        void OnCollisionEnter(Collision collisionInfo)
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

        /** */
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

        /* Spawn some destroy effects **/
        private void DestroyEffects()
        {

        }

        /** Moving with platform direction for a while time
         * This is helping to set move direction to projectile 
         */
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