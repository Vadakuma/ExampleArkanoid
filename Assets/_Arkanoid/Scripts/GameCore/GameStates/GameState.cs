using Arkanoid.Enemies;
using Arkanoid.LevelGenerator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid.GameStates
{
    public class GameState : MonoBehaviour
    {
        public  static GameData gameData; // scriptable object with base settings of game and session settings

        //TODO: implement normal singletone 
        private static GameState _instance;
        public  static GameState Instance { get { return _instance; } private set { _instance = value; } }
        // main game state switcher
        private static IGameState  state;
        public static  IGameState  State { get { return state; } private set { state = value;   } }


        private void Awake()
        {
            Instance = this;

            // Get main game data
            gameData = (GameData)Resources.Load<GameData>("GameData");
            if (gameData == null)
                Debug.LogWarning("Load game data problem. Check Resources/GameData.asset");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            UpdateManager.SubscribeToUpdate(OnUpdate);
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
            UpdateManager.UnSubscribeFromUpdate(OnUpdate);
        }

        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0) // check that it is right Game level by index and generate scene stuff
                GoToGenerateLevelState();

            // reset all data from previous session
            if(gameData != null)
                gameData.ResetAllSavedScoreData();
        }

        private void OnUpdate()
        {
            state.Update();
        }

        /** 
         * STATE switches
         * */
        public void GoToRestartLevelState()
        {
            state = new RestartLevelState(state);
        }

        public void GoToGenerateLevelState()
        {
            state = new GenerateLevelState(state);
        }

        public void GoToPlayState()
        {
            state = new PlayState(state);
        }

        public void GoToPauseState()
        {
            state = new PauseState(state);
        }

        public void GoToReturnState()
        {
            state = new ReturnToMenuState(state);
        }

        public void GoToWaitState()
        {
            state = new WaitState(state);
        }

        public void GoToWinState()
        {
            state = new WinState(state);
        }

        public void GoToLoseState()
        {
            state = new LoseState(state);
        }
    }
}