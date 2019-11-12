using Arkanoid.Abilities;
using Arkanoid.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.LevelGenerator
{
    /// <summary>
    /// Reset and Generate new levels
    /// </summary>
    public class Level : MonoBehaviour
    {
        [SerializeField, Tooltip("Set level ball prefab")]
        protected GameObject            ball;
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


        public void GenerateLevel()
        {
            // levelGenerator = new SimpleGenerator(10, enemyPool, enemySpawnPoint);
            levelGenerator = new SimpleGenerator();
            levelSettings = levelGenerator.Generate();

            // spawn Ball
            SpawnBall();
        }

        /// <summary>
        ///  setup brick by lase level settings
        /// </summary>
        public void RestartLastLevel()
        {
            // levelGenerator = new SimpleGenerator(10, enemyPool, enemySpawnPoint);
            if (levelGenerator == null)
                levelGenerator = new SimpleGenerator();

            levelGenerator.ResetLevel(levelSettings);

            // reset settings in Ball or spawn a new one
            SpawnBall();
        }

        private void SpawnBall()
        {
            if(Ball.Instance != null)
            {
                // stop and reset position
                Ball.Instance.ResetProjectile();
            }
            else
            {
                // check and spawn
                if (ball && ball.GetComponent<Ball>() != null)
                {
                    Instantiate(ball);
                }
            }
        }

        /** */
        public  Ability GetRandLevelAbility()
        {
            int rand;
            rand = Random.Range(0, abilities.Count);
            return abilities[rand];
        }
    }

}