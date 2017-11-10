using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /** Reset and Generate new levels

     * */
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

        /** setup btick by lase level settings*/
        public void RestartLastLevel()
        {
            // levelGenerator = new SimpleGenerator(10, enemyPool, enemySpawnPoint);
            if (levelGenerator == null)
                levelGenerator = new SimpleGenerator();

            levelGenerator.ResetLevel(levelSettings);

            // reset settings in Ball or spawn a new one
            SpawnBall();
        }

        /** */
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







    public interface ILevelGenerator
    {
        LevelSettings Generate();
        void ResetLevel(LevelSettings _ls);
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

        public void ResetLevel(LevelSettings _ls)
        {
            EnemyManager.Instance.GenerateEnemyPosition(_ls);
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

        public void ResetLevel(LevelSettings _ls) { }
    }

}