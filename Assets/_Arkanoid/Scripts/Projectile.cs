using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    /** Projectile is easy bounce ball.
     * */
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected int   damage;
        [SerializeField]
        protected float speed;

        public int      DamageAmount { get { return damage; } private set { } }
        public float    Speed { get { return speed; } private set { } }

        private Rigidbody   projectileRigidbody;
        private Rigidbody   playerRigidbody;

        private Vector3     direction = Vector3.zero;
        private Vector3     lastDirection = Vector3.zero;
        private Vector3     velocity = Vector3.zero;

        private readonly Vector3 initdir = new Vector3(1, 1, 1);

        private WaitForSeconds stickingwait = new WaitForSeconds(0.15f);

        // Use this for initialization
        void Start()
        {
            projectileRigidbody = GetComponent<Rigidbody>();

            // initial inpulse
            StartMoving();
        }


        void FixedUpdate()
        {
            direction = projectileRigidbody.velocity;
        }

        // Update is called once per frame
        void Update() {     }

        /** */
        public void StopMoving()
        {
            lastDirection = projectileRigidbody.velocity;
            projectileRigidbody.velocity = Vector3.zero;
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
            Debug.Log("Projectile OnCollisionEnter");
            ContactPoint cp = collisionInfo.contacts[0];

            // Checking and calculate the next direction to move
            if (collisionInfo.gameObject.CompareTag("Player"))
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
            Enemy e = collisionInfo.gameObject.GetComponent<Enemy>();
            if (collisionInfo.gameObject.CompareTag("Enemy") || e != null)
            {
                e.AddDamage(DamageAmount);
            }
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