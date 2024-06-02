using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class LevelFinisher : MonoBehaviour
{
    [SerializeField] private UnityEvent _onAnimationStart;
    [SerializeField] private UnityEvent _onAnimationEnd;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField][Range(0, 5)] private float _animationDuration;

    const string _foldAnimation = "Fold";

    public void LevelFinish(Vector2 moveTo)
    {
        _onAnimationStart.Invoke();
        _playerAnimator.SetTrigger(_foldAnimation);
        DOTween.Sequence().SetLink(gameObject).SetEase(Ease.InOutCubic)
            .Append(transform.DOMove((Vector3)moveTo, _animationDuration))
            .Join(transform.DOScale(0f, _animationDuration))
            .Join(transform.DORotate(new(0f, 0f, 360f), _animationDuration, RotateMode.FastBeyond360))
            .AppendCallback(OnAnimationEnd);
    }

    private void OnAnimationEnd()
    {
        _onAnimationEnd.Invoke();
        EventBus.Invoke<ILevelFinishHandler>((obj) => obj.OnLevelFinish());
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_sprite == null) { _sprite = GetComponentInChildren<SpriteRenderer>(); }
        if (_playerAnimator == null) { _playerAnimator = GetComponentInChildren<Animator>(); }
    }
#endif
}
