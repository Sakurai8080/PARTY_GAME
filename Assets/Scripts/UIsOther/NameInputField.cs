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
    [SerializeField]
    TMP_InputField[] _nameField = default;

    [SerializeField]
    Button _nameAddButton = default;

    List<string> _nameList = new List<string>();
    private int _gamePlayerAmount = 0;

    private void Start()
    {
        _nameAddButton.OnClickAsObservable()
                      .TakeUntilDestroy(this)
                      .Subscribe(_ =>
                      {
                          NameChatch();
                          NameLifeManager.Instance.Setup(_nameList);
                      });
    }

    private void NameChatch()
    {
        for (int i = 0; i < _gamePlayerAmount; i++)
        {
            string currentName = _nameField[i].text;
            _nameList.Add(currentName);
        }
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
