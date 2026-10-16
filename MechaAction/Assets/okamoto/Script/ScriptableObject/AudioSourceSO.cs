using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioSource")]
public class AudioSourceSO : ScriptableObject
{
    public List<AudioSourceMusic> audioSourceList = new List<AudioSourceMusic>();
    private Dictionary<string, AudioSourceMusic> _audioDictionary;

    [System.Serializable]
    public class AudioSourceMusic
    {
        [SerializeField] string audiosource;
        [SerializeField] AudioClip clip;
        [SerializeField] float volum;
        [SerializeField] bool loop;

        public string Audiosource { get => audiosource; }
        public AudioClip Clip { get => clip; }
        public float Volum { get => volum; }
        public bool Loop { get => loop; }

    }
    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        _audioDictionary = new Dictionary<string, AudioSourceMusic>();
        foreach (var data in audioSourceList)
        {
            _audioDictionary[data.Audiosource] = data;
        }
    }

    public AudioSourceMusic GetAudio(string name)
    {
        if (_audioDictionary.TryGetValue(name, out var data))
            return data;
        return null;
    }
}
