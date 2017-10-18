using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    [RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupController : MonoBehaviour {

		private CanvasGroup 	canvasGroup;
		private float 			currcgAlpha;
		private float 			needcgAlpha;
		private float 			smoothness = 0.1f;
		private bool			destroy = false;


        public delegate void 	    AlphaUpdate();
		private AlphaUpdate		    AlphaUpdater =() => { };
        private event AlphaUpdate   OnUpdateDoneEvent = () => { };

        private AlphaUpdate RestartDoneMethod;


        // Use this for initialization
        void Start () {
			
		}

        /** 
		public CanvasGroup SetCanvasGroup
		{
			set {
				canvasGroup = value;
			}
		}*/

        /** _cg - canvasGroup to control
		* _alpha - current value in alpha filed
		* _smoothness - Lerp smoothnes on changing 
		* _destroy - destroy CanvasGroupController at the end
		*/
        public void Restart(CanvasGroup _cg, float _alpha, float _smoothness, bool _destroy)
		{
			needcgAlpha = _alpha;
			canvasGroup = _cg;
			smoothness  = _smoothness;
			destroy		= _destroy;

			if(_alpha > 0.5f) {
				canvasGroup.interactable   = true;
				canvasGroup.blocksRaycasts = true;
				AlphaUpdater  = new AlphaUpdate(AlphaTo_1);
			} else {
				canvasGroup.interactable   = false;
				canvasGroup.blocksRaycasts = false;
				AlphaUpdater  = new AlphaUpdate(AlphaTo_0);
			}
		}


        /** _cg - canvasGroup to control
		* _alpha - current value in alpha filed
		* _smoothness - Lerp smoothnes on changing 
		* _destroy - destroy CanvasGroupController at the end
        * _restartDoneMethod - 
		*/
        public void Restart(CanvasGroup _cg, float _alpha, float _smoothness, bool _destroy, AlphaUpdate _restartDoneMethod)
        {
            needcgAlpha = _alpha;
            canvasGroup = _cg;
            smoothness = _smoothness;
            destroy = _destroy;

            if (_alpha > 0.5f)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                AlphaUpdater = new AlphaUpdate(AlphaTo_1);
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                AlphaUpdater = new AlphaUpdate(AlphaTo_0);
            }
            RestartDoneMethod = _restartDoneMethod;
            OnUpdateDoneEvent += RestartDoneMethod;
        }

        /** apply some canvas group params */
        public static void UpdateCanvasGroup(CanvasGroup _cg, float _alpha, bool _blockraycasts, bool _interactable) {
			if(_cg) {
				_cg.alpha 		   = _alpha;
				_cg.blocksRaycasts = _blockraycasts;
				_cg.interactable   = _interactable;
			}
		}

		// Update is called once per frame
		void Update () {
			AlphaUpdater();
		}

		private void AlphaTo_0()
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, needcgAlpha, smoothness);
			if(canvasGroup.alpha < 0.05f) {
				StopUpdater();
			}
		}

		private void AlphaTo_1()
		{
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, needcgAlpha, smoothness);
			if(needcgAlpha - canvasGroup.alpha < 0.05f) {
				StopUpdater();
			}
		}

		private void StopUpdater()
		{
			canvasGroup.alpha = needcgAlpha;
			if(!destroy)
				AlphaUpdater = () => { };
            else
				Destroy(this);

            OnUpdateDoneEvent();

            // stop listening
            OnUpdateDoneEvent -= RestartDoneMethod;
        }
	}
}