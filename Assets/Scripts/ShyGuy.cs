using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuy : HumanoidEnemy
{
    [SerializeField]
    public float AttackRadius;
    private StateMachine _moveStateManager;
    private Camera _playerCamera;

    public override void Start()
    {
        base.Start();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _moveStateManager = gameObject.AddComponent<StateMachine>();
        _moveStateManager.Holder = this;
        _moveStateManager.SetNewState(new ShyGuy.IdleState());
    }

    void Update()
    {
        if (!IsEnemyInCameraSight())
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.MoveState))
            {
                _moveStateManager.SetNewState(new ShyGuy.MoveState());
            }
            if (GetDistanceToPlayer() < AttackRadius)
            {
                // Add code for attack handling.
            }
        }
        else
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.IdleState))
            {
                _moveStateManager.SetNewState(new ShyGuy.IdleState());
            }
        }
    }

    public override void OnHit(float damage)
    {

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
        public override void ExitState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
        }

        public override void FixedUpdateState(StateMachine stateMachine)
        {
        }

        public override void UpdateState(StateMachine stateMachine)
        {
            Self.NavMeshAgent.SetDestination(Self.Player.transform.position);
        }
    }
}
