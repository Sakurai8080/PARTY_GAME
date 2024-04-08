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

    private List<KeyValuePair<TimeSpan, string>> _timeNameDic = new List<KeyValuePair<TimeSpan, string>>();

    /// <summary>
    /// 名前と時間の紐づけ
    /// </summary>
    /// <param name="time">時間</param>
    /// <param name="name">名前</param>
    /// <param name="callBack">確認後のコールバック</param>
    /// <returns></returns>
    public void TimeNameAdd(TimeSpan time, string name, Action callBack = null)
    {
        _timeNameDic.Add(new KeyValuePair<TimeSpan, string>(time, name));
        Debug.Log($"<color=blue>時間 {time} : 名前 {name}</color>");
        try
        { 
            if (_timeNameDic.Count() >= NameLifeManager.Instance.GamePlayerAmount)
            {
                ResultCheck();
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
        IEnumerable<TimeSpan> timeGaps = _timeNameDic.Select(x => CalculateTimeGap(x.Key));
        var (maxGap, maxGapOrders) = GetMaxGapAndOrders(timeGaps);
        maxGapOrders.ForEach(order => LosePlayerProcess(order,maxGap));
        _nameSecondsDisplay.FinalResultTextChange(maxGapOrders.ToArray());
        DelayAndLoadSceneTask().Forget();
    }

    private TimeSpan CalculateTimeGap(TimeSpan measuredTime)
    {
        TimeSpan offsetTime = TimeSpan.FromMilliseconds(-1);
        TimeSpan correctTime = TimeSpan.FromSeconds(10);
        TimeSpan gap = correctTime - measuredTime;
        return (gap < TimeSpan.Zero) ? TimeSpan.FromTicks(-gap.Ticks)+ offsetTime: gap + offsetTime;
    }

    private (TimeSpan, List<int>) GetMaxGapAndOrders(IEnumerable<TimeSpan> timeGaps)
    {
        TimeSpan maxGap = TimeSpan.MinValue;
        List<int> maxGapOrders = new List<int>();

        string tempGap = "";
        for (int i = 0; i < timeGaps.Count(); i++)
        {
            TimeSpan gap = timeGaps.ElementAt(i);
            string gapFormat = gap.ToString(@"ss\.ff");
            Debug.Log($"{_timeNameDic[i].Value}は<color=yellow>{gapFormat}</color>");

            if (maxGap <= gap || tempGap == gapFormat)
            {
                if (maxGap < gap && tempGap != gapFormat)
                    maxGapOrders.Clear();

                tempGap = gapFormat;
                Debug.Log($"<color=red>{tempGap}</color>");
                maxGap = gap;
                maxGapOrders.Add(i);
            }
        }
        return (maxGap, maxGapOrders);
    }

    private void LosePlayerProcess(int order,TimeSpan maxGap)
    {
        NameLifeManager.Instance.ReduceLife(_timeNameDic[order].Value);
        Debug.Log($"{_timeNameDic[order].Value}は{maxGap}で負け");
    }

    private async UniTask DelayAndLoadSceneTask()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(4));
        GameManager.Instance.SceneLoader("GameSelect");
    }
}
