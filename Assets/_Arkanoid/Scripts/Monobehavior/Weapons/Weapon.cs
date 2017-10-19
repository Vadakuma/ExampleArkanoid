using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Weapon is dummy for this project
 * */
namespace Arkanoid
{
    public interface IWeapon
    {
        void Fire();

    }

    [System.Serializable]
    public class WeaponSettings
    {
        [SerializeField]
        protected GameObject projectile;
        [SerializeField]
        protected Transform firepoint;

        public Transform GetFirepoint { get { return firepoint; } private set { firepoint = value; } }
        public GameObject GetProjectile { get { return projectile; } private set { projectile = value; } }
    }


    public class Weapon : MonoBehaviour, IWeapon
    {

        private WeaponSettings weaponSettings;
        public WeaponSettings GetWeaponSettings { get { return weaponSettings; } private set { weaponSettings = value; } }


        // 
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /** */
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