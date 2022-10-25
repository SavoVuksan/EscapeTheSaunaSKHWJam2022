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
    private StateManager _moveStateMachine;
    private AttackState _attackState;
    public override void Start()
    {
        base.Start();
        _moveStateMachine = gameObject.AddComponent<StateManager>();
        _moveStateMachine.Holder = this;
        _moveStateMachine.setNewState(new IdleState());
    }

    void Update()
    {
        if (GetDistanceToPlayer() < AttackRadius && _attackState == null)
        {
            _attackState = new AttackState();
            _attackState.enterState(null);
        }
    }

    public override void OnHit(float damage)
    {
        _moveStateMachine.setNewState(new HitState());
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
        public override void enterState(StateManager manager)
        {
            base.enterState(manager);
        }

        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
            if ((Self as Creep).GetDistanceToPlayer() < (Self as Creep).MoveActivationRadiusToPlayer)
            {
                manager.setNewState(new MoveState());
            }
        }

        public override void updateState(StateManager manager)
        {
        }


    }

    private class MoveState : HumanoidState
    {
        public override void enterState(StateManager manager)
        {
            base.enterState(manager);
        }

        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
            (Self as Creep).NavMeshAgent.SetDestination((Self as Creep).Player.transform.position);
        }

        public override void updateState(StateManager manager)
        {
        }
    }

    private class HitState : HumanoidState
    {
        public override void enterState(StateManager manager)
        {
            base.enterState(manager);
        }

        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
        }

        public override void updateState(StateManager manager)
        {
        }
    }

    private class AttackState : HumanoidState
    {

        public override void enterState(StateManager manager)
        {
            base.enterState(manager);
            // Add logic for towel grabbing and schlong tapping
            PrintUtil.Instance.Print("Play attack anim!");
        }

        public override void exitState(StateManager manager)
        {
        }

        public override void FixedUpdateState(StateManager manager)
        {
        }

        public override void updateState(StateManager manager)
        {
        }
    }

}