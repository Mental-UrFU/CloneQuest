using UnityEngine;
using UnityEngine.Events;

public class Spikes : MonoBehaviour
{
    [SerializeField] private UnityEvent _onApplyDamage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent<IDamageable>(out var damageable)) { return; }
        _onApplyDamage.Invoke();
        damageable.ApplyDamage();
    }
}
