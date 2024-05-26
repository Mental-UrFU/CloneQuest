using UnityEngine;

public class CameraSizeAdjuster : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _size;

    private void Start() => Resize();
    private void LateUpdate() => Resize();

    private void Resize()
    {
        var levelRatio = _size.x / _size.y;
        var screenRatio = (float)Screen.width / Screen.height;
        _camera.orthographicSize = ((screenRatio < levelRatio) ? _size.x / screenRatio : _size.y) / 2;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_camera == null) { _camera = Camera.main; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position, _size);
    }
#endif
}
