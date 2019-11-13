using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
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
}