using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Example. Weapon is dummy for this project
 * */
namespace Arkanoid
{
    public interface IWeapon
    {
        void Fire();
    }


    public class Weapon : MonoBehaviour, IWeapon
    {

        private WeaponSettings _weaponSettings;
        public WeaponSettings GetWeaponSettings { get { return _weaponSettings; } private set { _weaponSettings = value; } }


        public void Fire()
        {
            GameObject go = Instantiate(GetWeaponSettings.GetProjectile);
            if(go)
            {
                Projectile proj = go.GetComponent<Projectile>();
                if (proj)
                    proj.SetActiveState(GetWeaponSettings.GetFirepoint);
            }
        }
    }
}