using UnityEngine;
using System.Collections;

// オブジェクトをランダムに生成するクラス
public class RandomObject : MonoBehaviour
{
    // スマホ
    [SerializeField] private GameObject smartPhone;
    // ランダムに生成させるオブジェクト
    [SerializeField] private GameObject[] randomObjects;
    // 生成させるエリア
    [SerializeField] private Transform[] areas;
    // 何個生成するか
    [SerializeField] private int howMany = 100;
    // 生成されたスマホを格納する変数
    private GameObject go_smartPhone;

    private void Start ()
    {
        // 一気に生成すると重いので、いくつかに分けて生成
        StartCoroutine (Generate ());
    }

    // 生成するコルーチン
    private IEnumerator Generate ()
    {
        // スマホをランダムな位置に生成(スマホ用の範囲で)
        go_smartPhone = Instantiate (smartPhone, RandomPositionS (), Quaternion.identity);
        // 個数分生成
        for (int i = 0 ; i < howMany ; i++)
        {
            // どれを生成するかランダムで決める
            int random_randomObjects = Random.Range (0, randomObjects.Length);
            // ランダムな位置に生成
            Instantiate (randomObjects[random_randomObjects], RandomPosition(), Quaternion.identity);
            // 10子生成したら次のフレームで
            if(i % 10 == 0)
            {
                yield return null;
            }
        }
    }

    // ランダムな位置で生成
    private Vector3 RandomPosition ()
    {
        // あらかじめ設定されたエリアのどれで生成するか
        int random_areas = Random.Range (0, areas.Length);
        // そのエリア内のどこに生成するか
        Vector3 random_pos = areas[random_areas].position;
        random_pos.x += Random.Range (-areas[random_areas].localScale.x / 2f, areas[random_areas].localScale.x / 2f);
        random_pos.z += Random.Range (-areas[random_areas].localScale.z / 2f, areas[random_areas].localScale.z / 2f);
        return random_pos;
    }

    // スマホ用のランダムな位置
    private Vector3 RandomPositionS ()
    {
        // 1には生成しない
        int random_areas = Random.Range (1, areas.Length);
        Vector3 random_pos = areas[random_areas].position;
        random_pos.x += Random.Range (-areas[random_areas].localScale.x / 2f, areas[random_areas].localScale.x / 2f);
        random_pos.z += Random.Range (-areas[random_areas].localScale.z / 2f, areas[random_areas].localScale.z / 2f);
        return random_pos;
    }

    private void Update ()
    {
        // スマホがステージ外に出ることがあったら、今度は通常のランダムな位置に移動
        if(go_smartPhone.transform.position.x > 6f || go_smartPhone.transform.position.x < -6f
            || go_smartPhone.transform.position.z < -10.5f || go_smartPhone.transform.position.z > 6f
            || go_smartPhone.transform.position.y < -1f || go_smartPhone.transform.position.y > 4.5f)
        {
            go_smartPhone.transform.position = RandomPosition ();
        }
    }
}
