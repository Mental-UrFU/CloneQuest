using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerControls))]
[RequireComponent(typeof(Sensor))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerControls _controls;
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Sensor _groundSensor;

    [SerializeField] private UnityEvent<bool> _groundState;
    [SerializeField] private UnityEvent<float> _moveVelocity;
    [SerializeField] private UnityEvent<int> _moveDirection;
    [SerializeField] private UnityEvent<float> _airVelocity;
    [SerializeField] private UnityEvent<float> _land;
    [SerializeField] private UnityEvent _jump;

    private Vector2 _frameVelocity;
    private Vector2 _platformVelocity;

    private void Init()
    {
        _wasGrounded = _groundSensor.IsHit;
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        HandleVelocity();
        HandleMove();
        HandleJump();
        HandleGravity();
        ApplyVelocity();
    }

    # region Collisions

    private bool _isGrounded;
    private bool _wasGrounded;
    private float _lastGroundTime;
    private float _groundAngle;

    private void CheckCollisions()
    {
        _isGrounded = _groundSensor.IsHit;
        if (_isGrounded)
        {
            var hit = _groundSensor.Hit;
            _lastGroundTime = Time.fixedTime;
            _groundAngle = Vector2.Angle(_groundSensor.Hit.normal, Vector2.up);
            if (!_wasGrounded)
            {
                _rigidbody.position -= new Vector2(0, hit.distance);
                _wasGrounded = true;
                _coyoteLocked = false;
                _groundState.Invoke(_wasGrounded);
                _land.Invoke(-_frameVelocity.y + _platformVelocity.y);
            }
        }
        else
        {
            _airVelocity.Invoke(_frameVelocity.y);
            if (_wasGrounded)
            {
                _wasGrounded = false;
                _groundState.Invoke(_wasGrounded);
            }
        }
    }

    #endregion

    #region Frame Velocity

    private void HandleVelocity()
    {
        _frameVelocity = _rigidbody.velocity;
        var platform = _groundSensor.Hit.rigidbody;
        _platformVelocity = platform ? platform.velocity : Vector2.zero;
    }

    #endregion

    #region Move

    private void HandleMove()
    {
        var move = _controls.Move;
        var direction = MathF.Sign(move);
        _moveDirection.Invoke(direction);
        if (_isGrounded)
        {
            if (_groundAngle > _config.MaxSurfaceAngle) { return; }
            var groundNormal = _groundSensor.Hit.normal;
            var alongGround = new Vector2(groundNormal.y, -groundNormal.x);
            var relativeVelocity = _frameVelocity - _platformVelocity;
            _moveVelocity.Invoke(relativeVelocity.x);
            _frameVelocity = ((direction == 0)
                ? Vector2.MoveTowards(relativeVelocity, Vector2.zero, _config.Deceleration * Time.fixedDeltaTime)
                : Vector2.MoveTowards(relativeVelocity, _config.Velocity * move * alongGround,
                    (direction != -MathF.Sign(relativeVelocity.x)
                        ? _config.Acceleration
                        : MathF.Max(_config.Acceleration, _config.Deceleration))
                    * Time.fixedDeltaTime))
                + _platformVelocity;
        }
        else
        {
            _frameVelocity.x = (direction == 0)
                ? Mathf.MoveTowards(_frameVelocity.x, 0, _config.AirDeceleration * Time.fixedDeltaTime)
                : Mathf.MoveTowards(_frameVelocity.x, move * _config.AirVelocity, _config.AirAcceleration * Time.fixedDeltaTime);
        }
    }

    #endregion Move

    #region Jump

    private bool _jumpEarlyEnded;
    private bool _coyoteLocked;
    private bool CanUseCoyote => !_coyoteLocked && _lastGroundTime + _config.CoyoteTime >= Time.fixedTime;
    private void HandleJump()
    {
        var jump = _controls.Jump;
        var jumpWasReleased = _controls.PopJumpReleasedState();
        if (!_jumpEarlyEnded && !_isGrounded && jumpWasReleased && _frameVelocity.y > 0) { _jumpEarlyEnded = true; }
        if ((_isGrounded || CanUseCoyote) && jump && jumpWasReleased && _groundAngle <= _config.MaxSurfaceAngle) { ExecuteJump(); }
    }
    private void ExecuteJump()
    {
        _jumpEarlyEnded = false;
        _frameVelocity.y = _config.JumpVelocity;
        _coyoteLocked = true;
        _jump.Invoke();
    }

    #endregion

    #region HandleGravity

    private void HandleGravity()
    {
        var gravity = _rigidbody.gravityScale * Physics2D.gravity;
        if (!_isGrounded)
        {
            if (_jumpEarlyEnded && _frameVelocity.y > 0) { gravity *= _config.JumpEndEarlyGravityModifier; }
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, _config.MaxFallVelocity, gravity.y * Time.deltaTime);
            return;
        }
        if (_groundAngle <= _config.MaxSurfaceAngle)
        {
            _rigidbody.AddForce(gravity * -_rigidbody.mass);
        }
    }

    #endregion

    private void ApplyVelocity() => _rigidbody.velocity = _frameVelocity;

    private void OnEnable() { Init(); }
    private void OnDisable() { _rigidbody.velocity = Vector2.zero; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_rigidbody) { _rigidbody = GetComponent<Rigidbody2D>(); }
        if (!_controls) { _controls = GetComponent<PlayerControls>(); }
        if (!_groundSensor) { Debug.LogWarning("Sensor not attached"); }
        if (!_config) { Debug.LogWarning("Config not attached"); }
    }
#endif
}
