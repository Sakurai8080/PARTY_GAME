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

/// <summary>
/// 結果を表示するためのコンポーネント
/// </summary>
public class ResultUIsActivator : MonoBehaviour
{
    [Header("変数")]
    [Tooltip("名前とライフの結果を表示するUIリスト")]
    [SerializeField]
    private List<LifeImage> _nameLifeUIList = new List<LifeImage>();

    [Tooltip("負けた人を表示するUIグループ")]
    [SerializeField]
    private CanvasGroup _loseNameUIGroup = default;

    [Tooltip("次のシーンに移行するためのボタン")]
    [SerializeField]
    private CanvasGroup _ButtonUIGroup = default;

    [Tooltip("負けた人を表示するTMP")]
    [SerializeField]
    private TextMeshProUGUI _loseNameTMP = default;

    private CancellationTokenSource _cts;

    void Start()
    {
        _cts = new CancellationTokenSource();
        ResultUIsActiveTask(_cts.Token).Forget();
    }

    /// <summary>
    /// 結果を順番に表示するタスク
    /// </summary>
    /// <param name="cancellationToken"></param>
    private async UniTask ResultUIsActiveTask(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            _loseNameTMP.text = NameLifeManager.Instance.FinallyLoseName;
            _loseNameTMP.text = _loseNameTMP.text.Length > 5 ? _loseNameTMP.text.Substring(0, 5) : _loseNameTMP.text;

            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            for (int i = 0; i < NameLifeManager.Instance.GamePlayerAmount; i++)
            {
                _nameLifeUIList[i].gameObject.SetActive(true);
                await UniTask.Delay(TimeSpan.FromMilliseconds(450));
            }
            
            await UniTask.Delay(TimeSpan.FromMilliseconds(1000));
            
            _loseNameUIGroup.DOFade(1,1f)
                            .SetEase(Ease.Linear);

            await UniTask.Delay(TimeSpan.FromMilliseconds(500));
            
            _ButtonUIGroup.DOFade(1,1f)
                          .SetEase(Ease.Linear);
        }
        catch(OperationCanceledException)
        {
            Debug.Log("結果表示中にタスクはキャンセルされました。");
            CancelToken();
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
            CancelToken();
        }
        finally
        {
            CancelToken();
        }
        await UniTask.Delay(0);
    }
    
    private void CancelToken()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

    }

    private void OnDestroy()
    {
        CancelToken();
    }
}
