using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool IsOpened => gameObject.activeSelf;

    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _resumeButton;
    [Space]
    [SerializeField] private CanvasGroup _overlay;
    [SerializeField] private VerticalLayoutGroup _panel;
    [Space]
    [SerializeField] private float _animationTime;
    [SerializeField] private float _panelButtonsSize;
    [SerializeField] private float _panelSpacing;

    private PlayerActions _input;

    public void Start()
    {
        _mainMenuButton.onClick.AddListener(() => EventBus.Invoke<ILevelMenuLoadHandler>(obj => obj.OnLoadMenu()));
        _restartButton.onClick.AddListener(() => EventBus.Invoke<ILevelRestartHandler>(obj => obj.OnLevelRestart()));
        _resumeButton.onClick.AddListener(Hide);
        _input = new();
        _input.Game.Esc.started += (ctx) => Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        DOTween.Sequence().SetLink(gameObject).SetEase(Ease.InOutCubic).SetUpdate(true)
            .Join(_overlay.DOFade(1f, _animationTime))
            .Join(DOVirtual.Float(-_panelButtonsSize, _panelSpacing, _animationTime, (value) => _panel.spacing = value))
            .OnComplete(() => _input.Enable());
    }

    public void Hide()
    {
        DOTween.Sequence().SetLink(gameObject).SetEase(Ease.InOutCubic).SetUpdate(true)
            .Join(_overlay.DOFade(0f, _animationTime))
            .Join(DOVirtual.Float(_panelSpacing, -_panelButtonsSize, _animationTime, (value) => _panel.spacing = value))
            .AppendCallback(() => gameObject.SetActive(false))
            .OnComplete(() => _input.Disable());
    }

    private void OnEnable() => Pause.Set(true);
    private void OnDisable() => Pause.Set(false);
}
