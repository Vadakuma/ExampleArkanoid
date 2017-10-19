using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(CanvasGroupController))]
    public class MainMenu : MonoBehaviour
    {

        private SceneLoad sceneLoad = new SceneLoad();
        // UICanvasContainer allow to easier make fadeInOut staff
        private UICanvasContainer uICanvasContainer = new UICanvasContainer();

        // Use this for initialization
        void Start()
        {
            uICanvasContainer = new UICanvasContainer(gameObject.GetComponent<CanvasGroup>(),
                gameObject.GetComponent<CanvasGroupController>());

            uICanvasContainer.UpdateCanvasGroup(0, true, true); // fast dark in
            // and
            uICanvasContainer.Fade(1, 0.05f, false); // slow dark out
        }

        // Update is called once per frame
        void Update()   {        }

        /** */
        public void GoToScene(int index)
        {
            StartCoroutine(sceneLoad.AsyncLoad(index));
            // Restart fade effect with special listener ActivateScene
            uICanvasContainer.cgcontroller.Fade(uICanvasContainer.cgroup, 0.0f, 0.05f, false, OnFadeGoToScene);
        }

        /** */
        public void ApplicationQuit()
        {
            // Restart fade effect with special listener ActivateScene
            uICanvasContainer.cgcontroller.Fade(uICanvasContainer.cgroup, 0.0f, 0.05f, false, OnFadeQuit);
        }

        /** */
        private void OnFadeGoToScene()
        {
            sceneLoad.Activation(); // loaded scene activation
        }

        /** */
        private void OnFadeQuit()
        {
            Application.Quit(); ;
        }
    }
}