using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// タイトル画面でのプレイヤーの挙動
public class TitlePlayer : MonoBehaviour
{
    // 動く
    private void Start ()
    {
        StartCoroutine (PlayerMove ());
    }

    private IEnumerator PlayerMove ()
    {
        // ランダムな位置を行ったり来たり
        while (true)
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent> ();
            while (!navMeshAgent.SetDestination (new Vector3 (Random.Range (-4.5f, 2.0f), 0, Random.Range (-8.0f, 3.0f)))) {}
            yield return new WaitForSeconds (4f);
        }
    }
}
