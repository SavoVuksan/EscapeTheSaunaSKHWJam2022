using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    [Header("State Machine")]
    [SerializeField]
    private string currentStateName;
    private IState currentState;



    [Header("general")]
    public LayerMask playerLayer;

    public virtual void Awake()
    {
    }
    public virtual void Update()
    {
        currentState.updateState(this);
    }
    public virtual void FixedUpdate()
    {
        //FSM 
        currentState.FixedUpdateState(this);
    }

    public void setNewState(IState newState)
    {
        currentState?.exitState(this);

        currentState = newState;
        currentStateName = newState.ToString();

        currentState.enterState(this);
    }

}
