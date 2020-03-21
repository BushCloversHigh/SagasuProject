using UnityEngine;

// 動くオブジェクトの音
public class DynamicObjectSound : MonoBehaviour
{
    // このオブジェクトの材質
    [SerializeField] private Attribute attribute;

    // 効果音のクラスを取得するプロパティ
    private SoundEffecter _soundEffecter;
    private SoundEffecter soundEffecter
    {
        get
        {
            return _soundEffecter ? _soundEffecter : (_soundEffecter = GameObject.FindWithTag ("AudioManager").GetComponent<SoundEffecter> ());
        }
    }

    // 効果音を鳴らす
    public void Play ()
    {
        soundEffecter.Play (attribute);
    }

    // 何かにぶつかったら、効果音を鳴らす
    public void OnCollisionEnter (Collision collision)
    {
        Play ();
    }
}
