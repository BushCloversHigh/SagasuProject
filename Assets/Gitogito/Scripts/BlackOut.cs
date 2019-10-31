using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;

public class BlackOut : MonoBehaviour
{
    public static void FadeIn (float delay, float dulation, float end, Action method)
    {
        GameObject fade = Instantiate (Resources.Load<GameObject> ("BlackOut"), GameObject.FindWithTag ("UI").transform);
        fade.GetComponent<BlackOut> ().StartFadeIn (delay, dulation, end, method);
    }

    private void StartFadeIn (float delay, float dulation, float end, Action method)
    {
        StartCoroutine (FadeInCor (delay, dulation, end, method));
    }

    private IEnumerator FadeInCor (float delay, float dulation, float end, Action method)
    {
        Image img = GetComponentInChildren<Image> ();
        img.DOFade (1f, 0f);
        yield return new WaitForSeconds (delay);
        img.DOFade (0f, dulation);
        yield return new WaitForSeconds (dulation + end);
        method ();
        yield return new WaitForSeconds (0.1f);
        Destroy (gameObject);
    }

    public static void FadeOut (float delay, float dulation, float end, Action method)
    {
        GameObject fade = Instantiate (Resources.Load<GameObject> ("BlackOut"), GameObject.FindWithTag ("UI").transform);
        fade.GetComponent<BlackOut> ().StartFadeOut (delay, dulation, end, method);
    }

    private void StartFadeOut (float delay, float dulation, float end, Action method)
    {
        StartCoroutine (FadeOutCor (delay, dulation, end, method));
    }

    private IEnumerator FadeOutCor (float delay, float dulation, float end, Action method)
    {
        Image img = GetComponentInChildren<Image> ();
        img.DOFade (0f, 0f);
        yield return new WaitForSeconds (delay);
        img.DOFade (1f, dulation);
        yield return new WaitForSeconds (dulation + end);
        method ();
        yield return new WaitForSeconds (0.1f);
        Destroy (gameObject);
    }

}

