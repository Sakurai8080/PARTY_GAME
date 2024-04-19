using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// 選択するカードのボタン
/// </summary>
public class BombSelectButton : MonoBehaviour
{
    public IObservable<BombAnim> OnClickObserver => _onClickSubject;
    public Button ButtonCompornent => _buttonCompornent;

    [Header("カードに付くボタン")]
    [SerializeField]
    private Button _bombButton = default;

    private BombAnim _bombAnim;
    private Button _buttonCompornent;
    private Subject<BombAnim> _onClickSubject = new Subject<BombAnim>();

    private void Start()
    {
        _bombAnim = GetComponent<BombAnim>();
        _buttonCompornent = GetComponent<Button>();
        _bombButton.OnClickAsObservable()
                   .TakeUntilDestroy(this)
                   .Subscribe(_ => _onClickSubject.OnNext(_bombAnim));
    }
}
