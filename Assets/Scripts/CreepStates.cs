using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private TheCreep _creep;
    public void enterState(StateManager manager)
    {
        this._creep = manager.Holder as TheCreep;
    }

    public void exitState(StateManager manager)
    {
    }

    public void FixedUpdateState(StateManager manager)
    {
        if (_creep.GetDistanceToPlayer() < this._creep.MoveActivationRadiusToPlayer)
        {
            manager.setNewState(new MoveState());
        }
    }

    public void updateState(StateManager manager)
    {
    }


}

public class MoveState : IState
{
    private TheCreep _creep;
    public void enterState(StateManager manager)
    {
        _creep = manager.Holder as TheCreep;
    }

    public void exitState(StateManager manager)
    {
    }

    public void FixedUpdateState(StateManager manager)
    {
        _creep.NavMeshAgent.SetDestination(_creep.Player.transform.position);
    }

    public void updateState(StateManager manager)
    {
    }
}

public class HitState : IState
{
    private TheCreep _creep;

    public void enterState(StateManager manager)
    {
        _creep = manager.Holder as TheCreep;
    }

    public void exitState(StateManager manager)
    {
    }

    public void FixedUpdateState(StateManager manager)
    {
    }

    public void updateState(StateManager manager)
    {
    }
}

public class AttackState : IState
{

    public void enterState(StateManager manager)
    {
        // Add logic for towel grabbing and schlong tapping
        PrintUtil.Instance.Print("Play attack anim!");
    }

    public void exitState(StateManager manager)
    {
    }

    public void FixedUpdateState(StateManager manager)
    {
    }

    public void updateState(StateManager manager)
    {
    }
}
