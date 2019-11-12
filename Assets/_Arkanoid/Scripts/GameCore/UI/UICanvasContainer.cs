using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.UI
{
    // UICanvasContainer allow to easier make fadeInOut staff
    [System.Serializable]
    public class UICanvasContainer
    {
        // menu canvas group
        public CanvasGroup cgroup;
        // special controller for alpha from canvas group
        public CanvasGroupController cgcontroller;

        public UICanvasContainer() { }
        public UICanvasContainer(CanvasGroup cg, CanvasGroupController cgc)
        {
            cgroup = cg;
            cgcontroller = cgc;
        }

        /**
		* _alpha - current value in alpha field
		* _smoothness - lerp smoothnes on changing 
		* _destroy - destroy CanvasGroupController at the end
		*/
        public void Fade(float _alpha, float _smoothness, bool _destroy)
        {
            if (cgcontroller != null && cgroup != null)
                cgcontroller.Fade(cgroup, _alpha, _smoothness, _destroy);
        }


        /** apply some canvas group params. Fields from CanvasGroup component
         * _alpha -
         * _blockraycasts -
         * _interactable -
         */
        public void UpdateCanvasGroup(float _alpha, bool _blockraycasts, bool _interactable)
        {
            if (cgroup)
            {
                cgroup.alpha = _alpha;
                cgroup.blocksRaycasts = _blockraycasts;
                cgroup.interactable = _interactable;
            }
        }
    }
}
