using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Space]
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _land;
    [SerializeField] private float _minLandingVelocity;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _portalEntry;
    [SerializeField] private AudioClip _softReset;
    [SerializeField] private AudioClip[] _steps;

    private readonly System.Random _random = new();

    public void Jump() { _audioSource.PlayOneShot(_jump); }
    public void Land(float velocity) { if (velocity >= _minLandingVelocity) { _audioSource.PlayOneShot(_land); } }
    public void Walk(float velocity)
    {
        if (Mathf.Abs(velocity) < 0.1f || _audioSource.isPlaying) { return; }
        _audioSource.PlayOneShot(_steps[_random.Next(0, _steps.Length - 1)]);
    }
    public void Kill() { _audioSource.PlayOneShot(_death); }
    public void PortalEntry() { _audioSource.PlayOneShot(_portalEntry); }
    public void SoftReset() { _audioSource.PlayOneShot(_softReset); }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_audioSource == null) { _audioSource = GetComponent<AudioSource>(); }
    }
#endif
}
