using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;

/// <summary>
/// 10秒ゲーム全体の管理
/// </summary>
public class SecondsGameManager : SingletonMonoBehaviour<SecondsGameManager>
{
    [Header("変数")]
    [Tooltip("テキスト操作コンポーネント")]
    [SerializeField]
    private　SecondsDisplay _nameSecondsDisplay;

    private Dictionary<TimeSpan, string> _timeNameDic = new Dictionary<TimeSpan, string>();
    private TimeSpan _correctTime = TimeSpan.FromSeconds(10);
    private int _resultLoseOrder = 0;

    /// <summary>
    /// 名前と時間の紐づけ
    /// </summary>
    /// <param name="time">時間</param>
    /// <param name="name">名前</param>
    /// <param name="callBack">確認後のコールバック</param>
    /// <returns></returns>
    public async UniTask TimeNameAdd(TimeSpan time, string name, Action callBack = null)
    {
        _timeNameDic.Add(time, name);
        Debug.Log($"時間 {time} : 名前 {name}");
        try
        {
            if (_timeNameDic.Count() >= NameLifeManager.Instance.GamePlayerAmount)
            {
                ResultCheck();
                _nameSecondsDisplay.FinalResultTextChange(_resultLoseOrder);
                await UniTask.Delay(TimeSpan.FromSeconds(4));
                GameManager.Instance.SceneLoader("GameSelect");
                return;
            }
            callBack?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"カウント確認中にエラー: {ex}");
        }
    }
    
    /// <summary>
    /// 最終の乖離チェック
    /// </summary>
    private void ResultCheck()
    {
        TimeSpan currentMaxGapTime = TimeSpan.MinValue;
        TimeSpan loseTime = TimeSpan.MinValue;
        int currentOrder = 0;
        foreach (var item in _timeNameDic.Keys)
        {
            TimeSpan gap = _correctTime - item;

            gap = (gap < TimeSpan.Zero) ? TimeSpan.FromTicks(-gap.Ticks) : gap;

            Debug.Log($"<color=red>{gap}</color>");
            if (currentMaxGapTime < gap)
            {
                currentMaxGapTime = gap;
                _resultLoseOrder = currentOrder;
                loseTime = item;
            }
            currentOrder++;
        }
        string loseName = _timeNameDic[loseTime];
        NameLifeManager.Instance.ReduceLife(loseName);
    }
}
