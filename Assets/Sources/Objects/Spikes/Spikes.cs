using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<IDamageable>(out var damageable)) { return; }
        damageable.ApplyDamage();
    }
}
