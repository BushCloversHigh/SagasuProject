using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private float fadeSpeed = 0.3f;
    private bool isShowing = false;
    private IEnumerator cor;
    public void ShowToast (string message)
    {
        if (isShowing)
        {
            StopCoroutine (cor);
        }
        isShowing = true;
        GameObject toast = GameObject.Find ("UI/System").transform.GetChild (0).gameObject;
        toast.SetActive (true);
        Image back = toast.GetComponent<Image> ();
        Text mess = back.transform.GetChild(0).GetComponent<Text> ();
        mess.text = message;
        back.DOFade (0f, 0f);
        mess.DOFade (0f, 0f);
        back.DOFade (0.7f, fadeSpeed);
        mess.DOFade (1f, fadeSpeed);
        cor = Close ();
        StartCoroutine (cor);
    }

    private IEnumerator Close ()
    {
        yield return new WaitForSeconds (3f);
        GameObject toast = GameObject.Find ("UI/System").transform.GetChild (0).gameObject;
        Image back = toast.GetComponent<Image> ();
        back.DOFade (0f, fadeSpeed);
        Text mess = back.transform.GetChild (0).GetComponent<Text> ();
        mess.DOFade (0f, fadeSpeed);
        yield return new WaitForSeconds (fadeSpeed);
        toast.gameObject.SetActive (false);
        isShowing = false;
    }

    public void Loading ()
    {
        GameObject.Find ("UI/System").transform.Find("Load").gameObject.SetActive (true);
    }

    public void LoadEnd ()
    {
        Invoke ("LoadEnd2", 1f);
    }

    private void LoadEnd2 ()
    {
        GameObject.Find ("UI/System").transform.Find ("Load").gameObject.SetActive (false);
    }
}
