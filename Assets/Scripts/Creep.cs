using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HurricaneVR.Framework.Core;

public class Creep : HumanoidEnemy
{
    [SerializeField]
    public float MoveActivationRadiusToPlayer;
    [SerializeField]
    public float AttackRadius;
    [SerializeField]
    public Timer AttackTimer;
    [SerializeField]
    public Timer TowelDropTimer;
    private StateMachine _moveStateMachine;
    private AudioHandler _audioHandler;
    public override void Start()
    {
        base.Start();
        _moveStateMachine = gameObject.AddComponent<StateMachine>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.SetNewState(new IdleState());
        _audioHandler = GetComponent<AudioHandler>();


    }

    public override void OnEnable()
    {
        base.OnEnable();
        AttackTimer.Init(this);
        TowelDropTimer.Init(this);
        SoundTimer.TimeOutEvent += PlaySound;
        AttackTimer.TimeOutEvent += Attack;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SoundTimer.TimeOutEvent -= PlaySound;
        AttackTimer.TimeOutEvent -= Attack;
    }

    void Update()
    {
        if (GetDistanceToPlayer() < AttackRadius)
        {
            AttackTimer.StartTimer();
        }
        else
        {
            AttackTimer.StopTimer();
        }
    }

    public override void OnHit(float damage)
    {
        // Do something if hit.
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MoveActivationRadiusToPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }

    private void PlaySound()
    {
        _audioHandler.PlayRandomFromGroup("Grunting");
    }

    private void Attack()
    {
        _audioHandler.Play("Grab");
        if (GameManager.Instance.hasTowel)
        {
            GameManager.Instance.WaistSocket.ForceRelease();
            GameManager.Instance.TowelSword.transform.parent = transform;
        }
        else
        {
            GameManager.Instance.LoseGame();
        }
        _moveStateMachine.SetNewState(new LeaveState());
    }


    private class IdleState : HumanoidState
    {
        public override void EnterState(StateMachine stateMachine)
        {
            base.EnterState(stateMachine);
        }

        public override void ExitState(StateMachine stateMachine)
        {
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
            if ((Self as Creep).GetDistanceToPlayer() < (Self as Creep).MoveActivationRadiusToPlayer)
            {
                stateMachine.SetNewState(new MoveState());
            }
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
        }
        public override void ExitState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
        }
        public override void FixedUpdateState(StateMachine stateMachine)
        {
            (Self as Creep).NavMeshAgent.SetDestination((Self as Creep).Player.transform.position);
        }
        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }
    private class LeaveState : HumanoidState
    {
        public override void EnterState(StateMachine stateMachine)
        {
            base.EnterState(stateMachine);
            (Self as Creep).TowelDropTimer.StartTimer();
            (Self as Creep).TowelDropTimer.TimeOutEvent += DropTowel;
            Self.NavMeshAgent.stoppingDistance = 0;
        }
        public override void ExitState(StateMachine stateMachine)
        {
             (Self as Creep).TowelDropTimer.TimeOutEvent -= DropTowel;
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position + (Self.transform.position - Self.Player.transform.position).normalized);
        }

        public override void UpdateState(StateMachine stateMachine)
        {
        }

        private void DropTowel(){
            GameManager.Instance.TowelSword.GetComponent<Towel>().ReparentOriginal();

            DestroySelf();
        }

        private void DestroySelf(){
            Destroy(Self.gameObject);
        }
    }

}