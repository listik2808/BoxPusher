using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PusherMover))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Pusher : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private GameObject _sword;
    [SerializeField] private GameObject _Crown;
    [SerializeField] private Animator _animator;

    private ParticleSystem _particleSystem;
    private PusherMover _pusherMover;
    private IReadOnlyCollection<Cube> _cubes;
    private Aim _aim;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private BoxCollider _boxCollider;
    private PusherStorage _pusherStorage;
    private Transform _point;

    private bool _isReady = false;

    private const string Fly = "Fly";
    private const string Run = "IsRun";
    private const string Push = "IsPush";
    private const float Offset = 0.15f;

    public Transform Point => _point;
    public bool IsReady => _isReady;
    public bool IsAttacking { get; private set; } = false;
    public Cube Target { get; private set; }
    public Cube Cube { get; private set; }
    public bool IsDead { get; private set; } = false;
    public bool HasPushed { get; private set; } = false;
    public bool IsWarrior { get; private set; } = false;

    private void OnEnable()
    {
        _Crown.SetActive(false);
        _sword.SetActive(false);
    }

    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _pusherMover = GetComponent<PusherMover>();
        _pusherMover.Init(Target, _aim);
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void SetTargetSpeed(Cube cube)
    {
        if (cube == null)
            return;

        HasPushed = true;
        Cube = cube;
        cube.SetSpeed(this);
    }

    public void Ready()
    {
        _isReady = true;
    }

    public void PusherAnimationFly()
    {
        _animator.SetTrigger(Fly);
    }

    public void PusherRun()
    {
        _animator.SetBool(Run, true);
    }

    public void PuherPush()
    {
        _animator.SetBool(Run, false);
        _animator.SetBool(Push, true);
    }

    public void Attack()
    {
        IsAttacking = true;
        StartCoroutine(TimerPoisk());
    }

    public void Warrior(Pusher pusher)
    {
        IsWarrior = true;
        HasPushed = false;
        _Crown.SetActive(true);
        _sword.SetActive(true);
        _animator.SetBool(Push,false);
        _animator.SetBool(Run, true);
    }

    public void Init(IReadOnlyCollection<Cube> cubes,Aim aim,PusherStorage pusherStorage)
    {
        _cubes = cubes;
        _pusherStorage = pusherStorage;
        _aim = aim;

        if (this is BluePusher)
        {
            FindClosestTarget(_aim.transform.position);
            SetPointTargetCube(-Offset);
        } 
        else
        {
            FindTarget(aim.Cube);
            SetPointTargetCube(Offset);
        } 
    }

    public void Die(Pusher pusher)
    {
        if (pusher.HasPushed == true)
        {
            if (pusher is RedPusher)
                pusher.Cube.RemovePusherRed(pusher);
            else
                pusher.Cube.RemovePusherBlue(pusher);

            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    private Transform SetPointTargetCube(float offset)
    {
        float z = offset;
        Vector3 point = new Vector3(Target.transform.position.x , Target.transform.position.y, Target.transform.position.z + z);
        GameObject game = new GameObject();
        var targetPoint = Instantiate(game,point, Quaternion.identity, Target.transform);
        _point = targetPoint.transform;
        return _point;
    }

    private void NearestEnemy()
    {
        Transform position = null;
        var enemies = _pusherStorage.GetPushers();

        if (this is RedPusher)
        {
            var result = enemies.Where(e => e is BluePusher).Where(e => e.IsDead == false).ToArray();
            position = GetEnemyPosition(result);
        }
        else
        {
            var result = enemies.Where(e => e is RedPusher).Where(e => e.IsDead == false).ToArray();
            position = GetEnemyPosition(result);
        }

        _pusherMover.Init(position);
    }

    private IEnumerator TimerPoisk()
    {
        var waitForsecons = new WaitForSeconds(0.5f);
        while (true)
        {
            NearestEnemy();
            yield return waitForsecons;
        }
    }

    private Transform GetEnemyPosition(Pusher[] pushers)
    {
        float distance = Mathf.Infinity;
        Transform point = null;
        Vector3 position = transform.position;
        foreach (Pusher pusher in pushers)
        {
            Vector3 diff = pusher.transform.position - position;
            float currentDistance = diff.sqrMagnitude;
            if (currentDistance < distance)
            {
                distance = currentDistance;
                point = pusher.transform;
            }
            else
                return point;
        }
        return point;
    }

    private void Destroy()
    {
        Destroy(_Crown.gameObject);
        Destroy(_sword.gameObject);
        _particleSystem.Play();
        _boxCollider.enabled = false;
        IsDead = true;
        _skinnedMeshRenderer.enabled = false;
        _pusherMover.enabled = false;
        Destroy(_point.gameObject);
        _navMeshAgent.enabled = false;
    }

    private Cube FindClosestTarget(Vector3 aim)
    {
        float distance = Mathf.Infinity;
        Vector3 position = aim;
        foreach (Cube cube in _cubes)
        {
            if (cube._isFinished == false)
            {
                Vector3 diff = cube.transform.position - position;
                float currentDistance = diff.sqrMagnitude;
                if (currentDistance < distance)
                {
                    Target = cube;
                    distance = currentDistance;
                }
            }
        }
        return null;
    }

    private Cube FindTarget(Cube cube)
    {
        Target = cube;
        return Target;
    }
}
