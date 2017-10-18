﻿using System.Collections;
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
        protected GameObject            projectile;
        [SerializeField]
        protected List<Ability>         abilities = new List<Ability>();


        private LevelSettings       levelSettings = new LevelSettings();
        private ILevelGenerator     levelGenerator;

        private static Level _instance;
        public  static Level Instance { get { return _instance; } private set { _instance = value; } }


        public LevelSettings GetLevelSettings { get { return levelSettings; } private set { levelSettings = value; } }

        void Awake()
        {
            Instance = this;
        }

        // Use this for initialization
        void Start() {   }


        public void GenerateLevel()
        {
            // levelGenerator = new SimpleGenerator(10, enemyPool, enemySpawnPoint);
            levelGenerator = new SimpleGenerator();
            levelSettings = levelGenerator.Generate();

            // spawn projectile
           // SpawnProjectile();
        }


        private void SpawnProjectile()
        {
            if(Projectile.Instance != null)
            {
                // stop and reset position
                Projectile.Instance.SetInitialPosition();
                Projectile.Instance.StopMoving();
            }
            else
            {
                // check and spawn
                if (projectile && projectile.GetComponent<Projectile>() != null)
                {
                    Instantiate(projectile);
                    //Projectile.Instance.SetInitialPosition();
                }
            }
        }

        // Update is called once per frame
        void Update() {  }


        /** */
        public Ability GetRandAbility()
        { 
            int rand;
            rand = Random.Range(0, abilities.Count);
            return abilities[rand];
        }
    }


    public interface ILevelGenerator
    {
        LevelSettings Generate();
    }

    /** Generate enemies for example*/ // temp
    [System.Serializable]
    public class SimpleGenerator : ILevelGenerator
    {
        public SimpleGenerator()
        {

        }

        /** Get some random level settings and generate enemy- brick simple wall
        * return random selected and used level settings 
        */
        public LevelSettings Generate()
        {
            LevelSettings ls = GameState.gameData.GetRandomLevelSettings;
            EnemyManager.Instance.GenerateEnemyPosition(ls);

            return ls;
        }
    }

    /** example*/
    [System.Serializable]
    public class SpawnPrefabGenerator : ILevelGenerator
    {
        private string prefabName;

        public SpawnPrefabGenerator(string prefabname)
        {
            prefabName = prefabname;
        }
        public LevelSettings Generate()
        {
            LevelSettings ls = GameState.gameData.GetRandomLevelSettings;
            // get and spawn gameobject from resources by prefabName
            Debug.Log("Generate: " + prefabName);
            return ls;
        }
    }

}