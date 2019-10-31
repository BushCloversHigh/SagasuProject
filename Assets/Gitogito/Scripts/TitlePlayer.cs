using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TitlePlayer : MonoBehaviour
{

    private void Start ()
    {
        StartCoroutine (PlayerMove ());
    }

    private IEnumerator PlayerMove ()
    {
        while (true)
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent> ();
            while (!navMeshAgent.SetDestination (new Vector3 (Random.Range (-4.5f, 2.0f), 0, Random.Range (-8.0f, 3.0f)))) {}
            yield return new WaitForSeconds (4f);
        }
    }
}
