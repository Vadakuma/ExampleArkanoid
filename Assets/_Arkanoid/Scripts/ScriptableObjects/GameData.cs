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
        private static GameData _instance;
        public static GameData Instance { get { return _instance; } private set { _instance = value; } }

        [SerializeField]
        protected List<LevelSettings>   levelSettings   = new List<LevelSettings>();
        [SerializeField]
        protected GameSettings          gameSettings    = new GameSettings();
        [SerializeField]
        protected SessionCondition      loseCondition   = new SessionCondition();
        

        // Container with results
        public List<PlayerData> SessionsResults = new List<PlayerData>();

        /** Condition about win or lose stuff*/
        public SessionCondition GetLoseCondition
        {
            get
            {
                return loseCondition;
            }
        }

        protected static PlayerData sessionPlayerData = new PlayerData();
        /** Params like a score during in the game session*/
        public static PlayerData SessionPlayerData
        {
            get
            {
                return sessionPlayerData;
            }
            private set { sessionPlayerData = value; }
        }

        /** add score duting in the game session*/
        public static void ApplyScore(int score)
        {
            sessionPlayerData.Score += score;
        }

        public static void ResetScore(int score)
        {
            sessionPlayerData.Score = 0;
        }


        /** for level generator*/
        public LevelSettings GetRandomLevelSettings {
            get {
                if (levelSettings.Count > 0)
                    return levelSettings[Random.Range(0, levelSettings.Count)];
                else
                    return new LevelSettings(); // bad
            }
            private set { }
        }

#if UNITY_EDITOR
            // creating asset from unity menu, but removing this from compilation build
        [MenuItem("Assets/Create/Arkanoid/GameData")]
        public static GameData CreateGameData()
        {
            if (_instance == null)
            {
                Instance = ScriptableObject.CreateInstance<GameData>();

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
        public int      enemyAmount;            // how much enemies will be generate on the level
        public int      rows;                   // rows in the brick-enemy wall
        public int      columns;                // columns in the brick-enemy wall
        public Vector2  movementShiftLimits;    //
        public Vector2  cell;                   // cell is the size place for the one brick-enemy
        public bool     useAbilitySpawner;      // don't working at the moment
    }

    [System.Serializable]
    public struct GameSettings
    {
        public int maxSavedSessions;
    }

    [System.Serializable] 
    public class PlayerData
    {
        private int score;
        private int maxRoundCounter;

        public int Score { get { return score; } set { score = value; } }
        public int MaxRoundCounter { get { return maxRoundCounter; } set { maxRoundCounter = value; } }

        public PlayerData() {      }

        public PlayerData(int _score, int _rounds)
        {
            Score = _score;
            MaxRoundCounter = _rounds;
        }
    }


    /** */
    public enum GameStatus
    {
        GS_INGAME = 0,
        GS_LOSE = 1,
        GS_WIN = 2,
    };


    // condition for win or lose
    [System.Serializable]
    public class SessionCondition
    {
        [SerializeField]
        protected bool ifPlatfromHealthIsZero;
        [SerializeField]
        protected bool ifProjectileIsDestroed;
        [SerializeField]
        protected bool ifTimeOut;


        /** */
        public GameStatus CheckConditions(GameObject go, int amount, int health)
        {
            if (ifPlatfromHealthIsZero && health < 1)
                return GameStatus.GS_LOSE;
            if (ifProjectileIsDestroed && !go.activeSelf)
                return GameStatus.GS_LOSE;

            if (amount > 0)
                return GameStatus.GS_INGAME;
            else
                return GameStatus.GS_WIN;
        }

        /** */
        public GameStatus CheckConditions(GameObject go, int amount, int health, float timeout)
        {
            GameStatus result = CheckConditions(go, amount, health);
            if (ifTimeOut && result == GameStatus.GS_INGAME)
            {
                if (timeout > 0)
                    return GameStatus.GS_INGAME;
                else
                    return GameStatus.GS_LOSE;
            }

            return result;
        }
    }
}