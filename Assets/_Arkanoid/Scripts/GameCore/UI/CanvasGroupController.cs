using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanoid
{
    [RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupController : MonoBehaviour {

		private CanvasGroup 	_canvasGroup;
		private float 			_currcgAlpha;
		private float 			_needcgAlpha;
		private float 			_smoothness = 0.1f;
		private bool			_destroy = false;


        public delegate void 	    AlphaUpdate();
		private AlphaUpdate		    AlphaUpdater =() => { };
        private event AlphaUpdate   OnUpdateDoneEvent = () => { };

        private AlphaUpdate RestartDoneMethod = () => { };


        /// <summary>
        ///
        /// </summary>
        /// <param name="_cg">canvasGroup to control</param>
        /// <param name="_alpha"> current value in alpha filed</param>
        /// <param name="_smoothness">lerp smoothnnes</param>
        /// <param name="_destroy"> destroy CanvasGroupController at the end</param>
        public void Fade(CanvasGroup _cg, float _alpha, float _smoothness, bool _destroy)
		{
			_needcgAlpha = _alpha;
			_canvasGroup = _cg;
			this._smoothness  = _smoothness;
			this._destroy		= _destroy;

			if(_alpha > 0.5f) {
				_canvasGroup.interactable   = true;
				_canvasGroup.blocksRaycasts = true;
				AlphaUpdater  = new AlphaUpdate(AlphaTo_1);
			} else {
				_canvasGroup.interactable   = false;
				_canvasGroup.blocksRaycasts = false;
				AlphaUpdater  = new AlphaUpdate(AlphaTo_0);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_cg">canvasGroup to control</param>
        /// <param name="_alpha">current value in alpha filed</param>
        /// <param name="_smoothness">lerp smoothnnes</param>
        /// <param name="_destroy"></param>
        /// <param name="_restartDoneMethod">destroy CanvasGroupController at the end</param>
        public void Fade(CanvasGroup _cg, float _alpha, float _smoothness, bool _destroy, AlphaUpdate _restartDoneMethod)
        {
            _needcgAlpha = _alpha;
            _canvasGroup = _cg;
            this._smoothness = _smoothness;
            this._destroy = _destroy;

            if (_alpha > 0.5f)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                AlphaUpdater = new AlphaUpdate(AlphaTo_1);
            }
            else
            {
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                AlphaUpdater = new AlphaUpdate(AlphaTo_0);
            }
            RestartDoneMethod = _restartDoneMethod;
            OnUpdateDoneEvent += RestartDoneMethod;
        }

        /// <summary>
        /// apply some canvas group params
        /// </summary>
        /// <param name="_cg">canvasGroup to control</param>
        /// <param name="_alpha">current value in alpha filed</param>
        /// <param name="_blockraycasts"></param>
        /// <param name="_interactable"></param>
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
			_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _needcgAlpha, _smoothness);
			if(_canvasGroup.alpha < 0.05f) {
				StopUpdater();
			}
		}

		private void AlphaTo_1()
		{
			_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _needcgAlpha, _smoothness);
			if(_needcgAlpha - _canvasGroup.alpha < 0.05f) {
				StopUpdater();
			}
		}

		private void StopUpdater()
		{
			_canvasGroup.alpha = _needcgAlpha;
			if(!_destroy)
				AlphaUpdater = () => { };
            else
				Destroy(this);

            OnUpdateDoneEvent();

            // stop listening
            OnUpdateDoneEvent -= RestartDoneMethod;
            RestartDoneMethod = () => { };
        }
	}
}