using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// 名前とライフを管理するマネージャー
/// </summary>
public class NameLifeManager : SingletonMonoBehaviour<NameLifeManager>
{
    public List<string> NameList => _nameList;
    public Dictionary<string, int> NameLifeDic => _nameLifeDic;
    public int GamePlayerAmount => _gamePlayerAmount;
    public string FinallyLoseName => _finallyLoseName;
    
    private List<string> _nameList = new List<string>();
    private Dictionary<string, int> _nameLifeDic = new Dictionary<string, int>();
    private int _gamePlayerAmount = 0;
    private int _currentOrder = 0;
    private string _finallyLoseName = default;

    /// <summary>
    /// 名前とライフポイント3の初期設定
    /// </summary>
    /// <param name="names">参加者の名前</param>
    public void Setup(HashSet<string> names)
    {
        if (names.Count() >= 1)
        {
            _gamePlayerAmount = names.Count();
            names.ToList().ForEach(name => _nameLifeDic.Add(name, 3));
            _nameList.AddRange(names);
        }
    }

    /// <summary>
    /// 負けたプレイヤーのライフを減らす処理
    /// </summary>
    /// <param name="loseName">負けたプレイヤーの名前</param>
    public void ReduceLife(string loseName)
    {
        _nameLifeDic[loseName]--;
         
        foreach (var item in _nameLifeDic.Keys)
        {
            Debug.Log($"{item} : {_nameLifeDic[item]}");
        }

        if (_nameLifeDic[loseName] <= 0)
        {
            _finallyLoseName = loseName;
        }
    }

    /// <summary>
    /// ライフを増やす処理
    /// </summary>
    /// <param name="loseName">回復するプレイヤーの名前</param>
    public void IncreaseLife(string lifeUpName)
    {
        _nameLifeDic[lifeUpName] += _nameLifeDic[lifeUpName] != 3 ? 1 : 0;
        foreach (var item in _nameLifeDic.Keys)
            Debug.Log($"{item} : {_nameLifeDic[item]}");
    }


    /// <summary>
    /// 名前からライフの確認
    /// </summary>
    /// <param name="checkName">確認する名前</param>
    /// <returns>ライフ数</returns>
    public int NamefromLifePass(string checkName)
    {
        return _nameLifeDic[checkName];
    }

    /// <summary>
    /// 順序を入れ替える処理
    /// </summary>
    public void NameListOrderChange()
    {
        _currentOrder++;
        if (_currentOrder >= _gamePlayerAmount)
        {
            _currentOrder = 0;
        }
    }

    /// <summary>
    /// 順序が1番目のプレイヤーを取得
    /// </summary>
    /// <returns></returns>
    public string CurrentNameReceiver()
    {
        return _nameList[_currentOrder];
    }
}
