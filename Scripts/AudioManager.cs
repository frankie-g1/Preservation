using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource asBGM;
    public AudioSource asSFX;
    public AudioClip BGM;

    /// <summary>
    /// Plays given background music to
    /// </summary>
    private void Awake()
    {
        returnToDefaultBGM();
    }

    /// <summary>
    /// Plays given sound effect
    /// </summary>
    /// <param name="sfx"> any sound effect </param>
    public void playSFX(AudioClip sfx) {
        asSFX.PlayOneShot(sfx);
    }

    /// <summary>
    /// updates volumes for both audio sources with given values.
    /// </summary>
    /// <param name="bgm"> bgm volume given in whole numbers from 1 to 100</param>
    /// <param name="sfx"> bgm volume given in whole numbers from 1 to 100</param>
    public void updateVolume(float bgm, float sfx) {
        asBGM.volume = bgm/100;
        asSFX.volume = sfx/100;
    }

    /// <summary>
    /// plays given background music on loop
    /// </summary>
    /// <param name="bgm"> given background music</param>
    public void changeBGM(AudioClip bgm) {
        asBGM.clip = bgm;
        asBGM.Play();
        asBGM.loop = true;
    }

    /// <summary>
    /// Returns background music to default BGM;
    /// </summary>
    public void returnToDefaultBGM() {
        asBGM.clip = BGM;
        asBGM.Play();
        asBGM.loop = true;
    }

}
