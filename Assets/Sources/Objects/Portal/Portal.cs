using UnityEngine;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent<LevelFinisher>(out var levelFinisher)) { return; }
        levelFinisher.LevelFinish(transform.position);
    }
    
}
