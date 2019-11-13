using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class MainMenu : MonoBehaviour
    {
        private SceneLoad sceneLoad = new SceneLoad();
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer _uiCanvasContainer = new UICanvasContainer();

        // Use this for initialization
        private void Start()
        {
            _uiCanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
                gameObject.GetComponent<CanvasGroupController>());

            _uiCanvasContainer.UpdateCanvasGroup(0, true, true); // fast dark in
            // and
            _uiCanvasContainer.Fade(1, 0.05f, false); // slow dark out
        }


        public void GoToScene(int index)
        {
            StartCoroutine(sceneLoad.AsyncLoad(index));
            // Restart fade effect with special listener ActivateScene
            _uiCanvasContainer.cgcontroller.Fade(_uiCanvasContainer.cgroup, 0.0f, 0.05f, false, OnFadeGoToScene);
        }

        public void ApplicationQuit()
        {
            // Restart fade effect with special listener ActivateScene
            _uiCanvasContainer.cgcontroller.Fade(_uiCanvasContainer.cgroup, 0.0f, 0.05f, false, OnFadeQuit);
        }

       
        private void OnFadeGoToScene()
        {
            sceneLoad.Activation(); // loaded scene activation
        }

        private void OnFadeQuit()
        {
            Application.Quit();
        }
    }
}