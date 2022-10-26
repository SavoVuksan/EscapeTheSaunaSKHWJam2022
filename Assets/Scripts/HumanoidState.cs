using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanoidState : IStateSavo
{
    internal HumanoidEnemy Self;

    public virtual void EnterState(StateMachine stateMachine)
    {
        Self = stateMachine?.Holder as HumanoidEnemy;
    }
    public abstract void ExitState(StateMachine stateMachine);
    public abstract void FixedUpdateState(StateMachine stateMachine);
    public abstract void UpdateState(StateMachine stateMachine);
}