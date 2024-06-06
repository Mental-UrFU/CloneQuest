using UnityEngine;
using DG.Tweening;

public class PressAnyButtonScreen : MonoBehaviour
{
    [SerializeField] private float _animationTime;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasGroup.DOFade(1f, _animationTime)
            .SetLink(gameObject)
            .SetUpdate(true);
    }

    public void Hide()
    {
        _canvasGroup.DOFade(0f, _animationTime)
            .SetLink(gameObject)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
