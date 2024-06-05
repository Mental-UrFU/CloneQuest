using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelCompleteScreenStarsContainer : MonoBehaviour
{
    [SerializeField] private UnityEvent _onStarShow;
    [SerializeField] private Image[] _stars;
    [SerializeField] float _animationTime;

    public void Reset() { foreach (var star in _stars) { star.rectTransform.localScale = Vector3.zero; } }
    public void Show(int starCount)
    {
        for (var i = 0; i < Math.Min(starCount, _stars.Length); i++)
        {
            var star = _stars[i];
            DOTween.Sequence().SetLink(gameObject).SetUpdate(true)
                .Join(star.rectTransform.DOScale(1f, _animationTime).SetEase(Ease.OutElastic).SetDelay(i * _animationTime))
                .JoinCallback(() => _onStarShow.Invoke());
        }
    }
}
