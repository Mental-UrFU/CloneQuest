using UnityEngine;
using DG.Tweening;

public class RigidbodySoftReset : MonoSoftResetListener
{
    public (Vector2 position, float rotation) Initial { get => (_initialPosition, _initialRotation); set => (_initialPosition, _initialRotation) = value; }

    [SerializeField] private Rigidbody2D _rigidbody;
    private Vector2 _initialPosition;
    private float _initialRotation;

    public void SoftResetHandler(float duration)
    {
        _rigidbody.simulated = false;
        DOTween.Sequence().SetLink(_rigidbody.gameObject).SetEase(Ease.Linear)
            .Join(transform.DOMove(_initialPosition, duration))
            .Join(transform.DORotate(new(0, 0, _initialRotation), duration))
            .OnKill(() =>
            {
                _rigidbody.position = _initialPosition;
                _rigidbody.rotation = _initialRotation;
                _rigidbody.simulated = true;
            });
    }

    private new void Awake()
    {
        (_initialPosition, _initialRotation) = (_rigidbody.position, _rigidbody.rotation);;
        StartActions.AddListener(SoftResetHandler);
        base.Awake();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_rigidbody == null) { _rigidbody = GetComponent<Rigidbody2D>(); }        
    }
#endif
}
