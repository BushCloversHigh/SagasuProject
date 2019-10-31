using UnityEngine;

public class TitleManager : Utility
{

    private void Start ()
    {
        BlackOut.FadeIn (0.5f, 0.5f, 0.1f, () => { }); 
    }

    public void StartPushed ()
    {
        soundEffecter.PlayAnytime (Attribute.DON);
        BlackOut.FadeOut (0.1f, 1f, 0.2f, () => { SceneChanger.To (Scene.GAMEMAIN); });
    }

    public void RankingPushed ()
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
