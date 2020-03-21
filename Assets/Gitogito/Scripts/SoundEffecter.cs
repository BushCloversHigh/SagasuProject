using UnityEngine;

// 材質の種類それに伴った効果音
public enum Attribute
{
    WOOD, GRASS, METAL, PLASTIC, HEAVY, DOOR, DRAW, DON, KA_, YEAH
}

// BGMの種類
public enum BGM
{
    GAME, TITLE, RESULT
}

// 効果音クラス
public class SoundEffecter : MonoBehaviour
{
    // Attribute列挙型に合わせてアタッチする
    [SerializeField] private AudioClip[] soundEffects;

    // オーディオソース
    private AudioSource se_audio, bgm_audio;
    // 音を鳴らせるかのフラグ
    private bool able = false;
    private float t = 0f;

    private void Start ()
    {
        // コンポーネントを取得
        bgm_audio = transform.GetChild (0).GetComponent<AudioSource> ();
        se_audio = transform.GetChild(1).GetComponent<AudioSource> ();
    }

    private void Update ()
    {
        // 一回音が鳴ったら、0.1秒後に鳴らせるようになる
        if (!able)
        {
            t += Time.deltaTime;
            if (t > 0.1f)
            {
                able = true;
                t = 0f;
            }
        }
    }

    // 効果音を再生
    public void Play (Attribute attribute)
    {
        if (able)
        {
            able = false;
            se_audio.PlayOneShot (soundEffects[(int)attribute]);
        }
    }

    // able変数に関係なく、絶対に鳴らせる
    public void PlayAnytime (Attribute attribute)
    {
        se_audio.PlayOneShot (soundEffects[(int)attribute]);
    }

    // BGMを変更する
    public void BGMChange (BGM bgm)
    {
        string pass;
        switch (bgm)
        {
        case BGM.GAME:
            pass = "GameBGM";
            break;
        case BGM.TITLE:
            pass = "TitleBGM";
            break;
        case BGM.RESULT:
            pass = "ResultBGM";
            break;
        default:
            pass = "";
            break;
        }
        AudioClip bgmClip = Resources.Load<AudioClip> (pass);
        bgm_audio.clip = bgmClip;
        bgm_audio.Play ();
    }

    // BGMのボリュームを変更する
    public void BGMVolume (float volume)
    {
        bgm_audio.volume = volume;
    }
}
