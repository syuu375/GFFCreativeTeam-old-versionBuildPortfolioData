using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AudioSourceSO;

public class AudioManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;
    private bool _titleMusicPlaying = false;
    private bool _gameMusicPlaying = false;

    public static AudioManager Instance;

    [SerializeField] private AudioSourceSO _audiosourceSO;

    private AudioSource audioSource;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;   //ˆê‰ž
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        var data= _audiosourceSO.GetAudio(soundName);
        if (data != null)
        {
            audioSource.volume = data.Volum;
            audioSource.loop = data.Loop;
            audioSource.PlayOneShot(data.Clip);
        }
    }

    public void StartTitleMusic()
    {
        if (!_titleMusicPlaying)
        {
            _titleMusic.Play();
            _gameMusic.Stop();
            _titleMusic.volume = 0.5f;
            _gameMusicPlaying = false;
            _titleMusicPlaying = true;
        }
    }

    public void StartGameMusic()
    {
        if (!_gameMusicPlaying)
        {
            _gameMusic.Play();
            _titleMusic.Stop();
            _gameMusic.volume = 0.5f;
            _titleMusicPlaying = false;
            _gameMusicPlaying = true;
        }
    }

}
