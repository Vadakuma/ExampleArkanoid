using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
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
}