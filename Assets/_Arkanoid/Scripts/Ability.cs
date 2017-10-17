using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arkanoid
{
    public interface IAbility
    {
        void Apply(Platform p);
    }

    public class Ability : MonoBehaviour, IAbility
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Apply(Platform p) { }
    }
}