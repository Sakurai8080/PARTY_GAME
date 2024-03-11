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

public class BallUIsController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;

    [SerializeField]
    private Button _ballNamedButon = default;

    [SerializeField]
    private Ball _targetBall;

    private RectTransform _nameTextRect;
    private RectTransform _buttonRect;
    private Vector3 _offset = new Vector3(0,0.2f, 0);

    void Start()
    {
        _nameTextRect = _nameText.GetComponent<RectTransform>();
        _buttonRect = _ballNamedButon.GetComponent<RectTransform>();
        _buttonRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main, _targetBall.transform.position);

        BallGameManager.Instance.InGameObserver
                                .TakeUntilDestroy(this)
                                .Subscribe(value =>
                                {
                                    if (value == true)
                                    {
                                        TextOffsetChange();
                                    }
                                });

        _ballNamedButon.OnClickAsObservable()
                       .TakeUntilDestroy(this)
                       .Subscribe(_ =>
                       {
                           _ballNamedButon.interactable = false;
                           BallController.Instance.BallAddDictionary(_targetBall);
                           TextSetup();
                           BallGameManager.Instance.ChooseBall();
                           NameLifeManager.Instance.NameListOrderChange();
                       });

    }

    void FixedUpdate()
    {
        _nameTextRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main,_targetBall.transform.position + _offset);
    }

    private void TextSetup()
    {
        _nameText.text = NameLifeManager.Instance.CurrentNameReciever();
        _nameText.DOFade(1, 0.25f);
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
