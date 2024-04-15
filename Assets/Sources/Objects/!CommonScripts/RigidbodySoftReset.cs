using UnityEngine;

public class RigidbodySoftReset : MonoBehaviour, ISoftReset
{
    [SerializeField] private Rigidbody2D _rigidbody;
    private Vector2 _initialPosition;
    private float _initialRotation;

    public void SoftReset(float duration)
    {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
        _rigidbody.position = _initialPosition;
        _rigidbody.rotation = _initialRotation;
    }

    private void Awake()
    {
        _initialPosition = _rigidbody.position;
        _initialRotation = _rigidbody.rotation;
        EventBus.Subscribe<ISoftReset>(this);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ISoftReset>(this);
    }

    private void OnValidate()
    {
        if (_rigidbody == null) { _rigidbody = GetComponent<Rigidbody2D>(); }
    }
}