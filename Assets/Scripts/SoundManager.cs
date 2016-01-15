using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
    
    public AudioSource efxSource;                   
    public AudioSource musicSource;                 
    public static SoundManager instance = null;                 
    public float lowPitchRange = .95f;              
    public float highPitchRange = 1.05f;            

    // Use this for initialization
    void Awake () {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public void PlaySingle(AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        efxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();
    }
}
