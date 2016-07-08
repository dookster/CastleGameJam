using UnityEngine;
using System.Collections;

public class MusicFader : MonoBehaviour {

    public AudioSource musicSource;

    public float fadeSpeed = 2f;
    public float musicMaxVol = 0.5f;

    private float targetVolume = 0f;

	// Use this for initialization
	void Start ()
    {
        musicSource.volume = 0f;
        FadeUp();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(musicSource.volume < targetVolume)
        {
            musicSource.volume += (fadeSpeed * Time.deltaTime);
        }

        if (musicSource.volume > targetVolume)
        {
            musicSource.volume -= (fadeSpeed * Time.deltaTime);
        }
    }

    
    public void FadeUp()
    {
        targetVolume = musicMaxVol;
    }

    public void FadeDown()
    {
        targetVolume = 0f;
    }

}
