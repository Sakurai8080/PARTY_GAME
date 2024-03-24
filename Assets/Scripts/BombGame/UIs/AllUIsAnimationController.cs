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
public class AllUIsAnimationController : SingletonMonoBehaviour<AllUIsAnimationController>
{
    //プロパティを追加
    public List<Tween> _allTweenList = new List<Tween>();
    private List<Button> _allBombButton = new List<Button>();

    //プロパティを追加
    public Color _resetColor = default;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 初期値設定
    /// </summary>
    /// <param name="buttons">全カードのボタン</param>
    /// <param name="images">全カードの画像</param>
    public void InitSet(List<Button> buttons)
    {
        AddListButton(buttons);
        InteractableValidTask(false, 2).Forget();
    }

    public void ResetTransformColor()
    {
        foreach (var button in _allBombButton)
        {
            Image image = button.image;
            image.transform.DOScale(1, 0.25f)
                               .SetEase(Ease.InBack);
            image.transform.DORotate(Vector3.zero, 0.25f)
                               .SetEase(Ease.InBack);
            image.DOFade(1, 0.25f);
            image.DOColor(_resetColor, 0.25f);
        }
    }

    public IEnumerator PauseTweens()
    {
        ResetTransformColor();
        foreach (var tween in _allTweenList)
        {
            tween.Pause();
        }
        yield return new WaitForSeconds(3f);

        RestartTweens();
    }

    public void RestartTweens()
    {
        if (!BombManager.Instance.OnExplosion)
        {
            foreach (var tween in _allTweenList)
            {
                tween.Play();
            }
        }
    }

    public void TweenRemoveFromList(Tween tween)
    {
        _allTweenList.Remove(tween);
    }

    public void AllKillTweens()
    {
        foreach (var button in _allBombButton)
        {
            Image image = button.image;
            image.transform.DOKill();
            image.DOKill();
        }
    }

    public void KillTweens(Tween tween)
    {
        tween?.Kill();
        tween?.Kill();
        tween = null;
        tween = null;
    }

    public void AddListButton(List<Button> buttons)
    {
        _allBombButton.AddRange(buttons);
    }

    public async UniTask InteractableValidTask(bool isCheck, int delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        foreach (var button in _allBombButton)
        {
            button.interactable = !isCheck;
        }
    }
}
