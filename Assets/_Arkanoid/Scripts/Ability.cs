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

    [System.Serializable]
    public class SpeedUpSettings : AbilitySettings
    {
        public float speedfactor = 1.5f;
    }

    [System.Serializable]
    public class HealingSettings : AbilitySettings
    {
        public int health = 100;
    }

    [System.Serializable]
    public class FireSettings : AbilitySettings
    {
        public GameObject fireprojectile;
    }

    /***********************************
     * Abilities
     * ********************************/
    public abstract class Ability : IAbility
    {
        protected  PlatformSettings ps;

        public virtual void Apply(Platform p) { }
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

    [System.Serializable]
    public class Ability_SpeedUp : Ability
    {
        [SerializeField]
        protected SpeedUpSettings abilitySettings = new SpeedUpSettings();
        //Speed up effect
        private float oldSpeed = 0;
        private bool speedup = false;

        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            p.StartCoroutine(SpeedUpAbility());
            //Debug.Log("Ability_SpeedUp Apply");
        }

        /** */
        private IEnumerator SpeedUpAbility()
        {
            if (!speedup)
            {
                speedup = true;
                oldSpeed = ps.Speed;
                ps.Speed *= abilitySettings.speedfactor;
                yield return new WaitForSeconds(abilitySettings.lifetime);
                ps.Speed = oldSpeed;
                speedup = false;
            }

            yield return null;
        }
    }

    [System.Serializable]
    public class Ability_FireUp : Ability
    {
        [SerializeField]
        protected FireSettings abilitySettings = new FireSettings();

        public override void Apply(Platform p)
        {
            ps = p.GetPlatformSettings;
            GameObject.Instantiate(abilitySettings.fireprojectile, p.transform.position, p.transform.rotation);
            //Debug.Log("Ability_FireUp Apply");
        }
    }
}