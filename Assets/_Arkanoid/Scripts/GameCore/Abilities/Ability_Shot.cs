using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Abilities
{
    [System.Serializable]
    public class ShotSettings : AbilitySettings
    {
        public GameObject fireprojectile;
    }

    [System.Serializable]
    public class Ability_Shot : Ability
    {
        [SerializeField]
        protected ShotSettings abilitySettings = new ShotSettings();

        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            GameObject.Instantiate(abilitySettings.fireprojectile, p.transform.position, p.transform.rotation);
        }
    }
}