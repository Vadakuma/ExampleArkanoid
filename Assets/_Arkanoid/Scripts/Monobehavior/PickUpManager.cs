using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    public class PickUpManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Level settings parametr dublicate!!")]
        protected bool              useAbilitySpawner = true;
        [SerializeField, Tooltip("Rand from 0 to value")]
        protected float             pickupSpawnFrequency = 15.0f;
        [SerializeField, Tooltip("Prefabs for spawn")]
        protected List<GameObject>  pickups = new List<GameObject>();


        private static PickUpManager _instance;
        public  static PickUpManager Instance { get { return _instance; } private set { _instance = value; } }

        private WaitForSeconds       spawnwait;
        private GameObject           lastspawnedpickup;

        private static bool isActive = false;

        void Awake()
        {
            // for Coroutine
            spawnwait = new WaitForSeconds(pickupSpawnFrequency);

            Instance = this;

            // TODO: should get this parametr from level settings
            // start pickup generator
            if (useAbilitySpawner)
            {
                if (pickups.Count > 0)
                {
                    isActive = true;
                    StartCoroutine(GeneratePickUps());
                }
                else
                {
                    Debug.Log("There are no any pickups.");
                }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update() { }

        /** Pickups generator
        **/
        private IEnumerator GeneratePickUps()
        {
            int rand;
            while (isActive)
            {
                yield return spawnwait;
                if (isActive)
                {
                    rand = Random.Range(0, pickups.Count);
                    SpawnPickup(pickups[rand]);
                }
            }
        }


        /** */
        public void UnPauseSpawn()
        {
            isActive = true;
        }

        /** */
        public void PauseSpawn()
        {
            isActive = false;
        }

        /** */
        private void SpawnPickup(GameObject pickup)
        {
            lastspawnedpickup = Instantiate(pickup);
            // setup ability container
            Pickup pu = lastspawnedpickup.GetComponent<Pickup>();
            if (pu)
            {
                // set ability depends on level settings
                pu.SetAbility(Level.Instance.GetRandAbility());
            }
        }
    }
}