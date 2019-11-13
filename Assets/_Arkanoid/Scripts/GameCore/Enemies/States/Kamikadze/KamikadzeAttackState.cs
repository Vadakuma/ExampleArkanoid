using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arkanoid.PlayerPlatform;

namespace Arkanoid.Enemies
{
    /// <summary>
    /// Main active life state
    /// </summary>
    public class KamikadzeAttackState : KamikadzeActiveState
    {
        private Transform _bricktr;
        private Transform _platformtr;
        private Vector3 _dir = Vector3.zero;
        private float _dist;
        private bool _isActive = false;

        public KamikadzeAttackState(Enemy e) : base(e)
        {
            _bricktr = e.transform;
            _platformtr = Platform.Instance.transform;
            _isActive = true; // let stat attack!
        }

        public override void Update()
        {

            if (_isActive)
            {
                // move and attack staff
                _dist = Vector3.Distance(_platformtr.position, _bricktr.position);
                if (_dist < kamikadzeBrickSettings.MinAttackDist)
                    ExplodeAction();
                // direction to the platform
                _dir = _platformtr.position - _bricktr.position;
                // move on last calculate direction
                _bricktr.Translate(_dir.normalized * Time.deltaTime * kamikadzeBrickSettings.Speed);
            }
        }

        private void ExplodeAction()
        {
            Debug.Log("ExplodeAction!");
            _isActive = false;
            if (Platform.Instance)
                Platform.Instance.AddDamage(kamikadzeBrickSettings.Damage);
            // dying stuff
            parent.ToDeadStateActivate();
        }
    }
}
