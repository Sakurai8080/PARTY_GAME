using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

//ボールと名前の紐づけ
public class BallGameManager : SingletonMonoBehaviour<BallGameManager>
{
    [SerializeField]
    private Button _fallButton = default;

    [SerializeField]
    private TextMeshProUGUI _explonationText = default;

    public IReadOnlyReactiveProperty<bool> InGame => _inGame;

    public IObservable<bool> InGameObserver => _inGame;

    private ReactiveProperty<bool> _inGame = new ReactiveProperty<bool>();

    private int _chooseBallCount = 0;

    public void ChooseBall()
    {
        _chooseBallCount++;
        if (_chooseBallCount == NameLifeManager.Instance.GamePlayerAmount)
        {
            _explonationText.gameObject.SetActive(false);
            _fallButton.gameObject.SetActive(true);
            _fallButton.image.DOFade(1, 0.25f)
                             .SetEase(Ease.InQuad);
        }
    }

    public void GameStateSwitcher(bool onValid)
    {
        _inGame.Value = onValid;
    }
}
