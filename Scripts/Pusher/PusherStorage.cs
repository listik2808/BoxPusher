using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherStorage : MonoBehaviour
{
    private List<Pusher> _pushers= new List<Pusher>();

    public void AddPusher(Pusher pusher)
    {
        _pushers.Add(pusher);
    }

    public IReadOnlyCollection<Pusher> GetPushers()
    {
        return _pushers;
    }
}
