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
        protected string stateName = "InBaseGameState";

        public string StateName { get { return stateName; } protected set { stateName = value; } }

        public InBaseGameState(IGameState prev)
        {
            SetStateName(); // set state name first of all

            if (prev != null)
            {
                prev.Disable();
            }

            if (InGameUI.Instance)
                InGameUI.Instance.SpawnPopup(StateName);

            SetDeActiveGameActors();
        }
        public virtual void Disable()
        {
            if (InGameUI.Instance)
                InGameUI.Instance.ClosePopup(StateName);
        }

        public virtual void Update() { }

        /** */
        public void SetActiveGameActors()
        {
            // update value
            Ball.Instance.StartMoving();
            EnemyManager.Instance.UnPauseEnemies();
            PickUpManager.Instance.UnPauseSpawn();
            Platform.Instance.GoToActiveState();
        }

        protected virtual void SetStateName() { }

        /** */
        public void SetDeActiveGameActors()
        {
            // update value
            Ball.Instance.StopMoving();
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
        private SessionCondition loseCondition;  
        // cach for win conditions checking
        private GameObject  projectileLink;
        private int         enemiesAmount;
        private int         platformHealh;

        public InPlayState(IGameState prev) : base(prev)
        {
            StateName = "InPlayState";

            if (Ball.Instance)
                Ball.Instance.StartMoving();
           

            // activate checking win conditions
            if (GameState.Instance)
            {
                if (GameState.gameData != null)
                {
                    loseCondition = GameState.gameData.GetLoseCondition;
                    active = true;
                    GameState.Instance.StartCoroutine(CheckConditions());
                }
                else
                {
                    Debug.Log("Problem with GameData");
                }
            }
            else
            {
                Debug.Log("Problem with GameState.Instance");
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
                //Debug.Log(CheckWinConditions());
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
            if (Ball.Instance)
                projectileLink = Ball.Instance.gameObject;
            else
                projectileLink = null;
            enemiesAmount = EnemyManager.Instance.GetActiveEnemiesAmount();
            platformHealh = Platform.Instance.GetPlatformSettings.Health;
           
            // SessionCondition from GameData
            return loseCondition.CheckConditions(projectileLink, enemiesAmount, platformHealh);
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
        public InPauseState(IGameState prev) : base(prev)  {  }
        protected override void SetStateName() { StateName = "InPauseState"; }
    }

    /** */
    public class InReturnToMenuState : InBaseGameState
    {
        public InReturnToMenuState(IGameState prev) : base(prev) {  }
        protected override void SetStateName() { StateName = "InReturnToMenuState"; }
    }

    /** */
    public class InWaitState : InBaseGameState
    {
        public InWaitState(IGameState prev) : base(prev) { }
        protected override void SetStateName() { StateName = "InWaitState"; }
    }

    /** */
    public class InWinState : InBaseGameState
    {
        public InWinState(IGameState prev) : base(prev)  {  }
        protected override void SetStateName() { StateName = "InWinState"; }
    }

    /** */
    public class InRestartLevelState : InBaseGameState
    {
        private int status = -1;

        public InRestartLevelState(IGameState prev) : base(prev) {

            Level.Instance.RestartLastLevel();
            Platform.Instance.GoToResetState();
            SetActiveGameActors();
            status = 0;
        }
        protected override void SetStateName() { StateName = "InRestartLevelState"; }

        public override void Update()
        {
            // should wait for a while when the constructor work wiil be done
            if (status > -1)
            {
                if (status == 0)
                    GameState.Instance.GoToPlayState();
                status = -1;
            }
        }
    }

    /** */
    public class InLoseState : InBaseGameState
    {
        public InLoseState(IGameState prev) : base(prev) {  }
        protected override void SetStateName() { StateName = "InLoseState"; }
    }

    /** */
    public class InGenerateLevelState : InBaseGameState
    {
        private int status = -1;

        public InGenerateLevelState(IGameState prev) : base(prev)
        {
            // trying to generate level and waiting of reaction from user
            status = TryToGenerateLevel();
        }
        protected override void SetStateName() { StateName = "InGenerateLevelState"; }

        /** Go to waiting state for if success*/
        private void OnLevelGeneratedSuccess()
        {
            GameState.Instance.GoToWaitState();
        }

        /** Go to waiting state for if fail*/
        private void OnLevelGeneratedFail()
        {
            GameState.Instance.GoToWaitState();
        }

        public override void Update() {
            // should wait for a while when the constructor work wiil be done
            if (status > -1) {
                if (status == 1)
                    OnLevelGeneratedSuccess();
                else if (status == 0)
                    OnLevelGeneratedFail();
                status = -1;
            }   
        }


        /** */
        private int TryToGenerateLevel()
        {
            int success = -1;
            try
            {
                Level.Instance.GenerateLevel();
                success = 1;
            } catch {
                Debug.Log("generate level state fail!");
                success = 0;
            }

            return success;
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
        private /*static*/ IGameState state;
        public /*static*/ IGameState  State { get { return state; } private set { state = value;
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
           // Debug.Log("|||" + state.ToString());

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
        public void GoToRestartLevelState()
        {
            state = new InRestartLevelState(state);
        }

        public void GoToGenerateLevelState()
        {
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