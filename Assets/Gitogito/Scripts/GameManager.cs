using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum GameState
{
    READY, PLAYING, RESULT
}

public class GameManager : Utility
{
    private static GameState gameState = GameState.READY;

    [SerializeField] private Text timerText;
    private float time = 0f;

    public static GameState GetPlayerMoveAble ()
    {
        return gameState;
    }

    private void Start ()
    {
        BlackOut.FadeIn (1f, 1f, 0.5f, CountDown);
    }

    private void CountDown ()
    {
        StartCoroutine (CountDownCor ());
    }

    private IEnumerator CountDownCor ()
    {
        soundEffecter.BGMVolume (0.1f);
        Text start_countText = GameObject.Find ("UI/Start/Count").GetComponent<Text> ();
        RectTransform start_countRectT = start_countText.rectTransform;
        for (int i = 3 ; i >= 0 ; i--)
        {
            string s;
            if (i == 0)
            {
                s = "どこぉ〜？";
                GameObject.Find ("UI/Start/Sagase").SetActive (false);
                soundEffecter.PlayAnytime (Attribute.KA_);

            }
            else
            {
                s = StringWidthConverter.ConvertToFullWidth (i.ToString ());
                soundEffecter.PlayAnytime (Attribute.DON);
            }
            start_countText.text = s;
            start_countText.DOFade (1f, 0f);
            start_countText.DOFade (0f, 1f);
            start_countRectT.DOScale (Vector3.one, 0f);
            start_countRectT.DOScale (Vector3.one * 2f, 1f);
            yield return new WaitForSeconds (1f);
        }
        Destroy (GameObject.Find ("UI/Start"));
        soundEffecter.BGMVolume (0.2f);
        gameState = GameState.PLAYING;
    }

    private void Update ()
    {
        if (gameState != GameState.PLAYING) { return; }

        time += Time.deltaTime;
        timerText.text = string.Format("{0:###.###}", time);
    }

    public void FindSmartPhone ()
    {
        gameState = GameState.RESULT;
        soundEffecter.PlayAnytime (Attribute.YEAH);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        clearUI.SetActive (true);
        timeScore.text = StringWidthConverter.ConvertToFullWidth (string.Format ("{0:###.###}", time));

        if (time < DataManager.GetBestScore () || DataManager.GetBestScore() < 0.01f)
        {
            DataManager.SaveBestScore (time);
        }
    }

    public void Restart ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        BlackOut.FadeOut (0.5f, 2f, 0.5f,
            () => {
                SceneChanger.To (Scene.GAMEMAIN);
            });
    }

    public void GoTitle ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        BlackOut.FadeOut (0.5f, 1.5f, 0.5f,
            () =>
            {
                SceneChanger.To (Scene.TITLE);
            });
    }

    public void RankingPush ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (true);
        GetComponent<Ranking> ().Init ();
    }

    public void RankingClose ()
    {
        GameObject.Find ("UI").transform.Find ("Ranking").gameObject.SetActive (false);
    }
}
