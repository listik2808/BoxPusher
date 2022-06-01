using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] private Pusher _template;
    [SerializeField] private Transform _spawnPoints;
    [SerializeField] private Aim _aim;
    [SerializeField] private Cube[] _cubes;
    [SerializeField] private PusherStorage _pusherStorage;
    [SerializeField] private BarrelDirection _barrelDirection;

    private float _elepsedTime = 0;

    public const float DELAY = 0.2f;

    public IEnumerator TimeToSpawn()
    {
        _elepsedTime += Time.deltaTime;
        while (_elepsedTime >= DELAY)
        {
            Pusher spawned = Instantiate(_template, _spawnPoints.transform);

            if(spawned is RedPusher)
                _barrelDirection.GunAttakAnimation();
            else
                _barrelDirection.GunAttakAnimation();

            spawned.Init(_cubes, _aim,_pusherStorage);
            _pusherStorage.AddPusher(spawned);
            _elepsedTime = 0;
            yield return null;
        }
        _barrelDirection.GunStopAnimation();
    }
}
