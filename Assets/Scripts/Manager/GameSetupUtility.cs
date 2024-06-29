/// <summary>
/// ゲームを再度セットアップするためのクラス
/// </summary>
public static class GameSetupUtility
{

    /// <summary>
    /// リスタートボタンが押された時の処理
    /// </summary>
    public static void ResetartSetup()
    {
        GameManager.Instance.GameReset();
        NameLifeManager.Instance.Resetlife();
        GameManager.Instance.SceneLoader("GameSelect", BGMType.InGame);
    }

    /// <summary>
    /// タイトルボタンが押された時の処理
    /// </summary>
    public static void AllManagerSetup()
    {
        GameManager.Instance.GameReset();
        GameManager.Instance.SceneLoader("Title", BGMType.Title);
    }
}
