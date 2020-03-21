using UnityEngine;

// インタラクティブ (扉など)のオブジェクトのクラス
public class InteractSound : MonoBehaviour
{
    // 材質
    [SerializeField] private Attribute attribute;

    // 効果音クラスを取得するためのプロパティ
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
        soundEffecter.PlayAnytime (attribute);
    }
}
