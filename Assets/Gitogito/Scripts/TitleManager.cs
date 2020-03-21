using UnityEngine;

// タイトル画面管理
public class TitleManager : Utility
{

    private void Start ()
    {
        // フェードイン
        BlackOut.FadeIn (0.5f, 0.5f, 0.1f, () => { }); 
    }

    // スタートボタンを押した
    public void StartPushed ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        // フェードアウトしてゲームシーンに遷移
        BlackOut.FadeOut (0.1f, 1f, 0.2f, () => { SceneChanger.To (Scene.GAMEMAIN); });
    }

    // ランキングボタンを押した
    public void RankingPushed ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        // ランキングのUIを表示
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (true);
        GetComponent<Ranking> ().Init ();
    }

    // ランキングを閉じる
    public void RankingClose ()
    {
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (false);
    }
}
