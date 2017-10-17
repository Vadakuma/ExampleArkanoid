using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    [System.Serializable]
    public class PickUpSettings
    {
        [SerializeField]
        protected float lifeTime = 0;
    }

    /** */
    public class Pickup : MonoBehaviour
    {
        [SerializeField]
        protected IAbility abilitycontainer;

        public IAbility AbilityContainer { get { return abilitycontainer; } private set { } }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pickup Ability OnTriggerEnter");
            Platform player = other.gameObject.GetComponent<Platform>();
            if(player != null)
            {
                player.AddAbility(this);
            }
        }

        /** destroy staff */
        public void DeActivate()
        {
            Destroy(gameObject);
        }
    }
}