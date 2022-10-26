using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateSavo 
{
    void EnterState(StateMachine stateMachine);
    void ExitState(StateMachine stateMachine);
    void UpdateState(StateMachine stateMachine);
    void FixedUpdateState(StateMachine stateMachine);
}
