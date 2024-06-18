using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System.Threading;

public class ResultUIsActivator : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("名前とライフの結果を表示するUIリスト")]
    [SerializeField]
    private List<LifeImage> _nameLifeUIList = new List<LifeImage>();

    [Tooltip("")]
    [SerializeField]
    private CanvasGroup _loseNameUIGroup = default;

    [Tooltip("")]
    [SerializeField]
    private CanvasGroup _ButtonUIGroup = default;

    [Tooltip("")]
    [SerializeField]
    private TextMeshProUGUI _loseNameTMP = default;


    // Start is called before the first frame update
    void Start()
    {
        var cts = new CancellationTokenSource();
        ResultUIsActiveTask(cts.Token).Forget();
    }

    private async UniTask ResultUIsActiveTask(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            for (int i = 0; i < NameLifeManager.Instance.GamePlayerAmount; i++)
            {
                _nameLifeUIList[i].gameObject.SetActive(true);
                await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            }
            
            _loseNameTMP.text = NameLifeManager.Instance.FinallyLoseName;
            await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
            
            _loseNameUIGroup.DOFade(1,1f)
                            .SetEase(Ease.Linear);

            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            
            _ButtonUIGroup.DOFade(1,1f)
                          .SetEase(Ease.Linear);
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
        await UniTask.Delay(0);
    }
    
    private void CancelToken(CancellationTokenSource cancellationTokenSource)
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();

    }
}
