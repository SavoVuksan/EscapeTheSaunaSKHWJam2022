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
    public float SoundCooldown = 5;
    private StateMachine _moveStateMachine;
    private StateMachine _attackStateMachine;
    private AttackState _attackState;
    private AudioHandler _audioHandler;
    private float _soundCooldownTimer;
    public override void Start()
    {
        base.Start();
        _moveStateMachine = gameObject.AddComponent<StateMachine>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.SetNewState(new IdleState());

        _attackStateMachine = gameObject.AddComponent<StateMachine>();
        _audioHandler = GetComponent<AudioHandler>();
        _attackStateMachine.Holder = this;
        _soundCooldownTimer = SoundCooldown;
    }

    void Update()
    {
        if (GetDistanceToPlayer() < AttackRadius && _attackState == null)
        {
            _attackState = new AttackState();
            _attackStateMachine.SetNewState(_attackState);
        }

        if(_soundCooldownTimer <= 0){
            _audioHandler.PlayRandomFromGroup("Grunting");
            _soundCooldownTimer = SoundCooldown;
        }
        _soundCooldownTimer -= Time.deltaTime;
        _soundCooldownTimer = Mathf.Max(0, _soundCooldownTimer);
    }

    public override void OnHit(float damage)
    {
        _moveStateMachine.SetNewState(new HitState());
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MoveActivationRadiusToPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
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
        }
        public override void FixedUpdateState(StateMachine stateMachine)
        {
            (Self as Creep).NavMeshAgent.SetDestination((Self as Creep).Player.transform.position);
        }
        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    private class HitState : HumanoidState
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
        }


        public override void UpdateState(StateMachine stateMachine)
        {
        }
    }

    private class AttackState : HumanoidState
    {

        public override void EnterState(StateMachine stateMachine)
        {
            base.EnterState(stateMachine);
            if(GameManager.Instance.hasTowel){
                GameManager.Instance.WaistSocket.ForceRelease();
                GameManager.Instance.TowerSword.transform.parent = stateMachine.Holder.transform;

                
            }else{
                GameManager.Instance.LoseGame();
            }

            stateMachine.SetNewState(new LeaveState());
            // Add logic for towel grabbing and schlong tapping
        }

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

    private class LeaveState : HumanoidState
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

}