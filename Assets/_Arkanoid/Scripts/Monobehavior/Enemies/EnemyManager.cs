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
        [SerializeField,Tooltip("List with enemies prefabs for spawn in the pool")]
        protected List<GameObject>   enemies = new List<GameObject>();


        private static EnemyManager _instance;
        public static EnemyManager Instance { get { return _instance; } private set { _instance = value; } }

        // list with active enemies (not dead) on the scene
        private List<Enemy>     activeEnemy = new List<Enemy>();
        // simple enemy pool
        private EnemyPool       enemyPool;

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

        /** Make all enemies in Action state**/
        public void UnPauseEnemies()
        {
            foreach (Enemy e in activeEnemy)
                e.ToActiveStateActivate();
        }

        /** Make all enemies in Idle state*/
        public void PauseEnemies()
        {
            foreach (Enemy e in activeEnemy)
                e.ToIdleStateActivate();
        }

        /** If enemy  is dead - remove it from active list!*/
        public void RemoveFromActive(Enemy e)
        {
            activeEnemy.Remove(e);
        }

        /** Return all active enemies to the parent pool*/
        private void ReturnToPoolAll()
        {
            for (int idx = 0; idx < activeEnemy.Count; ++idx)
            {
                activeEnemy[idx].ToIdleStateActivate();
                activeEnemy[idx].ParentPool.ReturnEnemy(activeEnemy[idx]);
            }
            activeEnemy.Clear();
        }

        /** example */
        public void GenerateEnemyPosition(LevelSettings ls)
        {
            int amount = ls.enemyAmount;

            if (amount > 0 && enemyPool != null)
            {
                // before spawn we will return all enemies at this pool
                ReturnToPoolAll();

                Vector3 pos = Vector3.zero;
                Vector3 deltapos = Vector3.zero;
                Vector3 basepos = enemySpawnPoint.transform.position;
                // special shift for spwan position.
                // With this shif enemySpawnPoint will be in the middle of the objects row. 
                basepos.x -= ((ls.columns) * ls.cell.x) / 2 - ls.cell.x / 2;

                int column = 0;
                int row = 0;
                Enemy e;
                while (amount > 0 && row < ls.rows)
                {
                    // calculate new positio by row and column values
                    deltapos.x = ls.cell.x * column;
                    deltapos.z = -ls.cell.y * row;
                    pos = basepos + deltapos;

                    e = SetObjectPosition(enemyPool.GetRandomObject(), pos);
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

        private Enemy SetObjectPosition(Enemy e, Vector3 pos)
        {
            e.transform.localPosition = pos;
            return e;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}