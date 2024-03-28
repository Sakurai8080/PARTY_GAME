using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ボタンゲームの選択ボタン
/// </summary>
public class TBGameSelectBtn : MonoBehaviour
{
    public IObservable<Button> SelectedObsever => _selectedSubject;

    [Header("変数")]
    [Tooltip("選択するボタン")]
    [SerializeField]
    private Button _theButton = default;

    private Subject<Button> _selectedSubject = new Subject<Button>();

    void Start()
    {
        _theButton.OnClickAsObservable()
                  .TakeUntilDestroy(this)
                  .Subscribe(_ =>
                  {
                      _selectedSubject.OnNext(_theButton);
                  });
    }
}
