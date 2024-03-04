using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class RouletteController : SingletonMonoBehaviour<RouletteController>
{
    [SerializeField]
    private GameObject _rouletteObject;

    [SerializeField]
    private TextMeshProUGUI _text;

    private Tween _currentTween;
    

    public void RouletteRotate(int count)
    {
        if (count > 1)
            return;
        else if (count == 1)
        {
            float rondomZvalue = UnityEngine.Random.Range(1080, 1440);
            _currentTween.Kill();
            _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, rondomZvalue), 4f, RotateMode.FastBeyond360)
                                                     .SetEase(Ease.OutBack)
                                                     .OnComplete(async () =>
                                                     {
                                                         _text.DOColor(Color.blue, 0);
                                                         await UniTask.Delay(TimeSpan.FromSeconds(1));
                                                         GameManager.Instance.SceneLoader("MainScene");
                                                     });
        }
        else if (count == 0)
        {
            _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, 720), 1f,RotateMode.FastBeyond360)
                                                     .SetEase(Ease.InBack)
                                                     .OnComplete(() =>
                                                     {
                                                         _currentTween.Kill();
                                                         _currentTween = _rouletteObject.transform.DOLocalRotate(new Vector3(0, 0, 20), 0.01f)
                                                                                                  .SetEase(Ease.Linear)
                                                                                                  .SetLoops(-1, LoopType.Incremental);
                                                     });
        }
    }
}
