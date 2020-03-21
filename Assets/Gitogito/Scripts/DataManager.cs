using UnityEngine;

// データを管理するクラス
public class DataManager : MonoBehaviour
{
    // キー
    private const string KEY_SCORE = "cwnceinwoinincdimw";

    // ベストスコアをセーブ
    public static void SaveBestScore (float score)
    {
        PlayerPrefs.SetFloat (KEY_SCORE, score);
        PlayerPrefs.Save ();
    }

    // スコアを取得
    public static float GetBestScore ()
    {
        return PlayerPrefs.GetFloat (KEY_SCORE, 0);
    }
}
