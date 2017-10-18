using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{

    [System.Serializable]
    public class HealingSettings : AbilitySettings
    {
        public int health = 100;
    }

    [System.Serializable]
    public class Ability_Healing : Ability
    {
        [SerializeField]
        protected HealingSettings abilitySettings = new HealingSettings();


        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            int mHealth = ps.MaxHealth;
            ps.Health = abilitySettings.health;

            //Debug.Log("Ability_Healing Apply");
        }
    }
}