using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovablePlatform : MonoBehaviour
{
    [Header("Move Parameters")]
    [SerializeField] private bool _shouldMove = true;
    [SerializeField] private Vector2 _velocity = Vector2.zero;
    [Header("Move Bounds")]
    [SerializeField] private float _leftBoundX = 0f;
    [SerializeField] private float _rightBoundX = 0f;
    [SerializeField] private float _downBoundY = 0f;
    [SerializeField] private float _upBoundY = 0f;
    [SerializeField] private Rigidbody2D _rigidbody;

    private HashSet<Rigidbody2D> _bodies = new HashSet<Rigidbody2D>();

    void FixedUpdate()
    {
        if (!_shouldMove)
            return; 
        _rigidbody.velocity = _velocity;
        bool snapBodies = false;
        //if we moved outside the box; just reverse velocity params and snap them inside the bounding box
        if (_rigidbody.position.x < _leftBoundX || _rigidbody.position.x > _rightBoundX)
        {
            _velocity.x = -_velocity.x;
            _rigidbody.position += _velocity * Time.fixedDeltaTime;
            snapBodies = true;
        }
            
        if (_rigidbody.position.y < _downBoundY || _rigidbody.position.y > _upBoundY)
        {
            _velocity.y = -_velocity.y;
            _rigidbody.position += _velocity * Time.fixedDeltaTime;
            snapBodies = true;
        }
        
        foreach (var body in _bodies)
        {
            body.velocity += _velocity;
            if (snapBodies)
                body.position += _velocity * Time.fixedDeltaTime;
        }
    }

    void SubscribeObjectOnCollisionMovement(Rigidbody2D obj)
    {
        _bodies.Add(obj);
    }

    void UnSubscribeObjectOnCollisionMovement(Rigidbody2D obj)
    {
        _bodies.Remove(obj);
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        var body = collision.gameObject.GetComponent<Rigidbody2D>();
        SubscribeObjectOnCollisionMovement(body);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        var body = collision.gameObject.GetComponent<Rigidbody2D>();
        UnSubscribeObjectOnCollisionMovement(body);
    }

    private void OnValidate()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

}
