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

public class Ball : MonoBehaviour
{
    private TextMeshProUGUI _nameText = default;

    private void Start()
    {
        _nameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        TextDestroy();
        BallController.Instance.BallListRemover(this);
    }

    void TextDestroy()
    {
        _nameText.enabled = false;
    }
}
