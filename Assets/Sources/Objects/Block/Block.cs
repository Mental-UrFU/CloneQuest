using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed = 1.0f;

    [SerializeField] 
    private Rigidbody2D _rb;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            Vector2 clampVel = _rb.velocity;
            var attachedRbMagnitude = hit.collider.attachedRigidbody.velocity.magnitude;
            clampVel.x = Mathf.Clamp(clampVel.x, -maxSpeed - attachedRbMagnitude, maxSpeed + attachedRbMagnitude);
            _rb.velocity = clampVel;
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    //Slows down block on collision exit, so it won't slide
    //    _rb.velocity *= 0.1f; 
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Slows down block on collision, so it won't slide
        _rb.velocity *= 0.1f; 
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }
#endif
}
