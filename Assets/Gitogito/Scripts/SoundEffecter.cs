using UnityEngine;

public enum Attribute
{
    WOOD, GRASS, METAL, PLASTIC, HEAVY, DOOR, DRAW, DON, KA_, YEAH
}

public enum BGM
{
    GAME, TITLE, RESULT
}

public class SoundEffecter : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundEffects;

    private AudioSource se_audio, bgm_audio;
    private bool able = false;
    private float t = 0f;

    private void Start ()
    {
        bgm_audio = transform.GetChild (0).GetComponent<AudioSource> ();
        se_audio = transform.GetChild(1).GetComponent<AudioSource> ();
    }

    private void Update ()
    {
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

    public void Play (Attribute attribute)
    {
        if (able)
        {
            able = false;
            se_audio.PlayOneShot (soundEffects[(int)attribute]);
        }
    }

    public void PlayAnytime (Attribute attribute)
    {
        se_audio.PlayOneShot (soundEffects[(int)attribute]);
    }

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

    public void BGMVolume (float volume)
    {
        bgm_audio.volume = volume;
    }
}
