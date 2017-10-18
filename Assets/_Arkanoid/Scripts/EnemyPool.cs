using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /**
     * Using enemy pool from Level to generate the "brick walls"
     * */
    public class EnemyPool
    {
        private static int          poolSize = 0;
        private List<Enemy>         enemies_ = new List<Enemy>();
        private List<GameObject>    prefabs_ = new List<GameObject>();

        private Transform parent;

        /** */
        public void RandFillPool(int size, List<GameObject> enemygos, Transform _parent)
        {
            // TODO : change conditions
            if(poolSize < size && enemies_.Count == 0 && enemygos.Count > 0)
            {
                poolSize = size;
                prefabs_ = enemygos;
                parent = _parent;

                int rand;
                for (int idx = 0; idx < poolSize; ++idx)
                {
                    rand = Random.Range(0, prefabs_.Count);
                    try
                    {
                        enemies_.Add(GameObject.Instantiate(prefabs_[rand]).GetComponent<Enemy>());

                        enemies_[enemies_.Count - 1].transform.SetParent(_parent);
                        enemies_[enemies_.Count - 1].DeActivate(this);
                    } catch
                    {
                        Debug.Log("Spawned game object is not enemy!");
                    }
                }
            }
        }

        /** */
        public Enemy GetRandomObject()
        {
            for (int idx = 0; idx < poolSize; ++idx)
            {
                if (enemies_[idx].gameObject.activeInHierarchy == false)
                {
                    return enemies_[idx];
                }
            }
            enemies_.Add(GameObject.Instantiate(prefabs_[Random.Range(0, prefabs_.Count)]).GetComponent<Enemy>());
            enemies_[enemies_.Count - 1].transform.SetParent(parent);
            poolSize++;
            return enemies_[enemies_.Count - 1];
        }

        /** */
        public void ReturnEnemy(Enemy e)
        {
            e.DeActivate(this);
        }
    }
}
