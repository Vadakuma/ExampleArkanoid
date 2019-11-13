using Arkanoid.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /// <summary>
    /// For weapon firing. EXAMPLE! Fast implementation!
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected float         speed = 1.0f;
        [SerializeField]
        protected float         lifeTime = 10.0f;
        [SerializeField]
        protected int           damageAmount = 3;

        private bool        _isActive = false;

        // Use this for initialization
        private void Start()
        {
            Destroy(gameObject, lifeTime);
            _isActive = true;
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
            if (_isActive)
            {
                // move on last calculate direction
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }

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