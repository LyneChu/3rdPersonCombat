using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");

    private float remainingDodgeDuration;

    private Vector3 dodgingDirectionInput;

    private const float CrossFadeDuration = 0.1f;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine) {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter() {
        
        remainingDodgeDuration = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DodgeForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime) {
        Vector3 movement = new();

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / remainingDodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / remainingDodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeDuration -= deltaTime;

        if (remainingDodgeDuration <= 0f)
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
    }

    public override void Exit() {

        stateMachine.Health.SetInvulnerable(false);

    }
}
