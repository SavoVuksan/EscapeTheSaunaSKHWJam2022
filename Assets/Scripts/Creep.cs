using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creep : HumanoidEnemy
{
    [SerializeField]
    public float MoveActivationRadiusToPlayer;
    [SerializeField]
    public float AttackRadius;
    private StateMachine _moveStateMachine;
    private AttackState _attackState;
    public override void Start()
    {
        base.Start();
        _moveStateMachine = gameObject.AddComponent<StateMachine>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.SetNewState(new IdleState());
    }

    void Update()
    {
        if (GetDistanceToPlayer() < AttackRadius && _attackState == null)
        {
            _attackState = new AttackState();
            _attackState.EnterState(null);
        }
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
            // Add logic for towel grabbing and schlong tapping
            PrintUtil.Instance.Print("Play attack anim!");
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

}