using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    private void Start() {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll) {
        foreach (var collider in allColliders) {
            if (collider.gameObject.CompareTag("Ragdoll"))
                collider.enabled = isRagdoll;
        }

        foreach (var rigidbody in allRigidbodies) {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
                rigidbody.isKinematic = !isRagdoll;
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }


}
