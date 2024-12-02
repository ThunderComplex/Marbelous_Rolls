using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    [SerializeField] private AudioMixer audiomixer;
    [SerializeField] private string masterVolume;
    [SerializeField] private string musicVolume;
    [SerializeField] private string soundVolume;

    //[SerializeField] private AudioClip[] musicClips;
    [SerializeField] private IngameSounds[] soundClips;

    public enum Sounds
    {
        empty,
        menuButton,
        levelFail,
        boost,
        go,
        jump,
        colorSwitch,

    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("audiohasbeenchange") == 0)
        {
            PlayerPrefs.SetFloat(masterVolume, -10);
            audiomixer.SetFloat(masterVolume, PlayerPrefs.GetFloat(masterVolume));
            PlayerPrefs.SetFloat(musicVolume, -8);
            audiomixer.SetFloat(musicVolume, PlayerPrefs.GetFloat(musicVolume));
            PlayerPrefs.SetFloat(soundVolume, 10);
            audiomixer.SetFloat(soundVolume, PlayerPrefs.GetFloat(soundVolume));
        }
        else
        {
            setvolume(masterVolume, 0);
            setvolume(musicVolume, 0);
            setvolume(soundVolume, 20);
        }
    }
    private void setvolume(string volumename, float maxdb)
    {
        audiomixer.SetFloat(volumename, PlayerPrefs.GetFloat(volumename));
        bool gotvalue = audiomixer.GetFloat(volumename, out float soundvalue);
        if (gotvalue == true)
        {
            if (soundvalue > maxdb)
            {
                audiomixer.SetFloat(volumename, maxdb);
            }
        }
    }

    public void PlaySoundOneshot(int soundNumber)
    {
        soundSource.volume = soundClips[soundNumber].volume;
        soundSource.PlayOneShot(soundClips[soundNumber].clip);
    }



    [Serializable]
    public struct IngameSounds
    {
        public AudioClip clip;
        public float volume;
    }
}
