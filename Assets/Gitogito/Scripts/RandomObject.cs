using UnityEngine;
using System.Collections;

public class RandomObject : MonoBehaviour
{
    [SerializeField] private GameObject smartPhone;
    [SerializeField] private GameObject[] randomObjects;
    [SerializeField] private Transform[] areas;
    [SerializeField] private int howMany = 100;

    private GameObject go_smartPhone;

    private void Start ()
    {
        StartCoroutine (Generate ());
    }

    private IEnumerator Generate ()
    {
        go_smartPhone = Instantiate (smartPhone, RandomPositionS (), Quaternion.identity);

        for (int i = 0 ; i < howMany ; i++)
        {
            int random_randomObjects = Random.Range (0, randomObjects.Length);
            Instantiate (randomObjects[random_randomObjects], RandomPosition(), Quaternion.identity);

            if(i % 50 == 0)
            {
                yield return null;
            }
        }
    }

    private Vector3 RandomPosition ()
    {
        int random_areas = Random.Range (0, areas.Length);
        Vector3 random_pos = areas[random_areas].position;
        random_pos.x += Random.Range (-areas[random_areas].localScale.x / 2f, areas[random_areas].localScale.x / 2f);
        random_pos.z += Random.Range (-areas[random_areas].localScale.z / 2f, areas[random_areas].localScale.z / 2f);
        return random_pos;
    }

    private Vector3 RandomPositionS ()
    {
        int random_areas = Random.Range (1, areas.Length);
        Vector3 random_pos = areas[random_areas].position;
        random_pos.x += Random.Range (-areas[random_areas].localScale.x / 2f, areas[random_areas].localScale.x / 2f);
        random_pos.z += Random.Range (-areas[random_areas].localScale.z / 2f, areas[random_areas].localScale.z / 2f);
        return random_pos;
    }

    private void Update ()
    {
        if(go_smartPhone.transform.position.x > 6f || go_smartPhone.transform.position.x < -6f
            || go_smartPhone.transform.position.z < -10.5f || go_smartPhone.transform.position.z > 6f
            || go_smartPhone.transform.position.y < -1f || go_smartPhone.transform.position.y > 4.5f)
        {
            go_smartPhone.transform.position = RandomPosition ();
        }
    }
}
