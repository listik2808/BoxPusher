using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePusher : Pusher
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out RedPusher redPusher))
        {
            redPusher.Die(redPusher);
        }
        else if(other.TryGetComponent(out BluePusher bluePusher))
        {
            if (bluePusher.IsReady && bluePusher.HasPushed == false && HasPushed == true && bluePusher.Target == Target)
            {
                bluePusher.SetTargetSpeed(Cube);
                Cube.DirectionMovement();
                bluePusher.PuherPush();
                bluePusher.transform.SetParent(Target.transform);
            }
        }
    }
}
