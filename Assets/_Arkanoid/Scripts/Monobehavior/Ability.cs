using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    public interface IAbility
    {
        void Apply(Platform p);
    }
    /***********************************
     * Ability settings
     * ********************************/

    [System.Serializable]
    public class AbilitySettings
    {
        public float        lifetime = 0;
    }


    /***********************************
     * Abilities
     * ********************************/
    public class Ability : MonoBehaviour, IAbility
    {
        protected  PlatformSettings ps;

        public virtual void Apply(Platform p) { }
    }





}