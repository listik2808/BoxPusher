using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPusher : Pusher
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BluePusher bluePusher))
        {
            bluePusher.Die(bluePusher);
        }
        else if (other.TryGetComponent(out RedPusher redPusher))
        {
            if (redPusher.IsReady && redPusher.HasPushed == false && HasPushed == true && redPusher.Target == Target)
            {
                redPusher.PuherPush();
                redPusher.SetTargetSpeed(Cube);
                Cube.DirectionMovement();
                redPusher.transform.SetParent(Target.transform);
            }
        }
    }
}
