using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class GUIManager : MonoBehaviour
    {
        // container with all in level menus. UICanvasContainer allow to easy fadeInOut staff
        private Dictionary<string, UICanvasContainer> _menus = new Dictionary<string, UICanvasContainer>();

        //TODO: implement normal singletone 
        private static GUIManager _instance;
        public  static GUIManager Instance { get { return _instance; } private set { _instance = value; } }

        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer   _menu;
        private SceneLoad           _sceneLoad = new SceneLoad();
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer   _gUIManagerCanvasContainer;

        // Use this for initialization
        private void Awake()
        {
            Instance = this;
            _gUIManagerCanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
                gameObject.GetComponent<CanvasGroupController>());

            _gUIManagerCanvasContainer.UpdateCanvasGroup(1, true, true); // fast dark in
        }

        private void Start()
        {
            // start fade out
            _gUIManagerCanvasContainer.Fade(0, 0.05f, false); // fast dark out
        }

        /// <summary>
        /// collecting all menus from scene
        /// </summary>
        /// <param name="key"></param>
        /// <param name="uicc"></param>
        public void AddMenu(string key, UICanvasContainer uicc)
        {
            _menus.Add(key, uicc);
        }

        /// <summary>
        /// Activate menu by key
        /// </summary>
        /// <param name="key"></param>
        public void SpawnMenu(string key)
        {
            if(_menus.TryGetValue(key, out _menu))
            {
                _menu.Fade(1.0f, 0.1f, false);
            }
        }

        /// <summary>
        /// Deactivate menu by key
        /// </summary>
        /// <param name="key"></param>
        public void CloseMenu(string key)
        {
            //Debug.Log("ClosePopup: " + key);
            if (_menus.TryGetValue(key, out _menu))
            {
                _menu.Fade(0.0f, 0.1f, false);
            }
        }

        public void GoToScene(int index)
        {
            // need to save a data!
            StartCoroutine(_sceneLoad.AsyncLoad(index));
            // Restart fade effect with special listener ActivateScene
            _gUIManagerCanvasContainer.cgcontroller.Fade(_gUIManagerCanvasContainer.cgroup, 1.0f, 0.05f, false, OnFadeGoToScene);
        }

        public void GoToMainMenu()
        {
            GoToScene(0);
        }

        private void OnFadeGoToScene()
        {
            _sceneLoad.Activation(); // loaded scene activation
        }


        public void OnMenuState(string key)
        {
            if (GUIManager.Instance)
                GUIManager.Instance.SpawnMenu(key);
        }
    }
}