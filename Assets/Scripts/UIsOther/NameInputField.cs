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
    public IObservable<Unit> NamedFailObserver => _namedFailSubject;

    [SerializeField]
    TMP_InputField[] _nameField = default;

    private Subject<Unit> _allEndEditSubject = new Subject<Unit>();
    private Subject<Unit> _namedFailSubject = new Subject<Unit>();
    private HashSet<string> _nameSet = new HashSet<string>();
    private int _gamePlayerAmount = 0;
    private int _onEndEditCount = 0;

    private void Start()
    {
        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            _nameField[i].onEndEdit.AddListener(_ => AllOnEditChecker());
        }
    }
    
    private void AllOnEditChecker()
    {
        _onEndEditCount++;
        Debug.Log(_onEndEditCount);
        if (_onEndEditCount >= _gamePlayerAmount)
            _allEndEditSubject.OnNext(Unit.Default);
    }

    public void NameAndCountChecker()
    {
        _nameSet.Clear();
        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            string currentName =  (_nameField[i].text != "")? _nameField[i].text : $"P{i+1}" ;
            _nameSet.Add(currentName);
        }
        if (_nameSet.Count() != _gamePlayerAmount)
        {
            _namedFailSubject.OnNext(Unit.Default);
            return;
        }
        PassNameChecked();
    }

    private void PassNameChecked()
    {
        NameLifeManager.Instance.Setup(_nameSet);
        GameManager.Instance.SceneLoader("GameSelect");
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
