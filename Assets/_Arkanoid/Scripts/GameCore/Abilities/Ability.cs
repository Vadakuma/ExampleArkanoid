using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.Abilities
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
        public float lifetime = 0;
    }


    /***********************************
     * Abilities
     * ********************************/
    public class Ability : MonoBehaviour, IAbility
    {
        // short link to the platform settings
        protected PlatformSettings ps;

        public virtual void Apply(Platform p) { }
    }
}