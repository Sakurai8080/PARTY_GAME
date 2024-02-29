using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class BombSelectButton : MonoBehaviour
{
    [SerializeField]
    Button _bombButton = default;

    private BombAnim _bombAnim;

    private void Start()
    {
        _bombAnim = GetComponent<BombAnim>();

        _bombButton.OnClickAsObservable()
                   .TakeUntilDestroy(this)
                   .Subscribe(_ =>
                   {
                       BombManager.IsCheckingValid(true);
                       AllBombAnimationController._resetColor = _bombAnim.InitialColor;
                       StartCoroutine(AllBombAnimationController.PauseTweens());
                       _bombAnim.SelectedAnimation();
                   });
    }
}
