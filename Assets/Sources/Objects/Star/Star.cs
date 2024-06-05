using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour, ILevelSoftResetEndHandler
{
    [SerializeField] private UnityEvent _onPickup;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _animationTime;

    private bool _enabled = true;

    public void OnSoftResetEnd()
    {
        _enabled = true;
        _collider.enabled = _enabled;
        transform.DOScale(1f, _animationTime).SetLink(gameObject).SetEase(Ease.OutElastic);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_enabled) { return; }
        _enabled = false;
        _collider.enabled = _enabled;
        _onPickup.Invoke(); transform.DOScale(0f, _animationTime).SetLink(gameObject).SetEase(Ease.InOutCubic);
        EventBus.Invoke<IStarCollectHandler>(x => x.OnStarCollected());
    }

    private void Awake() => EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
    private void OnDestroy() => EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_collider == null) { _collider = GetComponent<Collider2D>(); }
    }
#endif
}
