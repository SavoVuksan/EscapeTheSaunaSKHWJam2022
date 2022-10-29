using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuy : HumanoidEnemy
{
    [SerializeField]
    public float AttackRadius;
    [SerializeField]
    public Timer AttackTimer;
    [SerializeField]
    public Timer AttackTelegraphTimer;
    [SerializeField]
    public Timer DespawnTimer;
    private StateMachine _moveStateManager;
    private Camera _playerCamera;
    private AudioHandler _audioHandler;

    private ragdollMaster _ragdoll;
    private bool _attacked;

    public override void Start()
    {
        base.Start();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _moveStateManager = gameObject.AddComponent<StateMachine>();
        _moveStateManager.Holder = this;
        _moveStateManager.SetNewState(new ShyGuy.IdleState());
        _audioHandler = GetComponent<AudioHandler>();
        _ragdoll = GetComponentInChildren<ragdollMaster>();
        _attacked = false;

    }

    public override void OnEnable()
    {
        base.OnEnable();
        AttackTimer.Init(this);
        AttackTelegraphTimer.Init(this);
        DespawnTimer.Init(this);
        SoundTimer.TimeOutEvent += PlaySound;
        AttackTimer.TimeOutEvent += Attack;
        AttackTelegraphTimer.TimeOutEvent += AttackTelegraph;
        DespawnTimer.TimeOutEvent += Despawn;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SoundTimer.TimeOutEvent -= PlaySound;
        AttackTimer.TimeOutEvent -= Attack;
        AttackTelegraphTimer.TimeOutEvent -= AttackTelegraph;
        DespawnTimer.TimeOutEvent -= Despawn;

    }

    private void Despawn()
    {
        GameManager.Instance.TowelSword.GetComponent<Towel>().ReparentOriginal();
        Destroy(gameObject);
    }

    private void PlaySound()
    {
        _audioHandler?.PlayRandomFromGroup("Grunting");
    }

    private void AttackTelegraph()
    {
        _audioHandler.Play("AttackTelegraph");
    }

    private void Attack()
    {
        _attacked = true;
        _audioHandler.Play("Attack");
        if (GameManager.Instance.hasTowel)
        {
            GameManager.Instance.WaistSocket.ForceRelease();
            GameManager.Instance.TowelSword.transform.parent = transform;
        }
        else
        {
            GameManager.Instance.LoseGame();
        }


    }

    void Update()
    {
        print(_moveStateManager.CurrentState);
        if (!IsEnemyInCameraSight())
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.MoveState))
            {
                _moveStateManager.SetNewState(new ShyGuy.MoveState());
            }
            if (GetDistanceToPlayer() <= AttackRadius)
            {
                AttackTimer.StartTimer();
                AttackTelegraphTimer.StartTimer();
                if (!(_moveStateManager.CurrentState is IdleState) && !_attacked)
                {
                    _moveStateManager.SetNewState(new IdleState());
                }
            }
            else
            {
                AttackTimer.StopTimer();
                AttackTelegraphTimer.StopTimer();
                if (!(_moveStateManager.CurrentState is MoveState) && !_attacked)
                {
                    _moveStateManager.SetNewState(new MoveState());
                }

            }
        }
        else
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.IdleState))
            {
                _moveStateManager.SetNewState(new ShyGuy.IdleState());
            }
        }

        if (_attacked && !(_moveStateManager.CurrentState is LeaveState))
        {
            _moveStateManager.SetNewState(new LeaveState());
        }
    }

    public override void OnHit(float damage)
    {
        _ragdoll.RagdollModeON();
    }

    public bool IsEnemyInCameraSight()
    {
        Vector3 pointOnScreen = _playerCamera.WorldToViewportPoint(transform.position);
        if (pointOnScreen.x > 1 || pointOnScreen.x < 0 || pointOnScreen.y > 1 || pointOnScreen.y < 0 || pointOnScreen.z < 0)
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }


    private class IdleState : HumanoidState
    {
        public override void ExitState(StateMachine stateMachine)
        {
        }
        public override void FixedUpdateState(StateMachine stateMachine)
        {
        }


        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    private class MoveState : HumanoidState
    {
        public override void EnterState(StateMachine stateMachine)
        {
            base.EnterState(stateMachine);
            (Self as ShyGuy)._audioHandler.Play("Walking");
        }
        public override void ExitState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
            (Self as ShyGuy)._audioHandler.Stop("Walking");
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.Player.transform.position);
        }
    }

    private class LeaveState : HumanoidState
    {
        public override void EnterState(StateMachine stateMachine)
        {
            base.EnterState(stateMachine);
            (Self as ShyGuy)._audioHandler.Play("Walking");
            (Self as ShyGuy).DespawnTimer.StartTimer();
        }
        public override void ExitState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
            (Self as ShyGuy)._audioHandler.Stop("Walking");
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position + (Self.transform.position - Self.Player.transform.position).normalized * 10);
        }
    }
}
