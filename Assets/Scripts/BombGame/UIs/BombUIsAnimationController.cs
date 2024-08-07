using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// カードのTweenアニメーションのコントローラー
/// </summary>
public class BombUIsAnimationController : SingletonMonoBehaviour<BombUIsAnimationController>
{
    public IObservable<Unit> OrderChangeObserver => _orderChangeSubject;

    [Header("変数")]
    [SerializeField]
    [Tooltip("回復用イメージ")]
    private Sprite _goldAppleImage;

    [SerializeField]
    [Tooltip("ボムイメージ")]
    private Sprite _bombImage;

    [SerializeField]
    [Tooltip("変更用画像")]
    private Image _afterImage;

    private List<Tween> _allTweenList = new List<Tween>();
    private List<Button> _allBombButton = new List<Button>();
    private Color _resetColor = default;
    private Subject<Unit> _orderChangeSubject = new Subject<Unit>();

    protected override void Awake() { }

    private void Start()
    {
        if (_allTweenList.Count >= 1 || _allBombButton.Count >= 1)
        {
            ResetUp();
        }
    }

    /// <summary>
    /// 重複の追加を防ぐためのリストリセット
    /// </summary>
    private void ResetUp()
    {
        _allTweenList?.Clear();
        _allBombButton?.Clear();
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
    /// カードを選択したときの処理
    /// </summary>
    /// <param name="initColor">カラーの初期値</param>
    public void CardSelected(BombAnim initColor)
    {
        InitButtonSetUp(initColor.InitialColor);
        PauseTweens();
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
    /// 全てのTweenのリスタート
    /// </summary>
    public void RestartTweens()
    {
         foreach (var tween in _allTweenList)
             tween.Play();
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
    /// コントロールするためUIリスト追加
    /// </summary>
    /// <param name="buttons"></param>
    public void AddListButton(List<Button> buttons)
    {
        _allBombButton.AddRange(buttons);
        Debug.Log($"Addされた{_allTweenList.Count} : {_allBombButton.Count} ");
    }

    /// <summary>
    /// 指定時間で全カードをクリックできない状態にする
    /// </summary>
    /// <param name="toggle">インタラクティブの切り替え</param>
    /// <param name="delayTime">何秒後か</param>
    public async UniTask InteractableValidTask(bool toggle, float delayTime)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
        foreach (var button in _allBombButton)
            button.interactable = toggle;
        if (toggle)
        {
            _orderChangeSubject.OnNext(Unit.Default);
        }
    }

    /// <summary>
    /// ボタンクリックに登録するセットアップ処理
    /// </summary>
    /// <param name="color">初期カラー</param>
    private void InitButtonSetUp(Color color)
    {
        InteractableValidTask(false, 0).Forget();
        _resetColor = color;
    }

    /// <summary>
    /// カード選択時に全てのアニメーションを一時停止
    /// </summary>
    /// <returns>待機時間</returns>
    private void PauseTweens()
    {
        ResetTransformColor();
        foreach (var tween in _allTweenList)
            tween.Pause();
    }

    /// <summary>
    /// ボムかりんごを選択したときの画像セット
    /// </summary>
    /// <param name="content">カードの中身タイプ</param>
    /// <param name="transform">選択したカードの位置</param>
    public void AfterImageSet(BoxContents content, RectTransform transform)
    {
        Sprite changeSprite = content == BoxContents.Bomb ? _bombImage : _goldAppleImage;
        _afterImage.sprite = changeSprite;
        AfterImageValid(1,0,true);
        _afterImage.rectTransform.anchoredPosition = transform.anchoredPosition;
    }

    /// <summary>
    /// イメージのフェード
    /// </summary>
    /// <param name="alphaAmount">設定するのアルファ値</param>
    /// <param name="delayTime">かける時間</param>
    /// <param name="isActive">フェードインかアウトか</param>
    public void AfterImageValid(float alphaAmount,float delayTime,bool isActive)
    {
        _afterImage.DOFade(alphaAmount, delayTime);
        _afterImage.enabled = isActive;
    }
}
