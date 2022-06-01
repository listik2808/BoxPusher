using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBarrier : Barrier
{
    [SerializeField] private Material _material;

    public Material FinishMaterial => _material;
}
