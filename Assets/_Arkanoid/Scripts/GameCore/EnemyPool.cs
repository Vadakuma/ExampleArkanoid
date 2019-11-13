using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Enemies
{
    /// <summary>
    /// Using enemy pool from Level to generate the "brick walls"
    /// </summary>
    public class EnemyPool : IDisposable
    {
        private Dictionary<EnemyType, EnemyTypePool> _pools = new Dictionary<EnemyType, EnemyTypePool>();

        private struct EnemyTypePool : IDisposable
        {
            public Enemy         _original;
            private Queue<Enemy> _pool;
            private Transform    _parent;

            public EnemyTypePool(Enemy original, Queue<Enemy> pool, Transform parent)
            {
                _original = original;
                _pool     = pool;
                _parent   = parent;
            }

            public Enemy Push()
            {
                if (_pool.Count == 0)
                {
                    if(_original == null)
                    {
                        Debug.LogError("Something wrong with _original");
                        //TODO: return enemy dummy
                        return null;
                    }

                    var instance = GameObject.Instantiate(_original);
                    instance.transform.SetParent(_parent);
                    return instance.Activate();
                }

                return _pool.Dequeue().Activate();
            }

            public void Pull(Enemy e)
            {
                if (e == null)
                {
                    Debug.LogWarning("Something wrong in Pull");
                    return;
                }

                _pool.Enqueue(e.DeActivate());
            }

            public void Dispose()
            {
                while (_pool.Count > 0)
                {
                    var enemy = _pool.Dequeue();
                    if (enemy == null)
                        continue;

                    enemy.Dispose();
                    GameObject.Destroy(enemy.gameObject);
                }
            }
        }


        public EnemyPool(int size, List<Enemy> enemies, Transform parent)
        {
            RandFillPool(size, enemies, parent); // size from level settings
        }


        private void RandFillPool(int size, List<Enemy> enemies, Transform parent)
        {
            if(enemies.Count == 0 || parent == null)
            {
                Debug.LogWarning("Something wrong in RandFillPool");
                return;
            }

            foreach(var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                var pool = new Queue<Enemy>();
                for (int idx = 0; idx < size; ++idx)
                    pool.Enqueue(GameObject.Instantiate(enemy).DeActivate());

                if(!_pools.ContainsKey(enemy.EnemyType))
                    _pools.Add(enemy.EnemyType, new EnemyTypePool(enemy, pool, parent));
            }
        }


        /** */
        public Enemy GetRandomObject()
        {
            var rndenemytype = (EnemyType) UnityEngine.Random.Range(0, _pools.Count);

            if (_pools.TryGetValue(rndenemytype, out EnemyTypePool pool))
                return pool.Push();

            Debug.LogError("Returned Enemy is null!");
            // TODO: return empty Enemy dummy
            return null; 
        }

        /** */
        public void ReturnEnemy(Enemy e)
        {
            if(e == null)
            {
                Debug.LogError("Returned enemy id null!");
                return;
            }

            if(_pools.TryGetValue(e.EnemyType, out EnemyTypePool pool))
            {
                pool.Pull(e);
            }
            else
            {
                Debug.LogWarning("Something wrong in RandFillPool");
            }
        }

        public void Dispose()
        {
            foreach(var pair in _pools)
            {
                var pool = pair.Value;
                pool.Dispose();
            }
            _pools.Clear();
        }
    }
}
