using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Set the empty object to flag the place of enemy generate")]
        protected Transform         enemySpawnPoint;
        [SerializeField, Tooltip("Set the parent object for enemies")]
        protected Transform         enemyParentObject;
        [SerializeField]
        protected List<GameObject>   enemies = new List<GameObject>();


        private static EnemyManager _instance;
        public static EnemyManager Instance { get { return _instance; } private set { _instance = value; } }

        private List<Enemy> activeEnemy = new List<Enemy>();

        private EnemyPool enemyPool;

        void Awake()
        {
            Instance = this;

            // creating and fill enemy pool by random enemies
            enemyPool = new EnemyPool();
            enemyPool.RandFillPool(10, enemies, enemyParentObject); // size from level settings
        }

        // Use this for initialization
        void Start() {   }


        public int GetActiveEnemiesAmount()
        {
            return activeEnemy.Count;
        }

        /** */
        public void UnPauseEnemies()
        {
            //Debug.Log("UnPauseEnemies");
            foreach (Enemy e in activeEnemy)
                e.ToActiveStateActivate();
        }

        /** */
        public void PauseEnemies()
        {
            //Debug.Log("PauseEnemies");
            foreach (Enemy e in activeEnemy)
                e.ToIdleStateActivate();
        }

        /** */
        public void RemoveFromActive(Enemy e)
        {
            activeEnemy.Remove(e);
        }

        /** example */
        public void GenerateEnemyPosition(LevelSettings ls)
        {
            int amount = ls.enemyAmount;

            if (amount > 0 && enemyPool != null)
            {
                activeEnemy.Clear();
                ls = GameState.gameData.GetRandomLevelSettings;
                Vector3 pos = Vector3.zero;
                Vector3 deltapos = Vector3.zero;

                int column = 0;
                int row = 0;
                Enemy e;
                while (amount > 0 && row < ls.rows)
                {
                    // calculate new positio by row and column values
                    deltapos.x = ls.cell.x * column;
                    deltapos.z = ls.cell.y * row;
                    pos = enemySpawnPoint.transform.position + deltapos;

                    e = SetObjectPostion(enemyPool.GetRandomObject(), pos);
                    e.Activate(enemyPool);
                    activeEnemy.Add(e);

                    column++;
                    if (ls.columns == column)
                    {
                        column = 0;
                        row++;
                    }
                    amount--;
                }
            }
        }

        private Enemy SetObjectPostion(Enemy e, Vector3 pos)
        {
            e.transform.position = pos;
            return e;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}