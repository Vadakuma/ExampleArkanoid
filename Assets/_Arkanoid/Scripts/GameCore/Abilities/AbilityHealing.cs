﻿using Arkanoid.PlayerPlatform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Abilities
{

    [System.Serializable]
    public class HealingSettings : AbilitySettings
    {
        public int health = 100;
    }

    [System.Serializable]
    public class AbilityHealing : Ability
    {
        [SerializeField]
        protected HealingSettings abilitySettings = new HealingSettings();


        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            int mHealth = ps.MaxHealth;
            ps.Health = abilitySettings.health;
        }
    }
}