using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /** 
     * 
     * */
    public class Level : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject>      enemies = new List<GameObject>();
        [SerializeField]
        protected List<GameObject>      pickups = new List<GameObject>();
        [SerializeField]
        protected List<Ability>         abilities = new List<Ability>();
        [SerializeField, Tooltip("Rand from 0 to value")]
        protected static float          pickupSpawnFrequency = 15.0f;
        [SerializeField, Tooltip("Set the empty object to flag the place of enemy generate")]
        protected Transform             enemySpawnPoint;
        [SerializeField, Tooltip(" ")]
        protected bool                  useAbilitySpawner = true;
        private EnemyPool               enemyPool;
        //private EnemyPool           pickupsPool; // a small amount of pickups

        private ILevelGenerator     levelGenerator;

        private WaitForSeconds      spawnwait = new WaitForSeconds(pickupSpawnFrequency);

        private GameObject          lastspawnedpickup;

        void Awake()
        {
            // fill enemy pool 

            // start pickup generator
            if (useAbilitySpawner)
            {
                if (pickups.Count > 0)
                    StartCoroutine(GeneratePickUps());
                else
                    Debug.Log("There are no any pickups.");
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            if (level != 0) // check that it is Game level by index
                GenerateLevel();
        }

        // Use this for initialization
        void Start() {   }



        public void GenerateLevel()
        {
            levelGenerator = new SimpleGenerator(25, enemyPool);
            levelGenerator.Generate();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /** Pickups generator
         * */
        private IEnumerator GeneratePickUps()
        {
            int rand;
            while (true)
            {
                rand = Random.Range(0, pickups.Count - 1);
                SpawnPickup(pickups[rand]);
                yield return spawnwait;
            }
        }


        private Ability GetRandAbility()
        { 
            int rand;
            rand = Random.Range(0, abilities.Count - 1);
            return abilities[rand];
        }

        /** */
        private void SpawnPickup(GameObject pickup)
        {
            lastspawnedpickup = Instantiate(pickup);
            // setup ability container
            Pickup pu = lastspawnedpickup.GetComponent<Pickup>();
            if(pu)
            {
                pu.SetAbility(GetRandAbility());
            }
        }
    }


    public interface ILevelGenerator
    {
        void Generate();
    }


    [System.Serializable]
    public class SimpleGenerator : ILevelGenerator
    {
        private int amount;
        private EnemyPool epool;

        public SimpleGenerator(int _amount, EnemyPool _pool)
        {
            amount = _amount;
            epool = _pool;
        }

        public void Generate()
        {
            if(amount > 0 && epool != null)
            {

            }
        }
    }

    [System.Serializable]
    public class SpawnPrefabGenerator : ILevelGenerator
    {
        private string prefabName;

        public SpawnPrefabGenerator(string prefabname)
        {
            prefabName = prefabname;
        }
        public void Generate()
        {
            // get and spawn gameobject from resources by prefabName
        }
    }

}