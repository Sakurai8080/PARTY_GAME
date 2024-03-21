using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;
using TMPro;

public class NameInputField : MonoBehaviour
{
    public IObservable<Unit> AllEndEditObserver => _allEndEditSubject;

    [SerializeField]
    TMP_InputField[] _nameField = default;

    [SerializeField]
    Button _nameAddButton = default;

    List<string> _nameList = new List<string>();
    private int _gamePlayerAmount = 0;

    int _onEndEditCount = 0;

    private Subject<Unit> _allEndEditSubject = new Subject<Unit>();

    private void Start()
    {
        _nameAddButton.OnClickAsObservable()
                      .TakeUntilDestroy(this)
                      .Subscribe(_ =>
                      {
                          int nameAmount = NameCatch();
                          NameLifeManager.Instance.Setup(_nameList);
                          GameManager.Instance.SceneLoader("GameSelect");
                      });

        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            _nameField[i].onEndEdit.AddListener(_ =>AllOnEditChecker());
        }
    }

    private void AllOnEditChecker()
    {
        _onEndEditCount++;
        if (_onEndEditCount == _gamePlayerAmount)
            _allEndEditSubject.OnNext(Unit.Default);
    }

    private int NameCatch()
    {
        _nameList.Clear();
        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            string currentName =  (_nameField[i].text != "")? _nameField[i].text : $"P{i+1}" ;
            _nameList.Add(currentName);
        }
        return _nameList.Count();
    }

    public void NameFieldNonAvailable(int selectAmount)
    {
        _gamePlayerAmount = selectAmount;
        int maxPeople = 8;

        for (int i = _gamePlayerAmount; i < maxPeople; i++)
        {
            _nameField[i].interactable = false;
        }
    }
}
