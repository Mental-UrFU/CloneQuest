using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _overlay;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private LevelCompleteScreenStarsContainer _stars;
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private float _animationTime;

    public void Show(int levelNumber, int starCount)
    {
        Pause.Set(true);
        _overlay.alpha = 0;
        _levelName.text = $"Уровень {levelNumber}";
        _overlay.gameObject.SetActive(true);
        _panel.localScale = Vector3.zero;
        _stars.Reset();
        DOTween.Sequence().SetLink(gameObject).SetUpdate(true)
            .Join(_overlay.DOFade(1f, _animationTime).SetEase(Ease.InOutCubic))
            .Join(_panel.DOScale(1f, _animationTime).SetEase(Ease.OutElastic))
            .AppendCallback(() => _stars.Show(starCount));
        _restartButton.onClick.AddListener(() => EventBus.Invoke<ILevelRestartHandler>(obj => obj.OnLevelRestart()));
        _nextButton.onClick.AddListener(() => EventBus.Invoke<ILevelLoadNextHandler>(obj => obj.OnLoadNext()));
        _menuButton.onClick.AddListener(() => EventBus.Invoke<ILevelMenuLoadHandler>(obj => obj.OnLoadMenu()));
    }
}
