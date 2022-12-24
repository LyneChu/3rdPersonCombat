using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    public override void Enter() {
        throw new System.NotImplementedException();
    }

    public override void Exit() {
        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime) {
        throw new System.NotImplementedException();
    }
}
