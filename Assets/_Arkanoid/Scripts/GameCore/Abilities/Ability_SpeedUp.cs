using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid.Abilities
{

    [System.Serializable]
    public class SpeedUpSettings : AbilitySettings
    {
        public float speedfactor = 1.5f;
    }

    [System.Serializable]
    public class Ability_SpeedUp : Ability
    {
        [SerializeField]
        protected SpeedUpSettings abilitySettings = new SpeedUpSettings();

        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            p.StartCoroutine(SpeedUpAbility());
        }

        /** */
        private IEnumerator SpeedUpAbility()
        {
            ps.SpeedUpFactor += abilitySettings.speedfactor;
            yield return new WaitForSeconds(abilitySettings.lifetime);
            ps.SpeedUpFactor -= abilitySettings.speedfactor;
        }
    }
}