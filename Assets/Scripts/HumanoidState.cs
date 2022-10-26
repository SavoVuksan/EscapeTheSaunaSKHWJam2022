using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanoidState: IState{
    internal HumanoidEnemy Self;

    public virtual void enterState(StateManager manager){
        Self = manager?.Holder as HumanoidEnemy;
    }
    public abstract void exitState(StateManager manager);
    public abstract void FixedUpdateState(StateManager manager);
    public abstract void updateState(StateManager manager);
}