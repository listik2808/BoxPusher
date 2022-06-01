using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAim : Aim
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MovementBlue();
        }
    }
}
