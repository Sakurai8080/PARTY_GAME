using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class NameTextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private Transform _targetBallTransform;

    private RectTransform _nameTextRect;
    private Vector3 _offset = new Vector3(0,0.2f, 0);

    void Start()
    {
        _nameTextRect = GetComponent<RectTransform>();

        BallGameManager.Instance.InGameObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value =>
                            {
                                if (value == true)
                                {
                                    TextOffsetChange();
                                }
                            });
    }

    void FixedUpdate()
    {
        _nameTextRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main,_targetBallTransform.position + _offset);
    }

    private void TextOffsetChange()
    {
        Vector3 onGameOffset = new Vector3(0, 0, 0.2f);
        float duration = 6f;
        TextFadeSwitcher(0.25f);
        DOTween.To(() => _offset, (value) => _offset = value, onGameOffset, duration).SetEase(Ease.InOutSine);
    }

    private void TextFadeSwitcher(float duration)
    {
        _nameText.DOFade(0, duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(async () =>
                 {
                     await UniTask.Delay(TimeSpan.FromSeconds(4));
                     duration = 3;
                     _nameText.DOFade(1, duration)
                              .SetEase(Ease.InFlash);
                 });
    }
}
