using UnityEngine;

// タイトル画面でドアを開くクラス
public class TouchOpen : MonoBehaviour
{
    // 開いているか
    private bool opened = false;
    // プレイヤーがぶつかったらドアを開く
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag ("Player"))
        {
            if (!opened)
            {
                GameObject.FindWithTag ("AudioManager").GetComponent<SoundEffecter> ().PlayAnytime (Attribute.DOOR);
                GetComponent<Animator> ().Play ("Open");
                opened = true;
            }
        }
    }
}
