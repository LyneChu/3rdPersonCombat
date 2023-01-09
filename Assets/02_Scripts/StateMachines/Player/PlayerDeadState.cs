using UnityEngine;
using System.Collections;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        // Todo Toggle Ragdoll

        stateMachine.Weapon.gameObject.SetActive(false);

    }

    public override void Tick(float deltaTime) { }

    public override void Exit() { }

}
