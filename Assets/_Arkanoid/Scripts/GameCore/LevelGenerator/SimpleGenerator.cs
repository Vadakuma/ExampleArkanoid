using Arkanoid.Enemies;
using Arkanoid.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.LevelGenerator
{
    /// <summary>
    /// Generate enemies for example TEMP
    /// </summary>
    public class SimpleGenerator : ILevelGenerator
    {
        public SimpleGenerator() { }

        /// <summary>
        /// Get some random level settings and generate enemy- brick simple wall
        ///  return random selected and used level settings
        /// </summary>
        /// <returns></returns>
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
}