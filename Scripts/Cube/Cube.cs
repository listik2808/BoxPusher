using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
[RequireComponent (typeof(MoveCube))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Material[] _material;
    [SerializeField] private GameObject[] _stickMan;

    private List<Pusher> _redPushers = new List<Pusher>();
    private List<Pusher> _bluePuhers = new List<Pusher>();

    private Renderer _renderer;
    private MoveCube _moveCube;
    private Rigidbody _rigidbody;

    private const float PointZRed = -9f;
    private const float PointZBlue = -4.2f;
    private const float NewSpeed = 0.055f;
    private const string IsSleep = "IsSleep";
    private const string IsIdle = "IsIdle";
    private const string IsFearCube = "IsFearCube";
    private const string IsFear = "IsFear"; 

    public int CountRed { get;private set; }
    public int CountBlue { get; private set; }
    public float Speed { get; private set; }
    public bool _isFinished { get; private set; } = false;

    private void Start()
    {
        _moveCube = GetComponent<MoveCube>();
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DeadRemove(_bluePuhers);
        DeadRemove(_redPushers);
        PlayAnimationCube();
    }

    public float DirectionMovement()
    {
        int count;
        CountBlue = _bluePuhers.Count;
        CountRed = _redPushers.Count;
        count = CountRed - CountBlue;
        Speed = ((NewSpeed * count) * -1);
        return Speed;
    }

    public void SetSpeed(Pusher pusher)
    {
        int positiveDirection = 1;
        int negativeDirection = -1;

        if(pusher is RedPusher)
        {
            AddPushedRed(pusher);
            Speed += NewSpeed * negativeDirection * Time.deltaTime;
        }
        else if(pusher is BluePusher)
        {
            AddPushedBlue(pusher);
            Speed += NewSpeed * positiveDirection * Time.deltaTime;
        }

        _moveCube.SetDirection(Speed);
    }

    public void AddPushedBlue(Pusher pusher)
    {
        AddPusher(_bluePuhers, pusher, out int count);
        CountBlue = count;
    }

    public void AddPushedRed(Pusher pusher)
    {
        AddPusher(_redPushers, pusher, out int count);
        CountRed = count;
    }

    public void RemovePusherBlue(Pusher pusher)
    {
        RemovePusher(_bluePuhers, pusher, out int count);
        CountBlue = count;
    }

    public void RemovePusherRed(Pusher pusher)
    {
        RemovePusher(_redPushers, pusher, out int count);
        CountRed = count;
    }
    private void PlayAnimationCube()
    {
        StickmanDisable();
        StopAnimation();
        if (_isFinished== false)
        {
            if (transform.localPosition.z <= PointZRed || transform.localPosition.z >= PointZBlue)
            {
                _stickMan[2].gameObject.SetActive(true);
                _animator.SetBool(IsFearCube, true);
            }
            else
            {
                if (CountBlue == 0 && CountRed == 0)
                {
                    _stickMan[0].gameObject.SetActive(true);
                    _animator.SetBool(IsSleep, true);
                }
                else
                {
                    _stickMan[1].gameObject.SetActive(true);
                    _animator.SetBool(IsIdle, true);
                }
            }
        }
        else
        {
            _stickMan[3].SetActive(true);
            _animator.SetBool(IsFear, true);
        }
    }

    private void AddPusher(List<Pusher> pushers,Pusher pusher,out int count)
    {
        pushers.Add(pusher);
        count = pushers.Count;
    }

    private void RemovePusher(List<Pusher> pushers,Pusher pusher,out int count)
    {
        pushers.Remove(pusher);
        count = pushers.Count;
        if (pusher is RedPusher)
            Speed = (count * NewSpeed) * -1;
        else
            Speed = (count * NewSpeed);

        _moveCube.SetDirection(Speed);
    }

    private float DirectionMovementTwoCubes(Cube cube,out float speed)
    {
        int count;
        count = cube.CountRed - CountBlue;
        speed = ((NewSpeed * count) * -1 );
        return speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            if (cube.CountRed > CountBlue)
            {
                DirectionMovementTwoCubes(cube,out float speed);
                Speed = speed;
                cube.Speed = speed;
            }
            else if (cube.CountRed < CountBlue)
            {
                DirectionMovementTwoCubes(cube, out float speed);
                Speed = speed;
                cube.Speed = speed;
            }
            cube._moveCube.SetDirection(Speed);
            _moveCube.SetDirection(Speed);

            if (_isFinished)
            {
                cube.Finish();
                cube._renderer.material = _renderer.material;
                if (cube.CountRed > CountBlue)
                {
                    cube.InstallWarriorsTeam(cube._redPushers);
                    cube.KillPusher(cube._bluePuhers);
                }
                else if (cube.CountBlue > CountRed)
                {
                    cube.InstallWarriorsTeam(cube._bluePuhers);
                    cube.KillPusher(cube._redPushers);
                }
            }
        }
    }

    private List<Pusher> DeadRemove(List<Pusher> pushers)
    {
        for (int i = 0; i < pushers.Count; i++)
        {
            if (pushers[i].IsDead == true)
            {
                pushers.RemoveAt(i);
            }
        }
        return pushers;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Pusher pusher))
        {
            if (_isFinished)
            {
                if(transform.position.z <= PointZBlue)
                {
                    pusher.SetTargetSpeed(this);
                    InstallWarriorsTeam(_redPushers);
                    KillPusher(_bluePuhers);
                }
                else if(transform.position.z <= PointZRed)
                {
                    pusher.SetTargetSpeed(this);
                    InstallWarriorsTeam(_bluePuhers);
                    KillPusher(_redPushers);
                }
            }
            if (pusher.HasPushed == false && pusher.Target == this)
            {
                pusher.SetTargetSpeed(this);
                MaterialSetting();
                pusher.PuherPush();
                pusher.transform.SetParent(gameObject.transform);
            }
        }
        if (other.TryGetComponent(out BlueBarrier blue))
        {
            Finish();
            _renderer.material = blue.FinishMaterial;
            InstallWarriorsTeam(_redPushers);
            KillPusher(_bluePuhers);
        }
        else if (other.TryGetComponent(out RedBarrier red))
        {
            Finish();
            _renderer.material = red.FinishMaterial;
            InstallWarriorsTeam(_bluePuhers);
            KillPusher(_redPushers);
        }
    }

    private void KillPusher(List<Pusher>pushers)
    {
        while(pushers.Count != 0)
        {
            for (int i = 0; i < pushers.Count; i++)
            {
                pushers[i].gameObject.SetActive(false);
                pushers[i].Die(pushers[i]);
            }
        }
    }

    private void InstallWarriorsTeam(List<Pusher> pushers)
    {
        foreach (Pusher pusher in pushers)
        {
            pusher.Warrior(pusher);
            pusher.Attack();
        }
    }

    private void MaterialSetting()
    {
        if (_isFinished == false)
        {
            if (CountRed > CountBlue)
            {
                Speed = DirectionMovement();
                _renderer.sharedMaterial = _material[0];
            }
            else if (CountBlue > CountRed)
            {
                Speed = DirectionMovement();
                _renderer.sharedMaterial = _material[1];
            }
            _moveCube.SetDirection(Speed);
        }
    }

    private void Finish()
    {
        _isFinished = true;
        _rigidbody.isKinematic = true;
        Speed = 0;
        _moveCube.enabled = false;
    }
    private void StickmanDisable()
    {
        for (int i = 0; i < _stickMan.Length; i++)
        {
            _stickMan[i].SetActive(false);
        }
    }

    private void StopAnimation()
    {
        _animator.SetBool(IsSleep, false);
        _animator.SetBool(IsFearCube, false);
        _animator.SetBool(IsIdle, false);
        _animator.SetBool(IsFear, false);
    }
}
