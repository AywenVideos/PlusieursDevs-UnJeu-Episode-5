using UnityEngine;
using Managers;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private AudioSource audioSourceForPlayList;
    public AudioClip[] _playlist1;
    public AudioClip[] _playlist2;
    public int musicIndex = 0;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSourceForPlayList = gameObject.AddComponent<AudioSource>();
        audioSourceForPlayList.volume = 0.25f;
        audioSource.loop = true;
    }

    private void Update()
    {
        if (audioSourceForPlayList.isPlaying == false)
        {
            PlayList();
        }
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        if (musicClip == null) return;

        if (audioSource.clip == musicClip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = musicClip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayList()
    {
        if(GameManager.Position == 0)
        {
            musicIndex = (musicIndex + 1) % _playlist1.Length;
            audioSourceForPlayList.clip = _playlist1[musicIndex];
            audioSourceForPlayList.Play();
        } 
        else
        {
            musicIndex = (musicIndex + 1) % _playlist2.Length;
            audioSourceForPlayList.clip = _playlist2[musicIndex];
            audioSourceForPlayList.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
