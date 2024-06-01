using System;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour, ILevelSoftResetEndHandler
{
    [SerializeField] private bool _facingRightDefault;
    [SerializeField] private Transform _flipAnchor;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] Animator _animator;

    const string _moveVelocityKey = "Move";
    const string _airVelocityKey = "Air";
    const string _groundKey = "OnGround";
    const string _airKey = "OnAir";

    public void ChangeGroundState(bool onGround) => _animator.SetTrigger(onGround ? _groundKey : _airKey);
    public void ChangeMoveVelocity(float velocity) => _animator.SetFloat(_moveVelocityKey, MathF.Abs(velocity));
    public void ChangeAirVelocity(float velocity) => _animator.SetFloat(_airVelocityKey, velocity);
    public void ChangeDirection(int direction) => UpdateDirection(direction);
    public void Kill()
    {
        const float time = 1f;
        _sprite.DOFade(0f, time).SetLink(gameObject);
    }
    public void Relive(float time)
    {
        _sprite.DOFade(1f, time).SetLink(gameObject);
    }

    #region rotation

    private bool _facingRight;
    private bool _previousFacingRight;

    private bool UpdateDirection(int direction)
    {
        _facingRight = direction == 0 ? _facingRight : direction > 0;
        if (_previousFacingRight == _facingRight)
            return false;
        _previousFacingRight = _facingRight;
        SetRotation(_facingRight);
        return true;
    }

    private void SetRotation(bool facingRight)
    {
        _facingRight = facingRight;
        var localScale = _flipAnchor.localScale;
        localScale.x = facingRight ? 1f : -1f;
        _flipAnchor.localScale = localScale;
    }

    private void ResetRotation()
    {
        _previousFacingRight = _facingRightDefault;
        SetRotation(_facingRightDefault);
    }

    #endregion

    public void OnSoftResetEnd() => ResetRotation();

    private void Awake()
    {
        EventBus.Subscribe<ILevelSoftResetEndHandler>(this);
        ResetRotation();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ILevelSoftResetEndHandler>(this);
    }
}
