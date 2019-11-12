using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    /** */
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class GUIManager : MonoBehaviour
    {
        // container with all in level menus. UICanvasContainer allow to easy fadeInOut staff
        private Dictionary<string, UICanvasContainer> menus = new Dictionary<string, UICanvasContainer>();


        private static GUIManager _instance;
        public  static GUIManager Instance { get { return _instance; } private set { _instance = value; } }
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer   menu;
        private SceneLoad           sceneLoad = new SceneLoad();
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer gUIManagerCanvasContainer;

        // Use this for initialization
        void Awake()
        {
            Instance = this;
            gUIManagerCanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
                gameObject.GetComponent<CanvasGroupController>());

            gUIManagerCanvasContainer.UpdateCanvasGroup(1, true, true); // fast dark in
        }

        void Start()
        {
            // start fade out
            gUIManagerCanvasContainer.Fade(0, 0.05f, false); // fast dark out
        }

        /**collecting all menus from scene */
        public void AddMenu(string key, UICanvasContainer uicc)
        {
            menus.Add(key, uicc);
        }

        /** Activate menu by key */
        public void SpawnMenu(string key)
        {
            //Debug.Log("SpawnPopup: " + key);
            menus.TryGetValue(key, out menu);
            if(menu != null)
            {
                menu.Fade(1.0f, 0.1f, false);
            }
        }

        /** Deactivate menu by key */
        public void CloseMenu(string key)
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
            gUIManagerCanvasContainer.cgcontroller.Fade(gUIManagerCanvasContainer.cgroup, 1.0f, 0.05f, false, OnFadeGoToScene);
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
            if (GUIManager.Instance)
                GUIManager.Instance.SpawnMenu(key);
        }
    }
}