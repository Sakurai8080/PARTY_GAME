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

public class TMProLimit : MonoBehaviour
{
    [SerializeField]
    private int _textMaxAmount = 3;

    private TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        TextLimitter();
    }

    private void TextLimitter()
    {
        string currentName = _text.text.ToString();
        _text.text = (currentName.Length > _textMaxAmount) ? currentName.Substring(0, _textMaxAmount) : currentName;
    }
}
