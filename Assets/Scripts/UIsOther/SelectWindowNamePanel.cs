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

public class SelectWindowNamePanel : MonoBehaviour
{

    [SerializeField]
    private LifeImage[] _lifeUIs = default;

    [SerializeField]
    private List<TextMeshProUGUI> _nameTmpro = new List<TextMeshProUGUI>();

    char _semicolon = ':';

    private void Awake()
    {
        for (int i = 0; i < NameLifeManager.Instance.GamePlayerAmount; i++)
        {
            string currentPlayerName = $"{NameLifeManager.Instance.NameList[i]}";
            _lifeUIs[i].gameObject.SetActive(true);
            _lifeUIs[i].NameRecirver(currentPlayerName);
            _nameTmpro[i].text = (currentPlayerName.Length >= 4) ? currentPlayerName.Substring(0, 3)+_semicolon : currentPlayerName+_semicolon;
            _nameTmpro[i].gameObject.SetActive(true);
        }
    }
}
