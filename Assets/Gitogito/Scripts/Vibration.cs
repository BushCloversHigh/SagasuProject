using UnityEngine;
using System.Collections;
using DG.Tweening;

// バイブレーションを鳴らすクラス
public class Vibration : MonoBehaviour
{
    // 何秒毎に鳴らすか
    [SerializeField] private float interval = 15f;
    // プレイヤーのトランスフォーム
    private Transform player;
    // バイブレーションがなっている方向を示すオブジェクト
    private RectTransform directionTransform;
    // 鳴ってます
    private bool isVibrate = false;

    private void Start ()
    {
        // 取得
        player = GameObject.FindWithTag ("Player").transform;
        directionTransform = GameObject.Find ("UI/VibrateDirection/Direction").GetComponent<RectTransform> ();
        // 定期的にバイブレーションする開始
        StartCoroutine (Vibrate ());
    }

    // バイブレーションのコルーチン
    private IEnumerator Vibrate ()
    {
        // Ready用に最初だけ5秒待つ
        yield return new WaitForSeconds (5f);
        // 無限ループ
        while (true)
        {
            // 小さくする
            directionTransform.DOSizeDelta (Vector2.zero, 0.5f);
            // バイブが鳴っていない
            isVibrate = false;
            // 間隔を開けて
            yield return new WaitForSeconds (interval);
            // バイブ用の音を鳴らし
            GetComponent<AudioSource> ().Play ();
            // 方向を示すオブジェクトを大きくし
            directionTransform.DOSizeDelta (new Vector2 (800, 800), 0.5f);
            // バイブがなってるよ
            isVibrate = true;
            yield return new WaitForSeconds (2.2f);
        }
    }

    private void Update ()
    {
        if (!isVibrate) { return; }
        // バイブがなっていない時に処理できる
        // 方向を計算
        // 位置関係を計算
        Vector3 diff = transform.position - player.position;
        // y座標は合わせる
        diff.y = 0;
        // 角度を外積で計算
        Vector3 axis = Vector3.Cross (player.forward, diff);
        float angle = Mathf.Deg2Rad * Vector3.Angle (player.forward, diff)
                         * (axis.y < 0 ? -1 : 1);
        float y = Mathf.Cos (angle);
        float x = Mathf.Sin (angle);
        directionTransform.localPosition = new Vector2 (
                Mathf.Clamp (x * 900, -400, 400),
                Mathf.Clamp (y * 600, -250, 250)
            );

    }
}

