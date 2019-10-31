using UnityEngine;

public class TouchOpen : MonoBehaviour
{
    private bool opened = false;
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
