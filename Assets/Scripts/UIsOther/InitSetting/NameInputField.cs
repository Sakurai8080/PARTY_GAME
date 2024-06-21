using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using TMPro;

/// <summary>
/// 名前を入力するフィールド
/// </summary>
public class NameInputField : MonoBehaviour
{
    public IObservable<Unit> AllEndEditObserver => _allEndEditSubject;
    public IObservable<Unit> NamedFailObserver => _namedFailSubject;

    [Header("変数")]
    [Tooltip("全入力フィールド")]
    [SerializeField]
    private TMP_InputField[] _nameField = default;

    private HashSet<string> _namesSet = new HashSet<string>();
    private int _gamePlayerAmount = 0;
    private int _onEndEditCount = 0;

    private Subject<Unit> _allEndEditSubject = new Subject<Unit>();
    private Subject<Unit> _namedFailSubject = new Subject<Unit>();

    private void Start()
    {
        for (int i = 0; i < _gamePlayerAmount; i++)
            _nameField[i].onEndEdit.AddListener(inputName => AllOnEditChecker(inputName));
    }

    /// <summary>
    /// 名付け完了後に問題が無いか確認
    /// </summary>
    public void NameAndCountChecker()
    {
        _namesSet.Clear();
        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            string currentName =  (_nameField[i].text != "") ? _nameField[i].text : $"P{i+1}" ;
            _namesSet.Add(currentName);
        }
        if (_namesSet.Count() != _gamePlayerAmount)
        {
            _namedFailSubject.OnNext(Unit.Default);
            return;
        }
        NamedSetupComplete();
    }

    /// <summary>
    /// 参加人数分だけフィールドを使用可にする。
    /// </summary>
    /// <param name="selectAmount">参加人数</param>
    public void NameFieldNonAvailable(int selectAmount)
    {
        _gamePlayerAmount = selectAmount;
        int maxPeople = 8;

        for (int i = _gamePlayerAmount; i < maxPeople; i++)
            _nameField[i].gameObject.SetActive(false);
    }

    /// <summary>
    /// 全ての入力が終わっているか確認
    /// </summary>
    /// <param name="name">入力された名前</param>
    private void AllOnEditChecker(string name)
    {
        if (name.Length >= 1)
            _onEndEditCount++;

        if (_onEndEditCount >= _gamePlayerAmount)
            _allEndEditSubject.OnNext(Unit.Default);
    }


    /// <summary>
    /// 名付け完了後に名前をマネージャーに渡してゲーム開始
    /// </summary>
    private void NamedSetupComplete()
    {
        NameLifeManager.Instance.Setup(_namesSet);
        GameManager.Instance.SceneLoader("GameSelect",BGMType.InGame);
    }
}
