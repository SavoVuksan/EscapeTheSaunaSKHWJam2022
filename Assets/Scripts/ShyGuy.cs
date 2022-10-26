using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuy : HumanoidEnemy
{
    [SerializeField]
    public float AttackRadius;
    private StateManager _moveStateManager;
    private Camera _playerCamera;

    public override void Start()
    {
        base.Start();
        _playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _moveStateManager = gameObject.AddComponent<StateManager>();
        _moveStateManager.Holder = this;
        _moveStateManager.setNewState(new ShyGuy.IdleState());
    }

    void Update()
    {
        if (!IsEnemyInCameraSight())
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.MoveState))
            {
                _moveStateManager.setNewState(new ShyGuy.MoveState());
            }
            if(GetDistanceToPlayer() < AttackRadius){
                // Add code for attack handling.
            }
        }
        else
        {
            if (!(_moveStateManager.CurrentState is ShyGuy.IdleState))
            {
                _moveStateManager.setNewState(new ShyGuy.IdleState());
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

    private class MoveState : HumanoidState
    {
        public override void exitState(StateManager manager)
        {
            Self.NavMeshAgent.SetDestination(Self.transform.position);
        }

        public override void FixedUpdateState(StateManager manager)
        {
        }

        public override void updateState(StateManager manager)
        {
            Self.NavMeshAgent.SetDestination(Self.Player.transform.position);
        }
    }
}
