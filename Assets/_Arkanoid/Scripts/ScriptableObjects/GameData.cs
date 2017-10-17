using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arkanoid
{
    public class GameData : ScriptableObject
    {
        public static GameData _instance;

        [SerializeField]
        protected LevelSettings     levelSettings = new LevelSettings();
        [SerializeField]
        protected GameSettings      gameSettings = new GameSettings();

        [SerializeField]
        protected List<PlayerData>  sessionsResults = new List<PlayerData>();


#if UNITY_EDITOR
        // creating asset from unity menu, but removing this from compilation build
        [MenuItem("Assets/Create/Arkanoid/GameData")]
        public static GameData CreateGameData()
        {
            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<GameData>();

                AssetDatabase.CreateAsset(_instance, "Assets/_Arkanoid/Resources/GameData.asset");
                AssetDatabase.SaveAssets();
                string relPath = AssetDatabase.GetAssetPath(_instance);
                EditorPrefs.SetString("ObjectPath", relPath);
                return _instance;
            }
            else
                return _instance;
        }
#endif
    }



    [System.Serializable]
    public struct LevelSettings
    {
        public int enemyAmount;
    }

    [System.Serializable]
    public struct GameSettings
    {
        public int maxSavedSessions;
    }

    [System.Serializable]
    public struct PlayerData
    {
        public int score;
    }
}