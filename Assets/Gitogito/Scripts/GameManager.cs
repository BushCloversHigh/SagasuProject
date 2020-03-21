using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// ゲームの状態の列挙型
public enum GameState
{
    READY, PLAYING, RESULT
}

// ゲームを管理するクラス
public class GameManager : Utility
{
    // 最初は準備
    private static GameState gameState = GameState.READY;

    // 時間経過用のクラス
    [SerializeField] private Text timerText;
    private float time = 0f;

    // 状態を返す
    public static GameState GetState ()
    {
        return gameState;
    }

    private void Start ()
    {
        // フェードイン
        BlackOut.FadeIn (1f, 1f, 0.5f, CountDown);
    }

    // カウントダウンを開始
    private void CountDown ()
    {
        StartCoroutine (CountDownCor ());
    }

    // カウントダウンのコルーチン
    private IEnumerator CountDownCor ()
    {
        // BGMのボリュームを小さく
        soundEffecter.BGMVolume (0.1f);
        // カウントダウンのテキストをさがす
        Text start_countText = GameObject.Find ("UI/Start/Count").GetComponent<Text> ();
        RectTransform start_countRectT = start_countText.rectTransform;
        // 3秒間のカウントダウン
        for (int i = 3 ; i >= 0 ; i--)
        {
            string s;
            // 0
            if (i == 0)
            {
                s = "どこぉ〜？";
                GameObject.Find ("UI/Start/Sagase").SetActive (false);
                soundEffecter.PlayAnytime (Attribute.KA_);

            }
            else // 3 2 1
            {
                // 全角に変換
                s = StringWidthConverter.ConvertToFullWidth (i.ToString ());
                soundEffecter.PlayAnytime (Attribute.DON);
            }
            // 数字のアニメーション
            start_countText.text = s;
            start_countText.DOFade (1f, 0f);
            start_countText.DOFade (0f, 1f);
            start_countRectT.DOScale (Vector3.one, 0f);
            start_countRectT.DOScale (Vector3.one * 2f, 1f);
            yield return new WaitForSeconds (1f);
        }
        // カウントダウンが終わったら消す
        Destroy (GameObject.Find ("UI/Start"));
        // 音量を少し上げる
        soundEffecter.BGMVolume (0.2f);
        // 状態をプレイングに
        gameState = GameState.PLAYING;
    }

    private void Update ()
    {
        // プレイ中じゃない時は何もしない
        if (gameState != GameState.PLAYING) { return; }

        // 経過時間
        time += Time.deltaTime;
        timerText.text = string.Format("{0:###.###}", time);
    }

    // スマホを見つけた
    public void FindSmartPhone ()
    {
        // 状態をリザルトに
        gameState = GameState.RESULT;
        // 効果音を鳴らす
        soundEffecter.PlayAnytime (Attribute.YEAH);

        // マウスカーソルを表示
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // クリア画面を表示
        clearUI.SetActive (true);
        // スコアを全角で表示
        timeScore.text = StringWidthConverter.ConvertToFullWidth (string.Format ("{0:###.###}", time));
        // ハイスコアを更新してセーブ
        if (time < DataManager.GetBestScore () || DataManager.GetBestScore() < 0.01f)
        {
            DataManager.SaveBestScore (time);
        }
    }

    // リスタート
    public void Restart ()
    {
        // 効果音
        soundEffecter.PlayAnytime (Attribute.DON);
        // フェードアウトした後、シーンを再読み込み
        BlackOut.FadeOut (0.5f, 2f, 0.5f,
            () => {
                SceneChanger.To (Scene.GAMEMAIN);
            });
    }

    // タイトルへ
    public void GoTitle ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        BlackOut.FadeOut (0.5f, 1.5f, 0.5f,
            () =>
            {
                SceneChanger.To (Scene.TITLE);
            });
    }

    // ランキングを開く
    public void RankingPush ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (true);
        GetComponent<Ranking> ().Init ();
    }

    // ランキングを閉じる
    public void RankingClose ()
    {
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (false);
    }
}
