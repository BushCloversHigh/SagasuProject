using UnityEngine;
using UnityEngine.UI;

public class Utility : MonoBehaviour
{
    public SoundEffecter soundEffecter
    {
        get { return GameObject.FindWithTag ("AudioManager").GetComponent<SoundEffecter> (); }
    }

    public GameManager gameManager
    {
        get { return GameObject.FindWithTag ("GameManager").GetComponent<GameManager> (); }
    }

    public GameObject clearUI
    {
        get { return GameObject.Find ("UI").transform.Find("Clear").gameObject; }
    }

    public Text timeScore
    {
        get { return clearUI.transform.Find ("Back/Time/Num").GetComponent<Text> (); }
    }
}
