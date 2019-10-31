using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    TITLE, GAMEMAIN
}

public class SceneChanger : MonoBehaviour
{
    public static void To (Scene scene)
    {
        SceneManager.LoadScene ((int)scene);
    }
}
