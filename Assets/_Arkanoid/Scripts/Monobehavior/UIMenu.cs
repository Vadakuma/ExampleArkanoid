using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arkanoid
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField]
        protected GameObject mainMenu;
        [SerializeField]
        protected GameObject pauseMenu;
        [SerializeField]
        protected GameObject ingameMenu;
        [SerializeField]
        protected GameObject loseMenu;
        [SerializeField]
        protected GameObject winMenu;
        [SerializeField]
        protected GameObject waitMenu;

        private static UIMenu _instance;
        public static  UIMenu Instance { get { return _instance; } private set { _instance = value; } }

        public string levelName;
        AsyncOperation async;


        // Use this for initialization
        void Awake()
        {
            Instance = this;

            DontDestroyOnLoad(this);

        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }


        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
   
        }

        void Start()
        {
    
        }
        // Update is called once per frame
        void Update()
        {

        }


        IEnumerator load()
        {
            Debug.LogWarning("ASYNC LOAD STARTED - " +
               "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
            async = SceneManager.LoadSceneAsync(1);
            async.allowSceneActivation = false;
            yield return async;
        }

        /** */
        public void ReturnToMainMenu() { }

        /** */
        public void GoToGame() {
            StartCoroutine(load());
        
            CanvasGroupController cc = mainMenu.GetComponent<CanvasGroupController>();
            // Restart fade effect with special listener ActivateScene
            cc.Restart(mainMenu.GetComponent<CanvasGroup>(), 0.0f, 0.05f, false, ActivateScene); 
        }

        /** */
        public void ActivateScene()
        {
           async.allowSceneActivation = true;
        }

        /** */
        public void ApplicationQuit()
        {
            // need to save a data!

            Application.Quit();
        }

        public void SpawnPopup(IGameState gs)
        {
            Debug.Log("SpawnPopup: "+ gs.ToString());
            switch(gs.ToString())
            {
                case "InPlayState":
                   break;
                default:
                    break;
            }
        }

        public void ClosePopup(IGameState gs)
        {
            Debug.Log("ClosePopup: " + gs);
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
    }
}