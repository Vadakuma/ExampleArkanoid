using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /** For weapon firing. EXAMPLE! Fast implementation!
     */
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected float         speed = 1.0f;
        [SerializeField]
        protected float         lifeTime = 10.0f;
        [SerializeField]
        protected int           damageAmount = 3;

        private bool        isActive = false;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, lifeTime);
            isActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                // move on last calculate direction
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        /** */
        public void SetActiveState(Transform point) {      }

        private void OnTriggerEnter(Collider other)
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (other.gameObject.CompareTag("Enemy") || e != null)
            {
                e.AddDamage(damageAmount);
                damageAmount = 0; // only one time
                Destroy(gameObject);
            }
        }
    }
}