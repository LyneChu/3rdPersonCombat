using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float TransitionDuration = 0.1f;

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter() {
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage);

        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime) {
        Debug.Log(stateMachine.Animator);
        Debug.Log(GetNormalizedTime(stateMachine.Animator));
        //if (true) {
        if (GetNormalizedTime(stateMachine.Animator) >= 1) {
            Debug.Log("here");

            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    }

    public override void Exit() {
    }
}
