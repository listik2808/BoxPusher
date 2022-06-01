using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aim : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    private Cube _cube;
    private Camera _camera;
    private Vector3 _firePoint;

    private const float Point_Y = 0.013f;

    public Vector3 FirePoint => _firePoint;
    public Cube Cube => _cube;
    public float PointY => Point_Y;

    private void Start()
    {
        _camera = Camera.main;
        _cube = GetComponent<Cube>();
    }

    public void MovementBlue()
    {
        Ray newPoint = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(newPoint, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
            transform.position = new Vector3(transform.position.x, Point_Y, transform.position.z);
            SetPoint(raycastHit.point,_cube);
        }
    }

    public Vector3 SetPoint(Vector3 point,Cube cube)
    {
        _firePoint = point;
        _cube = cube;
        return point;
    }
}
