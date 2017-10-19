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


        private Transform projtr;
        private Transform platformtr;
        private Vector3 dir = Vector3.zero;
        private float dist;
        private bool isActive = false;

        // Use this for initialization
        void Start()
        {
            platformtr = Platform.Instance.transform;
            projtr = gameObject.transform;

            Destroy(gameObject, lifeTime);
            isActive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isActive)
            {
                // move on last calculate direction
                projtr.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

        /** */
        public void SetActiveState(Transform point)
        {

        }

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