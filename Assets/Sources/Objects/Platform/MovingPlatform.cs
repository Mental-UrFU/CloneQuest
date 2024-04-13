using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rigidbody;

    private IEnumerator Start()
    {
        var velocity = 5f;
        var wait = 3f;
        while (true){
            _rigidbody.velocity = Vector2.right * velocity;
            yield return new WaitForSeconds(wait);
            _rigidbody.velocity = Vector2.left * velocity;
            yield return new WaitForSeconds(wait);
        }
    }

    private void OnValidate()
    {
        if (_rigidbody == null) { _rigidbody = GetComponent<Rigidbody2D>(); }
    }
}
