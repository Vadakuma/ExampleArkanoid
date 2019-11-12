using Arkanoid.LevelGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.PickUps
{
    public class PickUpManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Level settings parametr dublicate!!")]
        private bool              _useAbilitySpawner = true;
        [SerializeField, Tooltip("Rand from 0 to value")]
        private float             _pickupSpawnFrequency = 10.0f;
        [SerializeField, Tooltip("Prefabs for spawn")]
        private List<GameObject>  _pickups = new List<GameObject>();

        //TODO: implement normal singletone 
        private static PickUpManager _instance;
        public  static PickUpManager Instance { get { return _instance; } private set { _instance = value; } }

        private WaitForSeconds       spawnwait;
        private GameObject           lastspawnedpickup;

        private static Coroutine Spawn;


        private void Awake()
        {
            // for Coroutine
            spawnwait = new WaitForSeconds(_pickupSpawnFrequency);

            Instance = this;

            // TODO: should get this parametr from level settings
            // start pickup generator
            if (_useAbilitySpawner)
            {
                if (_pickups.Count > 0)
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

        /// <summary>
        /// Pickups generator
        /// </summary>
        /// <returns></returns>
        private IEnumerator GeneratePickUps()
        {
            int rand;
            while (true)
            {
                yield return spawnwait;
                rand = Random.Range(0, _pickups.Count);
                SpawnPickup(rand);
            }
        }


        public void UnPauseSpawn()
        {
            PauseSpawn(); // stop at first
            Spawn = StartCoroutine(GeneratePickUps());
        }

        public void PauseSpawn()
        {
            if (Spawn != null)
                StopCoroutine(Spawn);
        }

        private void SpawnPickup(int rand)
        {
            lastspawnedpickup = Instantiate(_pickups[rand]);
            // setup ability container
            Pickup pu = lastspawnedpickup.GetComponent<Pickup>();
            if (pu)
            {
                // set ability depends on level settings
                pu.SetAbility(Level.Instance.GetRandLevelAbility());
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}