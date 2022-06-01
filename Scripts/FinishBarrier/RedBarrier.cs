using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBarrier : Barrier
{
    [SerializeField] private Material _material;

    public Material FinishMaterial => _material;
}
