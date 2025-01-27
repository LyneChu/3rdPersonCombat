using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    int damage;
    float knockback;

    private List<Collider> alreadyCollidedWith = new();

    private void OnEnable() {
        alreadyCollidedWith.Clear();
    }

    public void SetAttack(int damage, float knockback) {
        this.damage = damage;
        this.knockback = knockback;
    }

    private void OnTriggerEnter(Collider other) {
        if (other == myCollider)
            return;

        if (alreadyCollidedWith.Contains(other))
            return;

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health)) {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver)) {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }


}
