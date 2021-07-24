using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject optionsPMenu;
    public GameObject mainPMenu;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public TextMeshProUGUI BGMVolText;
    public TextMeshProUGUI SFXVolText;
    public AudioManager AM;
    public bool optionsOpen = false;

    void Start()
    {
        loadOptions();
    }

    public void loadOptions() {
        float bgm = PlayerPrefs.GetFloat("BGMVolume");
        float sfx = PlayerPrefs.GetFloat("SFXVolume");
        AM.asBGM.volume = bgm;
        AM.asSFX.volume = sfx;
    }
    private void saveOptions()
    {
        PlayerPrefs.SetFloat("BGMVolume", AM.asBGM.volume);
        PlayerPrefs.SetFloat("SFXVolume", AM.asSFX.volume);
    }

    public void options() {
        optionsPMenu.SetActive(true);
        mainPMenu.SetActive(false);
        optionsOpen = true;
        updateVolumeBars();
    }

    public void back() {
        mainPMenu.SetActive(true);
        optionsPMenu.SetActive(false);
        optionsOpen = false;
        saveOptions();
    }

    

    /// <summary>
    /// Updates all Volume values
    /// </summary>
    public void updateVolume()
    {
        float bgm = BGMSlider.value;
        float sfx = SFXSlider.value;
        AM.updateVolume(bgm, sfx);
        BGMVolText.text = bgm.ToString();
        SFXVolText.text = sfx.ToString();

    }

    /// <summary>
    /// Updates all Volume UI values
    /// </summary>
    public void updateVolumeBars()
    {
        float bgm = AM.asBGM.volume * 100;
        float sfx = AM.asSFX.volume * 100;
        BGMSlider.value = bgm;
        SFXSlider.value = sfx;
        BGMVolText.text = bgm.ToString();
        SFXVolText.text = sfx.ToString();

    }

}
