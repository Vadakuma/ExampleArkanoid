using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    /***************************************************************************************************
     * Game STATES
     * *************************************************************************************************/
    public interface IGameState
    {
        void Disable();
        void Update();
    }

    /** */
    public abstract class InBaseGameState : IGameState
    {
        public InBaseGameState(IGameState prev)
        {
            if (prev != null)
            {
                prev.Disable();
            }
            

            if (UIMenu.Instance)
                UIMenu.Instance.SpawnPopup(this);

            SetDeActiveGameActors();
        }
        public virtual void Disable()
        {
            if (UIMenu.Instance)
                UIMenu.Instance.ClosePopup(this);
        }

        public virtual void Update() { }

        /** */
        public void SetActiveGameActors()
        {
            // update value
            Projectile.Instance.StartMoving();
            EnemyManager.Instance.UnPauseEnemies();
            PickUpManager.Instance.UnPauseSpawn();
            Platform.Instance.GoToActiveState();
        }

        /** */
        public void SetDeActiveGameActors()
        {
            // update value
            Projectile.Instance.StopMoving();
            EnemyManager.Instance.PauseEnemies();
            PickUpManager.Instance.PauseSpawn();
            Platform.Instance.GoToIdleState();
        }
    }

    /** */
    public class InPlayState : InBaseGameState
    {
        private WaitForSeconds waitbrforecheck = new WaitForSeconds(0.15f);
        private bool active = false;

        // cach for win conditions checking
        private GameObject  projectileLink;
        private int         enemiesAmount;
        private int         platformHealh;

        public InPlayState(IGameState prev) : base(prev)
        {
            if(Projectile.Instance)
                Projectile.Instance.StartMoving();
            // activate checking win conditions
            if (GameState.Instance)
            {
                active = true;
                GameState.Instance.StartCoroutine(CheckConditions());
            }

            SetActiveGameActors();
        }

        /** Check gameplay win conditions*/
        private IEnumerator CheckConditions()
        {
            while (active)
            {
                yield return waitbrforecheck;
                //
                switch (CheckWinConditions()) 
                {
                    case GameStatus.GS_LOSE:
                        if (GameState.Instance)
                            GameState.Instance.GoToLoseState();
                        break;
                    case GameStatus.GS_WIN:
                        if (GameState.Instance)
                            GameState.Instance.GoToWinState();
                        break;
                    default:
                        break;
                }
            }
        }

        /** */
        public GameStatus CheckWinConditions()
        {
            // update value
            projectileLink = Projectile.Instance.gameObject;
            enemiesAmount = EnemyManager.Instance.GetActiveEnemiesAmount();
            platformHealh = Platform.Instance.GetPlatformSettings.Health;
            // SessionCondition from GameData
            return SessionCondition.CheckWinConditions(projectileLink, enemiesAmount, platformHealh);
        }

        /** */
        public override void Disable()
        {
            base.Disable();
            active = false; // deactivate Coroutine
        }
    }

    /** */
    public class InPauseState : InBaseGameState
    {
        public InPauseState(IGameState prev) : base(prev)  {   }
    }

    /** */
    public class InReturnToMenuState : InBaseGameState
    {
        public InReturnToMenuState(IGameState prev) : base(prev) {  }
    }

    /** */
    public class InWaitState : InBaseGameState
    {
        public InWaitState(IGameState prev) : base(prev) {   }
    }

    /** */
    public class InWinState : InBaseGameState
    {
        public InWinState(IGameState prev) : base(prev)  {  }
    }

    /** */
    public class InLoseState : InBaseGameState
    {
        public InLoseState(IGameState prev) : base(prev) { }
    }

    /** */
    public class InGenerateLevelState : InBaseGameState
    {
        private bool isDone = false;

        public InGenerateLevelState(IGameState prev) : base(prev)
        {
            // trying to generate level and waiting of reaction from user
            isDone = TryToGenerateLevel();
        }

        /** Go to waiting state for */
        private void OnLevelGenerated()
        {
            GameState.Instance.GoToWaitState();
        }

        public override void Update() {
            // should wait for a while when the constructor work wiil be done
            if (isDone) {
                isDone = false;
                OnLevelGenerated();
            }   
        }

        /** */
        private bool TryToGenerateLevel()
        {
            bool succsess = false;
            try
            {
                Level.Instance.GenerateLevel();
                succsess = true;
            } catch {

            }

            return succsess;
        }
    }



    /***************************************************************************************************
     * MAIN GAME STATE MONO
     * *************************************************************************************************/
    //[System.Serializable]
    public class GameState : MonoBehaviour
    {
        public  static GameData gameData; // scriptable object with base settings of game and session settings

        private static GameState _instance;
        public  static GameState Instance { get { return _instance; } private set { _instance = value; } }
        // main game state switcher
        private static IGameState state;
        public static IGameState State { get { return state; } private set { state = value;
               // Debug.Log("Set: " + value);
            } }


        void Awake()
        {
            Instance = this;

            // Get main game data
            gameData = (GameData)Resources.Load<GameData>("GameData");
            if (gameData == null)
                Debug.LogWarning("Load game data problem. Check Resources/GameData.asset");
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }


        /** */
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0) // check that it is right Game level by index
                GoToGenerateLevelState();
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update()  {
            state.Update();
            Debug.Log("|||" + state.ToString());

            if (Input.GetKeyDown(KeyCode.Space))
                GoToWaitState();
            if (Input.GetKeyDown(KeyCode.P))
                GoToPlayState();
            if (Input.GetKeyDown(KeyCode.S))
                GoToPauseState();
        }


        /** 
         * STATE switches
         * */

        public void GoToGenerateLevelState()
        {
            //lock(State) { State = new InGenerateLevelState(state); }
            state = new InGenerateLevelState(state);
        }

        public void GoToPlayState()
        {
            state = new InPlayState(state);
        }

        public void GoToPauseState()
        {
            state = new InPauseState(state);
        }

        public void GoToReturnState()
        {
            state = new InReturnToMenuState(state);
        }

        public void GoToWaitState()
        {
            state = new InWaitState(state);
        }

        public void GoToWinState()
        {
            state = new InWinState(state);
        }

        public void GoToLoseState()
        {
            state = new InLoseState(state);
        }


        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }
}