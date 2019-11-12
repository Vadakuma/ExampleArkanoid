using Arkanoid.LevelGenerator;
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

        private static Coroutine Spawn;


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
                    if (Spawn == null)
                        Spawn = StartCoroutine(GeneratePickUps());
                    else
                        UnPauseSpawn();
                }
                else
                {
                    Debug.Log("There are no any pickups.");
                }
            }
        }

        /** Pickups generator
        **/
        private IEnumerator GeneratePickUps()
        {
            int rand;
            while (true)
            {
                yield return spawnwait;
                rand = Random.Range(0, pickups.Count);
                SpawnPickup(rand);
            }
        }


        /** */
        public void UnPauseSpawn()
        {
            PauseSpawn(); // stop at first
            Spawn = StartCoroutine(GeneratePickUps());
        }

        /** */
        public void PauseSpawn()
        {
            if (Spawn != null)
                StopCoroutine(Spawn);
        }

        /** */
        private void SpawnPickup(int rand)
        {
            lastspawnedpickup = Instantiate(pickups[rand]);
            // setup ability container
            Pickup pu = lastspawnedpickup.GetComponent<Pickup>();
            if (pu)
            {
                // set ability depends on level settings
                pu.SetAbility(Level.Instance.GetRandLevelAbility());
            }
        }
    }
}