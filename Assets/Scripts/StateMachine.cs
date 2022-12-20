using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void Update() {

        currentState?.Tick(Time.deltaTime);

        // ? : Null Conditional Operator 
        // Warning : it wont't work with MonoBehaviour / ScriptableObject
    }

}
