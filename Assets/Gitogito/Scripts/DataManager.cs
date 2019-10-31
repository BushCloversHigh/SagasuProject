using UnityEngine;

public class DataManager : MonoBehaviour
{
    private const string KEY_SCORE = "cwnceinwoinincdimw";

    public static void SaveBestScore (float score)
    {
        PlayerPrefs.SetFloat (KEY_SCORE, score);
        PlayerPrefs.Save ();
    }

    public static float GetBestScore ()
    {
        return PlayerPrefs.GetFloat (KEY_SCORE, 0);
    }
}
