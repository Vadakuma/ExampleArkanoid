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
        private static int poolSize = 30;

        public void FillPool(int size)
        {

        }

        public void ReturnEnemy(Enemy e)
        {
            e.DeActivate();
        }
    }
}
