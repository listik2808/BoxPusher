using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public  class PusherMover : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Cube _cube;
    private Aim _aim;
    private Pusher _pusher;
    private Transform _enemyPoint;

    private float _elepsedTime = 0;

    private const float JumpPower = 1f;
    private const int QuantityJump = 1;
    private const float Duration = 0.5f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = true;
        _navMeshAgent.updateUpAxis = true;
        _pusher = GetComponent<Pusher>();
    }

    private void Update()
    {
        if (_pusher.IsReady)
          RunTarget(_pusher.Point);

        if (_pusher.IsAttacking == true)
        {
            if (_enemyPoint != null)
                AttackTarget(_enemyPoint);
            else
                _pusher.Die(_pusher);
        }
    }

    public void Init(Cube cube, Aim aim)
    {
        _cube = cube;
        _aim = aim;
        MoveJump(aim.FirePoint);
    }

    public void Init(Transform enemyPoint)
    {
        _enemyPoint = enemyPoint;
    }

    private void EnableMove()
    {
        _pusher.Ready();
        _pusher.PusherRun();
    }

    private void MoveJump(Vector3 target)
    {
        var tween = transform.DOJump(target, JumpPower, QuantityJump, Duration);
        _pusher.PusherAnimationFly();
        tween.OnComplete(EnableMove);
    }

    private void RunTarget(Transform target)
    {
        Vector3 point;

        if (target == null)
            _pusher.Die(_pusher);

        if (_pusher.HasPushed)
        {
            _navMeshAgent.speed = 0.2f;
        } 

        if (_pusher is BluePusher)
            point = target.position;

        else
            point = target.position;

        _navMeshAgent.SetDestination(point);
    }

    private void AttackTarget(Transform target)
    {
        _navMeshAgent.speed = 3;
        _navMeshAgent.SetDestination(target.transform.position);
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        _elepsedTime += Time.deltaTime;
        while (_elepsedTime >= 3)
        {
            _pusher.Die(_pusher);
            _elepsedTime = 0;
        }
        yield return null;
    }
}
