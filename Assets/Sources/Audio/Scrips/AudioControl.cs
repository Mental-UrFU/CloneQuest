using UnityEngine;
using UnityEngine.Audio;

public class AudioControl : MonoBehaviour
{
    public static AudioControl Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _audioSource;

    const string MusicKey = "Music";
    const string SoundKey = "Sound";

    public float Music
    {
        get => GetValue(MusicKey);
        set
        {
            SetValue(MusicKey, value);
            if (value > 0f && !_audioSource.isPlaying) { _audioSource.Play(); }
            else if (value <= 0f && _audioSource.isPlaying) { _audioSource.Pause(); }
        }
    }

    public float Sound
    {
        get => GetValue(SoundKey);
        set => SetValue(SoundKey, value);
    }

    public void Awake()
    {
        if (Instance) { Destroy(this); return; }
        DontDestroyOnLoad(this);
        Instance = this;
        Music = Music;
        Sound = Sound;
    }

    private float ConvertVolume(float normalizedVolume) => Mathf.Log10(normalizedVolume) * 20;

    private float GetValue(string key) => PlayerPrefs.GetFloat(key, 1f);
    private void SetValue(string key, float normalizedVolume)
    {
        PlayerPrefs.SetFloat(key, normalizedVolume);
        _audioMixer.SetFloat(key, ConvertVolume(normalizedVolume));
    }

#if UNITY_EDITOR
    [ContextMenu("Enable Music")] private void EnableMusic() => Music = 1f;
    [ContextMenu("Enable Sound")] private void EnableSound() => Sound = 1f;
    [ContextMenu("Disable Music")] private void DisableMusic() => Music = 0f;
    [ContextMenu("Disable Sound")] private void DisableSound() => Sound = 0f;
    private void OnValidate() { if (_audioSource == null) { _audioSource = GetComponent<AudioSource>(); } }
#endif
}
