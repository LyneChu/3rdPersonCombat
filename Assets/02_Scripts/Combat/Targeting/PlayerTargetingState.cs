using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private Vector2 dodgingDirectionInput;
    private float remainingDodgeDuration;

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;


    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        stateMachine.InputReader.CancelEvent += OnCancel;
        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime) {
        if (stateMachine.InputReader.IsAttacking) {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if (stateMachine.InputReader.IsBlocking) {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if (stateMachine.Targeter.CurrentTarget == null) {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }


    public override void Exit() {
        stateMachine.InputReader.CancelEvent -= OnCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void OnCancel() {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge() {
        if (Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown)
            return;

        stateMachine.SetDodgeTime(Time.time);
        dodgingDirectionInput = stateMachine.InputReader.MovementValue;
        remainingDodgeDuration = stateMachine.DodgeDuration;

    }


    private Vector3 CalculateMovement(float deltaTime) {
        Vector3 movement = new();

        if (remainingDodgeDuration > 0f) {
            movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / remainingDodgeDuration;
            movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / remainingDodgeDuration;

            remainingDodgeDuration = Mathf.Max(remainingDodgeDuration - deltaTime, 0f);
        }
        else {
            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        }

        return movement;
    }


    private void UpdateAnimator(float deltaTime) {
        //float movementX = stateMachine.InputReader.MovementValue.x;
        //float movementY = stateMachine.InputReader.MovementValue.y;

        //stateMachine.Animator.SetFloat(TargetingRightHash, movementX);
        //stateMachine.Animator.SetFloat(TargetingForwardHash, movementY);

        if (stateMachine.InputReader.MovementValue.y == 0)
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0, 0.1f, deltaTime);
        else {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if (stateMachine.InputReader.MovementValue.x == 0)
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        else {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }

    }
}
