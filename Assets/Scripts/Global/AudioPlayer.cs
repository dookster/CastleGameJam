using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour
{

    public AudioSource musicSource;

    private static AudioPlayer instance = null;
    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType(typeof(AudioPlayer)) as AudioPlayer;
                if (instance == null)
                {           
                    instance = CreateAudioPlayer();
                    Debug.Log("Audiopayer created!");
                }
                instance.transform.parent = null;
                Object.DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private static AudioPlayer CreateAudioPlayer()
    {
        GameObject gO = new GameObject("AudioPlayer");
        AudioPlayer aP = gO.AddComponent<AudioPlayer>();

        return aP;
    }

    public static AudioPlayer FabricateNewPlayer()
	{
		GameObject gO = new GameObject("AudioPlayer");
		AudioPlayer created = gO.AddComponent<AudioPlayer>();
		return created;
	}

    private List<AudioSource> aSources = new List<AudioSource>();

	void Awake()
	{
        AddNewAudioSource();
        AddNewAudioSource();
    }

    public void Play2DAudio(AudioClip clip)
    {
        PlayAudio(clip, 1f, 0f);
    }

    public void Play2DAudio(AudioClip clip, float volume)
    {
        PlayAudio(clip, volume, 0f);
    }

    public void PlayAudio(AudioClip clip, float volume, float spatialBlend)
    {
        PlayAudio(clip, volume, spatialBlend, Vector3.zero);
    }

    public void PlayAudio(AudioClip clip, float volume, float spatialBlend, Vector3 position)
    {
        if (clip == null)
        {
            Debug.LogError("Audioclip is null");
        }
        AudioSource aS = getFreeSource();
        aS.transform.position = position;
        aS.spatialBlend = spatialBlend;
        aS.volume = volume;
        aS.clip = clip;
        aS.Play();
    }
    
    public void StopAudio()
	{
		foreach(AudioSource aS in aSources)
		{
            if(aS != musicSource)
            {
    			aS.Stop();
            }
		}
	}

    private AudioSource getFreeSource()
    {
        foreach(AudioSource aS in aSources)
        {            
            if(!aS.isPlaying)
            {
                return aS;
            }
        }
        
        Debug.Log("Creating new AudioSource for a total of " + aSources.Count);
        return AddNewAudioSource();
    }

    private AudioSource AddNewAudioSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = 1f;
        source.spatialBlend = 0f;
        aSources.Add(source);

        return source;
    }

    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    public void PlayMusic(AudioClip music)
    {
        if(musicSource == null)
        {
            musicSource = AddNewAudioSource();
            musicSource.loop = true;
        }
        musicSource.clip = music;
        musicSource.Play();
    }

    public void SetMusicVolume(float vol)
    {
        if(musicSource != null)
        {
            musicSource.volume = vol;
        }
    }

}
