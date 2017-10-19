using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Arkanoid
{
    public class SceneLoad 
    {
        private AsyncOperation async;
    
        public IEnumerator AsyncLoad(int sceneindex)
        {
            try
            {
                async = SceneManager.LoadSceneAsync(sceneindex);
                async.allowSceneActivation = false;
                //Wait until the last operation fully loads to return anything
                while (async != null && !async.isDone)
                {
                    yield return null;
                }
            }
            finally { }
            /*catch
            {
                Debug.LogWarning("Error from async scene load");
            }*/
        }

        public void Load(int sceneindex)
        {
            SceneManager.LoadScene(sceneindex, LoadSceneMode.Single);
        }

        public void Activation()
        {
            if(async != null)
                async.allowSceneActivation = true;
            async = null;
        }
    }
}