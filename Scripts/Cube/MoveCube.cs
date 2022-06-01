using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private float _direction = 0;

    private void Update()
    {
        Move(_direction);
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    private void Move(float direction)
    {
        transform.position += new Vector3(0, 0, direction * Time.deltaTime);
    }
}
