using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Vibration : MonoBehaviour
{
    [SerializeField] private float interval = 15f;
    private Transform player;
    private RectTransform directionTransform;
    private bool isVibrate = false;

    private void Start ()
    {
        player = GameObject.FindWithTag ("Player").transform;
        directionTransform = GameObject.Find ("UI/VibrateDirection/Direction").GetComponent<RectTransform> ();
        StartCoroutine (Vibrate ());
    }

    private IEnumerator Vibrate ()
    {
        yield return new WaitForSeconds (5f);
        while (true)
        {
            directionTransform.DOSizeDelta (Vector2.zero, 0.5f);
            isVibrate = false;
            yield return new WaitForSeconds (interval);
            GetComponent<AudioSource> ().Play ();
            directionTransform.DOSizeDelta (new Vector2 (800, 800), 0.5f);
            isVibrate = true;
            yield return new WaitForSeconds (2.2f);
        }
    }

    private void Update ()
    {
        if (!isVibrate) { return; }

        Vector3 diff = transform.position - player.position;
        diff.y = 0;
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

