using Arkanoid.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.LevelGenerator
{
    /// <summary>
    /// example
    /// </summary>
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