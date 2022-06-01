using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BarrelDirection : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Animator _animator;

    private const string Shoot = "Shoot";
    
    private void Update()
    {
        Tracking();
    }

    public void Tracking()
    {
        Vector3 targetDistance = _target.position - transform.position;
        Vector3 gunRotation = new Vector3(targetDistance.x, 0f, targetDistance.z);

        if(_target.TryGetComponent(out RedAim redAim))
            transform.rotation = Quaternion.LookRotation(-gunRotation, Vector3.up);
        else
            transform.rotation = Quaternion.LookRotation(gunRotation, Vector3.up);
    }

    public void GunAttakAnimation()
    {
        _animator.SetTrigger(Shoot);
    }

    public void GunStopAnimation()
    {
        _animator.SetTrigger(Shoot);
    }
}
