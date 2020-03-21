using UnityEngine;
using UnityEngine.SceneManagement;

// シーンの種類
public enum Scene
{
    TITLE, GAMEMAIN
}

// シーン遷移クラス
public class SceneChanger : MonoBehaviour
{
    // シーンロード
    public static void To (Scene scene)
    {
        SceneManager.LoadScene ((int)scene);
    }
}
