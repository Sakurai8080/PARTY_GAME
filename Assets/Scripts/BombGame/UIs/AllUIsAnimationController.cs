using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カードのTweenアニメーションのコントローラー
/// </summary>
public class AllUIsAnimationController : SingletonMonoBehaviour<AllUIsAnimationController>
{
    private List<Tween> _allTweenList = new List<Tween>();
    private List<Button> _allBombButton = new List<Button>();
    private Color _resetColor = default;

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
        InteractableValidTask(true, 2.5f).Forget();
    }

    /// <summary>
    /// ボタンクリックに登録するセットアップ処理
    /// </summary>
    /// <param name="color">初期カラー</param>
    public void InitButtonSetUp(Color color)
    {
        InteractableValidTask(false, 0).Forget();
        _resetColor = color;
    }

    /// <summary>
    /// 全てのトランスフォームを初期設定
    /// </summary>
    public void ResetTransformColor()
    {
        foreach (var button in _allBombButton)
        {
            Image image = button.image;
            image?.transform.DOScale(1, 0.25f).SetEase(Ease.InBack);
            image?.transform.DORotate(Vector3.zero, 0.25f).SetEase(Ease.InBack);
            image?.DOFade(1, 0.25f);
            image?.DOColor(_resetColor, 0.25f);
        }
    }

    /// <summary>
    /// カード選択時に全てのアニメーションを一時停止
    /// </summary>
    /// <returns>待機時間</returns>
    public IEnumerator PauseTweens()
    {
        ResetTransformColor();
        foreach (var tween in _allTweenList)
            tween.Pause();
        yield return new WaitForSeconds(3f);
        RestartTweens();
    }

    /// <summary>
    /// 全てのTweenのリスタート
    /// </summary>
    public void RestartTweens()
    {
        if (!BombGameManager.Instance.OnExplosion)
        {
            foreach (var tween in _allTweenList)
                tween.Play();
        }
    }

    /// <summary>
    /// Tweenコントロール用のリストに追加
    /// </summary>
    /// <param name="recieveTween">追加するTween</param>
    public void TweenAddList(Tween recieveTween)
    {
        _allTweenList.Add(recieveTween);
    }

    /// <summary>
    /// Tweenリストから取り除く
    /// </summary>
    /// <param name="tween">取り除くTween</param>
    public void TweenRemoveFromList(Tween tween)
    {
        _allTweenList.Remove(tween);
    }

    /// <summary>
    /// 全Tweenの削除
    /// </summary>
    public void AllKillTweens()
    {
        foreach (var button in _allBombButton)
        {
            Image image = button.image;
            image?.transform.DOKill();
            image?.DOKill();
        }
    }

    /// <summary>
    /// 特定のTween削除
    /// </summary>
    /// <param name="tween">削除するTween</param>
    public void KillTweens(Tween tween)
    {
        tween?.Kill();
        tween?.Kill();
        tween = null;
        tween = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttons"></param>
    public void AddListButton(List<Button> buttons)
    {
        _allBombButton.AddRange(buttons);
    }

    /// <summary>
    /// 指定時間で全カードをクリックできない状態にする
    /// </summary>
    /// <param name="toggle">インタラクティブの切り替え</param>
    /// <param name="delayTime">何秒後か</param>
    /// <returns></returns>
    public async UniTask InteractableValidTask(bool toggle, float delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        foreach (var button in _allBombButton)
            button.interactable = toggle;
    }
}
