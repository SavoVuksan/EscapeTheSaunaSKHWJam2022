using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    [Header("State Machine")]
    [SerializeField]
    private string currentStateName;
    public IState CurrentState;

    private object _holder;

    [Header("general")]
    public LayerMask playerLayer;

    public object Holder{
        get{
            return _holder;
        }
        set{
            _holder = value;
        }
    }

    public virtual void Awake()
    {
    }
    public virtual void Update()
    {

        CurrentState?.updateState(this);

    }
    public virtual void FixedUpdate()
    {
        //FSM 
        CurrentState?.FixedUpdateState(this);

    }

    public void setNewState(IState newState)
    {
        CurrentState?.exitState(this);

        CurrentState = newState;
        currentStateName = newState.ToString();

        CurrentState.enterState(this);
    }

}
