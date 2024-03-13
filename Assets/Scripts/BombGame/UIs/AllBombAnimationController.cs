using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

/// <summary>
/// カードのTweenアニメーションのコントローラー
/// </summary>
public static class AllBombAnimationController
{
    //プロパティを追加
    public static List<Image> _allImagesList = new List<Image>();
    public static List<Tween> _allTweenList = new List<Tween>();
    private static List<Button> _allBombButton = new List<Button>();

    //プロパティを追加
    public static Color _resetColor = default;

    private static bool _isChecking = false;

    public static void InitSet(List<Button> buttons,List<Image> images)
    {
        AddListButton(buttons);
        InteractableValidTask(false, 2).Forget();
        SetCards(images);
    }

    public static void ResetTransformColor()
    {
        foreach (var image in _allImagesList)
        {
            image.transform.DOScale(1, 0.25f)
                               .SetEase(Ease.InBack);
            image.transform.DORotate(Vector3.zero, 0.25f)
                               .SetEase(Ease.InBack);
            image.DOFade(1, 0.25f);
            image.DOColor(_resetColor, 0.25f);
        }
    }

    public static IEnumerator PauseTweens()
    {
        ResetTransformColor();
        foreach (var tween in _allTweenList)
        {
            tween.Pause();
        }
        yield return new WaitForSeconds(3f);

        RestartTweens();
    }

    public static void RestartTweens()
    {
        //プロパティに修正
        if (!BombManager.OnExplosion)
        {
            foreach (var tween in _allTweenList)
            {
                tween.Play();
            }
        }
    }

    public static void SetCards(List<Image> images)
    {
        images.ForEach(card =>
        {
            _allImagesList.Add(card);
        });
    }

    public static void TweenRemoveFromList(Tween tween)
    {
        _allTweenList.Remove(tween);
    }

    public static void AllKillTweens()
    {
        foreach (var image in _allImagesList)
        {
            image.transform.DOKill();
            image.DOKill();
        }
        _allImagesList.Clear();
    }

    public static void KillTweens(Tween tween)
    {
        tween?.Kill();
        tween?.Kill();
        tween = null;
        tween = null;
    }

    public static void AddListButton(List<Button> buttons)
    {
        _allBombButton.AddRange(buttons);
    }

    public static async UniTask InteractableValidTask(bool isCheck, int delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        _isChecking = isCheck;
        foreach (var button in _allBombButton)
        {
            button.interactable = !isCheck;
        }
    }
}
