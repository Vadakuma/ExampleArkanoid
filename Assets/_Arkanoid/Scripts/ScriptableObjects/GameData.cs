using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Arkanoid
{
    /// <summary>
    /// General Game Settings
    /// Level settings
    /// Player Data containers
    /// Lose conditions
    /// </summary>
    public class GameData : ScriptableObject
    {
        //TODO: implement normal singletone 
        private static GameData _instance;
        public static GameData Instance { get { return _instance; } private set { _instance = value; } }

        [SerializeField]
        protected List<LevelSettings>       levelSettings   = new List<LevelSettings>();
        [SerializeField]
        protected GameSettings              gameSettings    = new GameSettings();
        [SerializeField]
        protected SessionCondition          loseCondition   = new SessionCondition(); 
        // Container with results just info container
        public List<PlayerData>             SessionsResults = new List<PlayerData>();


        /// <summary>
        /// Condition about win or lose stuff
        /// </summary>
        public SessionCondition GetLoseCondition
        {
            get
            {
                return loseCondition;
            }
        }

        /// <summary>
        /// Params like a score during in the game session
        /// </summary>
        protected static PlayerData sessionPlayerData = new PlayerData();

        public static PlayerData SessionPlayerData
        {
            get
            {
                return sessionPlayerData;
            }
            private set { sessionPlayerData = value; }
        }

        /// <summary>
        /// for level generator
        /// </summary>
        public LevelSettings GetRandomLevelSettings
        {
            get
            {
                if (levelSettings.Count > 0)
                    return levelSettings[Random.Range(0, levelSettings.Count)];
                else
                    return new LevelSettings(); // bad
            }
            private set { }
        }


        /// <summary>
        /// check and add
        /// </summary>
        /// <param name="pd"></param>
        public void AddSessionsResult(PlayerData pd)
        {
            // accumulate total score
            if(SessionsResults.Count > 0)
                pd.TotalScore += SessionsResults[SessionsResults.Count - 1].TotalScore + pd.Score;
            else
                pd.TotalScore = pd.Score;

            SessionsResults.Add(pd);
        }

        /// <summary>
        /// add score duting in the game session
        /// </summary>
        /// <param name="score"></param>
        public static void ApplyScore(int score)
        {
            sessionPlayerData.Score += score;
        }

        /// <summary>
        /// reset session score
        /// </summary>
        public static void ResetScore()
        {
            // set in score last win result
            sessionPlayerData.Score = 0;
        }

        public static void ResetRoundCounter()
        {
            sessionPlayerData.MaxRoundCounter = 0;
        }

        /// <summary>
        /// reset all previus scores, before start a new game
        /// </summary>
        public void ResetAllSavedScoreData()
        {
            SessionsResults = new List<PlayerData>();
            ResetScore();
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


    // level scene generate settings
    [System.Serializable]
    public struct LevelSettings
    {
        public int      enemyAmount;            // how much enemies will be generate on the level
        public int      rows;                   // rows in the brick-enemy wall
        public int      columns;                // columns in the brick-enemy wall
        //public Vector2  movementShiftLimits;    //
        public Vector2  cell;                   // cell is the size place for the one brick-enemy
        //public bool     useAbilitySpawner;      // don't working at the moment
    }

    // don't working at the moment
    [System.Serializable]
    public struct GameSettings
    {
        public int maxSavedSessions;
    }

    // session result plaer data
    [System.Serializable] 
    public class PlayerData
    {
        [SerializeField]
        protected int totalScore;
        [SerializeField]
        protected int maxRoundCounter;

        private int sessionScore;


        public int TotalScore { get { return totalScore; } set { totalScore = value; } }
        public int Score { get { return sessionScore; } set { sessionScore = value; } }
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