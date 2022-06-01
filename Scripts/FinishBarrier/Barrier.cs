using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Barrier : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string Shake = "Shake";

    private protected void AnimationCamera()
    {
        _animator.SetTrigger(Shake);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Cube cube))
        {
            AnimationCamera();
        }
    }
}
