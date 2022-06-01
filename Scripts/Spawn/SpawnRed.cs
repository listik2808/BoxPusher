using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRed : Spawner
{
    private void Update()
    {
        StartCoroutine(TimeToSpawn());
    }
}
