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

public class TBGameBackGround : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _imageList = new List<Sprite>();

    private Image _activeBackGround;
    private Color _initColor;

    private void Start()
    {
        _activeBackGround = GetComponent<Image>();
        _initColor = _activeBackGround.color;
    }

    public void BackGroundChange(int activeButtonAmount)
    {
        _activeBackGround.color = _initColor;
        switch(activeButtonAmount)
        {
            case 2:
                _activeBackGround.sprite = _imageList[0];
                break;
            case 3:
                _activeBackGround.sprite = _imageList[1];
                break;
            case 4:
                _activeBackGround.sprite = _imageList[2];
                break;
            case 5:
                _activeBackGround.sprite = _imageList[3];
                break;
            case 1:
                _activeBackGround.sprite = _imageList[4];
                break;
            default:
                Debug.LogError($"<colot=red>{activeButtonAmount}の枚数が使用範囲外です</color>");
                break;

        }
        BackGroundFade();
    }

    private void BackGroundFade()
    {
        _activeBackGround.DOColor(Color.white, 2f)
                         .SetEase(Ease.OutExpo);
    }

}
