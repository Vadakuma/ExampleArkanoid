using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /** */
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class InGameUI : MonoBehaviour
    {
        // container with all in level menus. UICanvasContainer allow to easy fadeInOut staff
        private Dictionary<string, UICanvasContainer> menus = new Dictionary<string, UICanvasContainer>();


        private static InGameUI _instance;
        public  static InGameUI Instance { get { return _instance; } private set { _instance = value; } }
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer   menu;
        private SceneLoad           sceneLoad = new SceneLoad();
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer   inGameUICanvasContainer;

        // Use this for initialization
        void Awake()
        {
            Instance = this;
            inGameUICanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
                gameObject.GetComponent<CanvasGroupController>());

            inGameUICanvasContainer.UpdateCanvasGroup(1, true, true); // fast dark in
        }

        void Start()
        {
            // start fade out
            inGameUICanvasContainer.Fade(0, 0.05f, false); // fast dark out
        }

        /**collecting all menus from scene */
        public void AddMenu(string key, UICanvasContainer uicc)
        {
            menus.Add(key, uicc);
        }

        public void SpawnPopup(string key)
        {
            //Debug.Log("SpawnPopup: " + key);
            menus.TryGetValue(key, out menu);
            if(menu != null)
            {
                menu.Fade(1.0f, 0.1f, false);
            }
        }

        public void ClosePopup(string key)
        {
            //Debug.Log("ClosePopup: " + key);
            menus.TryGetValue(key, out menu);
            if (menu != null)
            {
                menu.Fade(0.0f, 0.1f, false);
            }
        }

        /** */
        public void GoToScene(int index)
        {
            // need to save a data!
            StartCoroutine(sceneLoad.AsyncLoad(index));
            // Restart fade effect with special listener ActivateScene
            inGameUICanvasContainer.cgcontroller.Fade(inGameUICanvasContainer.cgroup, 1.0f, 0.05f, false, OnFadeGoToScene);
        }

        public void GoToMainMenu()
        {
            GoToScene(0);
        }

        /** */
        private void OnFadeGoToScene()
        {
            sceneLoad.Activation(); // loaded scene activation
        }


        public void OnMenuState(string key)
        {
            if (InGameUI.Instance)
                InGameUI.Instance.SpawnPopup(key);
        }


        void OnDisable()
        {

        }
    }
}