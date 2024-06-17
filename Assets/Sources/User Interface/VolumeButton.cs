using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class VolumeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Type { Music, Sound }
    [SerializeField] private Type _type;
    [SerializeField] private RectTransform _container;
    [SerializeField] private Button _button;
    [SerializeField] private Image _crossImage;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _collapsedWidth;
    [SerializeField] private float _width;
    [SerializeField] private float _animationDuration;

    private void Start()
    {
        var volume = _type == Type.Music ? AudioControl.Instance.Music : AudioControl.Instance.Sound;
        var enabled = volume > 0f;
        var crossColor = _crossImage.color;
        _container.sizeDelta = new Vector2(_collapsedWidth, _container.sizeDelta.y);
        crossColor.a = enabled ? 0f : 1f;
        _crossImage.color = crossColor;
        _slider.value = volume;
        _slider.onValueChanged.AddListener(OnValueChange);
        _button.onClick.AddListener(OnClick);
    }

    private void OnValueChange(float value)
    {
        if (_type == Type.Music) { AudioControl.Instance.Music = value; }
        else { AudioControl.Instance.Sound = value; }
        var crossColor = _crossImage.color;
        crossColor.a = value > 0f ? 0f : 1f;
        _crossImage.color = crossColor;
    }

    private void OnClick()
    {
        var volume = _type == Type.Music ? AudioControl.Instance.Music : AudioControl.Instance.Sound;
        var enabled = volume > 0f;
        var crossColor = _crossImage.color;
        if (enabled)
        {
            crossColor.a = 1f;
            _crossImage.color = crossColor;
            if (_type == Type.Music) { AudioControl.Instance.Music = 0f; }
            else { AudioControl.Instance.Sound = 0f; }
        }
        else
        {
            crossColor.a = 0f;
            _crossImage.color = crossColor;
            _slider.value = Mathf.Max(0.1f, _slider.value);
            if (_type == Type.Music) { AudioControl.Instance.Music = _slider.value; }
            else { AudioControl.Instance.Sound = _slider.value; }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _container.DOKill();
        _container.DOSizeDelta(new Vector2(_width, _container.sizeDelta.y), _animationDuration).SetLink(_container.gameObject).SetEase(Ease.InOutCubic).SetUpdate(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _container.DOKill();
        _container.DOSizeDelta(new Vector2(_collapsedWidth, _container.sizeDelta.y), _animationDuration).SetLink(_container.gameObject).SetEase(Ease.InOutCubic).SetUpdate(true);
    }
}
