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
                       AllUIsAnimationController.Instance.InitButtonSetUp(_bombAnim.InitialColor);
                       StartCoroutine(AllUIsAnimationController.Instance.PauseTweens());
                       _bombAnim.SelectedAnimation();
                   });
    }
}
