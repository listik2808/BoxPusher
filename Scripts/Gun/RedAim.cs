using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAim : Aim
{
    [SerializeField] private Cube[] _cubes;

    private float _elepsedTime = 0;
    private Coroutine _setAim;

    private const int STEP = 2;
    private const float PointZ = -3f;

    private void Start()
    {
        StartCoroutine(SetAim());
    }

    private void Update()
    {
        _elepsedTime += Time.deltaTime;
        StartSetAim();
    }

    private void StartSetAim()
    {
        float time = Random.Range(3, 7);
        if(_elepsedTime >= time)
        {
            _elepsedTime = 0;
            if (_setAim != null)
            {
                StopCoroutine(_setAim);
                _setAim = null;
                _setAim = StartCoroutine(SetAim());
            }
            else
            {
                StartCoroutine(SetAim());
            }
        }
    }

    private  IEnumerator SetAim()
    {
        var point = Random.Range(0, _cubes.Length);
        for (int i = 0; i < _cubes.Length; i++)
        {
           if(i == point)
           {
                var cube = _cubes[i];
                if (_cubes[i]._isFinished)
                {
                    yield return null;
                }
                else
                {
                    if (cube.transform.position.z < -7.3f)
                    {
                        transform.position = new Vector3(cube.transform.position.x, PointY, PointZ); 
                    }
                    else
                    {
                        transform.position = new Vector3(cube.transform.position.x, PointY, cube.transform.position.z + STEP);
                    }

                    SetPoint(transform.position, cube);
                }
           }
        }
        yield return null;
    }
}
