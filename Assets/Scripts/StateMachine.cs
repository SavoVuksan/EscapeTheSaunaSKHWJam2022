using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    [Header("State Machine")]
    [SerializeField]
    private string _currentStateName;
    private IStateSavo _currentState;

    public HumanoidEnemy Holder;

    public IStateSavo CurrentState{
        get{
            return _currentState;
        }
        set{
            _currentState = value;
        }
    }

    public virtual void Update()
    {
        _currentState.UpdateState(this);
    }
    public virtual void FixedUpdate()
    {
        //FSM 
        _currentState.FixedUpdateState(this);
    }

    public void SetNewState(IStateSavo newState)
    {
        _currentState?.ExitState(this);

        _currentState = newState;
        _currentStateName = newState.ToString();

        _currentState.EnterState(this);
    }

}
