using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Set the empty object to flag the place of enemy generate")]
        private Transform         _enemySpawnPoint;
        [SerializeField, Tooltip("Set the parent object for enemies")]
        private Transform         _enemyParent;
        [SerializeField,Tooltip("List with enemies prefabs for spawn in the pool")]
        private List<Enemy>       _enemies = new List<Enemy>();


        private static EnemyManager _instance;
        public static EnemyManager Instance { get { return _instance; } private set { _instance = value; } }

        // list with active enemies (not dead) on the scene
        private List<Enemy>     _activeEnemy = new List<Enemy>();
        // simple enemy pool
        private EnemyPool       _enemyPool;

        private void Awake()
        {
            Instance = this;

            // creating and fill enemy pool by random enemies
            _enemyPool = new EnemyPool(10, _enemies, _enemyParent);
        }

        private void OnDestroy()
        {
            _enemyPool.Dispose();
        }

        public int GetActiveEnemiesAmount()
        {
            return _activeEnemy.Count;
        }

        /// <summary>
        /// Make all enemies in Action state
        /// </summary>
        public void UnPauseEnemies()
        {
            foreach (Enemy e in _activeEnemy)
                e.ToActiveStateActivate();
        }

        /// <summary>
        /// Make all enemies in Idle state
        /// </summary>
        public void PauseEnemies()
        {
            foreach (Enemy e in _activeEnemy)
                e.ToIdleStateActivate();
        }

        /// <summary>
        /// If enemy  is dead - remove it from active list!
        /// </summary>
        /// <param name="e"></param>
        public void RemoveFromActive(Enemy e)
        {
            _activeEnemy.Remove(e);
            _enemyPool.ReturnEnemy(e);
        }

        /// <summary>
        /// Return all active enemies to the parent pool
        /// </summary>
        private void ReturnToPoolAll()
        {
            for (int idx = 0; idx < _activeEnemy.Count; ++idx)
            {
                var enemy = _activeEnemy[idx];
                enemy.ToIdleStateActivate();
                enemy.ParentPool.ReturnEnemy(enemy);
            }
            _activeEnemy.Clear();
        }

        /// <summary>
        /// example
        /// </summary>
        /// <param name="ls"></param>
        public void GenerateEnemyPosition(LevelSettings ls)
        {
            int amount = ls.enemyAmount;

            if (amount == 0 || _enemyPool == null)
            {
                Debug.LogWarning("Something wrong in GenerateEnemyPosition");
                return;
            }

            // before spawn we will return all enemies at this pool
            ReturnToPoolAll();

            Vector3 pos = Vector3.zero;
            Vector3 deltapos = Vector3.zero;
            Vector3 basepos = _enemySpawnPoint.transform.position;
            // special shift for spwan position.
            // With this shif enemySpawnPoint will be in the middle of the objects row. 
            basepos.x -= ((ls.columns) * ls.cell.x) / 2 - ls.cell.x / 2;

            int column = 0;
            int row = 0;
            Enemy e;
            while (amount > 0 && row < ls.rows)
            {
                // calculate new position by row and column values
                deltapos.x = ls.cell.x * column;
                deltapos.z = -ls.cell.y * row;
                pos = basepos + deltapos;

                e = SetObjectPosition(_enemyPool.GetRandomObject(), pos);
                _activeEnemy.Add(e);

                column++;
                if (ls.columns == column)
                {
                    column = 0;
                    row++;
                }
                amount--;
            }
        }

        private static Enemy SetObjectPosition(Enemy e, Vector3 pos)
        {
            e.transform.localPosition = pos;
            return e;
        }
    }
}