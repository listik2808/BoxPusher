using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlue : Spawner
{
    private void Update()
    {
        if (Input.GetMouseButton(0) == false)
            return;
        else
            StartCoroutine(TimeToSpawn());
    }
}
