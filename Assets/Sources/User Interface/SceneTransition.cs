using System;
using DG.Tweening;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [SerializeField] private float _transitionTime;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show(Action callback = null)
    {
        _canvas.enabled = true;
        _canvasGroup.DOFade(1f, _transitionTime)
            .SetUpdate(true)
            .SetEase(Ease.InCubic)
            .OnComplete(OnShowCallback);
        void OnShowCallback() => callback?.Invoke();
    }

    public void Hide(Action callback = null)
    {
        _canvasGroup.DOFade(0f, _transitionTime)
            .SetUpdate(true)
            .SetEase(Ease.InCubic)
            .OnComplete(OnHideCallback);
        void OnHideCallback()
        {
            _canvas.enabled = false;
            callback?.Invoke();
        }
    }

    public void Awake()
    {
        if (Instance) { Destroy(this); return; }
        DontDestroyOnLoad(this);
        Instance = this;
    }

#if UNITY_EDITOR
    [ContextMenu("Show")] private void ShowMenu() => Show();
    [ContextMenu("Hide")] private void HideMenu() => Hide();
#endif
}
