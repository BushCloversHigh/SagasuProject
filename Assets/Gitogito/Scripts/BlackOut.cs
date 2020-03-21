using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;

// 画面のフェードインとフェードアウトを行うクラス
public class BlackOut : MonoBehaviour
{
    // フェードイン
    public static void FadeIn (float delay, float dulation, float end, Action method)
    {
        // 暗幕のオブジェクトを生成し、フェードインアニメーションをさせる
        GameObject fade = Instantiate (Resources.Load<GameObject> ("BlackOut"), GameObject.FindWithTag ("UI").transform);
        fade.GetComponent<BlackOut> ().StartFadeIn (delay, dulation, end, method);
    }

    // フェードインアニメーションを開始する関数
    private void StartFadeIn (float delay, float dulation, float end, Action method)
    {
        StartCoroutine (FadeInCor (delay, dulation, end, method));
    }

    // フェードインのアニメーションフロー
    private IEnumerator FadeInCor (float delay, float dulation, float end, Action method)
    {
        // Imageを子から取得
        Image img = GetComponentInChildren<Image> ();
        // 透明度を1にして真っ黒に
        img.DOFade (1f, 0f);
        // 遅延させる
        yield return new WaitForSeconds (delay);
        // 透明度を0にしていく
        img.DOFade (0f, dulation);
        yield return new WaitForSeconds (dulation + end);
        // フェードインが終わったら関数を呼び出す
        method ();
        yield return new WaitForSeconds (0.1f);
        // このオブジェクトを削除
        Destroy (gameObject);
    }

    // フェードアウト
    public static void FadeOut (float delay, float dulation, float end, Action method)
    {
        GameObject fade = Instantiate (Resources.Load<GameObject> ("BlackOut"), GameObject.FindWithTag ("UI").transform);
        fade.GetComponent<BlackOut> ().StartFadeOut (delay, dulation, end, method);
    }

    // フェードアウトのアニメーションを開始する関数
    private void StartFadeOut (float delay, float dulation, float end, Action method)
    {
        StartCoroutine (FadeOutCor (delay, dulation, end, method));
    }

    // フェードアウトのアニメーションのフロー
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

