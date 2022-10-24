using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void enterState(StateManager manager);
    void exitState(StateManager manager);
    void updateState(StateManager manager);
    void FixedUpdateState(StateManager manager);
}