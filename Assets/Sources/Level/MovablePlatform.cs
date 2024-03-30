using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MovablePlatform : MonoBehaviour
{
    [Header("Move Parameters")]
    [SerializeField] private bool _shouldMove = false;
    [SerializeField] private Vector2 _velocity = Vector2.zero;
    [Header("Move Bounds")]
    [SerializeField] private PlatformMovementType _movementType;

    [SerializeField] private float _leftBoundX = 0f;
    [SerializeField] private float _rightBoundX = 0f;
    [SerializeField] private float _downBoundY = 0f;
    [SerializeField] private float _upBoundY = 0f;

    private HashSet<Rigidbody2D> _bodies = new HashSet<Rigidbody2D>();

    public enum PlatformMovementType
    {
        None,
        Vertical,
        Horizontal,
        Diagonal
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_velocity.x, _velocity.y) * Time.deltaTime;
        //if we moved outside the box; just reverse velocity params
        if (transform.position.x < _leftBoundX || transform.position.x > _rightBoundX)
            _velocity.x = -_velocity.x;
        if (transform.position.y < _downBoundY || transform.position.y > _upBoundY)
            _velocity.y = -_velocity.y;
        foreach (var body in _bodies)
        {
            body.transform.position += new Vector3(_velocity.x, _velocity.y) * Time.deltaTime;
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
}
