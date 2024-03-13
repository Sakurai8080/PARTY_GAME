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

public class Life : MonoBehaviour
{
    [SerializeField]
    private Image[] LifeImages = default;

    string _connectionName = default;
    int _currentLife = 0;

    private void Start()
    {
        LifeReciever();
        for (int i = 0; i < _currentLife; i++)
        {
            LifeImages[i].gameObject.SetActive(true);
        }
    }

    public void NameRecirver(string name)
    {
        _connectionName = name;
    }

    private void LifeReciever()
    {
        _currentLife = NameLifeManager.Instance.NamefromLifePass(_connectionName);
    }
}
