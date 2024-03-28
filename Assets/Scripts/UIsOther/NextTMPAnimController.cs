using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面の推移するボタンのアニメーション
/// </summary>
public class NextTMPAnimController : MonoBehaviour
{
    public bool OnAnimation => _onAnimation;

    [Header("変数")]
    [Tooltip("各アニメーションコンポーネント")]
    [SerializeField]
    private List<NextTextAnimation> _nextTextAnimations = default;

    private bool _onAnimation = false;

    /// <summary>
    /// アニメーションON/OFFの切り替え
    /// </summary>
    /// <param name="isStart"></param>
    public void TMPAnimationSwitcher(bool isStart)
    {
        _onAnimation = isStart;
        if (isStart)
            _nextTextAnimations.ForEach(x => x.MakeTweenSequence());
        else
            _nextTextAnimations.ForEach(x => x.StopTween());
    }
}
