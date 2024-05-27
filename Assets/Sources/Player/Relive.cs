using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Relive : MonoBehaviour, IDamageable, ILevelSoftResetStartHandler
{
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent<float> _onReviveStart;
    [SerializeField] private UnityEvent _onRevive;

    private bool _isDead = false;

    public void ApplyDamage()
    {
        if (_isDead) { return; }
        _isDead = true;
        _onDeath.Invoke();
    }

    public void Revive(float time = 0f)
    {
        if (!_isDead) { return; }
        _onReviveStart.Invoke(time);
        StartCoroutine(ReliveRoutine(time));
    }

    private IEnumerator ReliveRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        _isDead = false;
        _onRevive.Invoke();
    }

    public void OnSoftResetStart(float duration) => Revive(duration);

    private void OnEnable() => EventBus.Subscribe<ILevelSoftResetStartHandler>(this);
    private void OnDisable() => EventBus.Unsubscribe<ILevelSoftResetStartHandler>(this);
}
