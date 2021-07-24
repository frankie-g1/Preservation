using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    public AudioManager AM;
    public ButtonManager BM;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public TextMeshProUGUI BGMVolText;
    public TextMeshProUGUI SFXVolText;
    public GameObject DefaultMenu;
    public GameObject OptionsMenu;
    public GameObject AboutMenu;
    public GameObject LoadMenu;
    public DifficultyMenu difficultyMenu;
    public TextMeshProUGUI selectedSaveText;


    public GameObject playButton;
    public GameObject loadButton;

    private Vector3 playOrgPos;

    public GameObject loadPrefab;
    private string save = "";

    private void Awake()
    {
        loadOptions();
        DefaultMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        difficultyMenu.hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        playOrgPos = playButton.transform.position;
        loadCheck();
    }

    //Dynamically changes the play button depending on if the user has pre-existing saves on their device.
    public void loadCheck() {
        if (!SaveManager.doesAnyExist())
        {
            loadButton.SetActive(false);
            playButton.transform.position = loadButton.transform.position;
            playButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("Play");
        }
        else {
            loadButton.SetActive(true);
            playButton.transform.position = playOrgPos;
            playButton.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().SetText("New Game");
        }
    }

    public void selectSave(string s) {
        selectedSaveText.text = "Selected: " + s;
        save = s;
    }

    //Loads all options found within player prefs into the options menu.
    public void loadOptions()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume");
        float sfx = PlayerPrefs.GetFloat("SFXVolume");
        AM.asBGM.volume = bgm;
        AM.asSFX.volume = sfx;
    }

    //Saves all the options within the options menu into player prefs.
    private void saveOptions()
    {
        PlayerPrefs.SetFloat("BGMVolume", AM.asBGM.volume);
        PlayerPrefs.SetFloat("SFXVolume", AM.asSFX.volume);
    }

    /// <summary>
    /// Updates all Volume values
    /// </summary>
    public void updateVolume() {
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

    /// <summary>
    /// Begins gameplay
    /// </summary>
    public void Play() {
        DefaultMenu.SetActive(false);
        saveOptions();
        difficultyMenu.show();
    }

    //Brings up and fills load screen with names on save files.
    public void load()
    {
        DefaultMenu.SetActive(false);
        LoadMenu.SetActive(true);
        loadFill();
    }

    public void loadFill() {

        GameObject content = LoadMenu.transform.Find("Panel").transform.Find("Scroll View").GetComponent<ScrollRect>().content.gameObject;

        //Clears all existing Childeren of content object
        var children = new List<GameObject>();
        foreach (Transform child in content.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        string[] ids = SaveManager.allSaveIDs_Static();
        if (ids.Length == 0) {
            selectedSaveText.text = "No Saves Found.";
            loadCheck();
            return;
        }

        //Creates all prefabs from existing save files within save location
        for (int i = 0; i < ids.Length; i++) {
            GameObject load = Instantiate(loadPrefab);
            string name = ids[i];

            load.transform.Find("Name").GetComponent<Text>().text = name;
            load.transform.Find("Number").GetComponent<Text>().text = (i + 1).ToString();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { selectSave(name);});
            load.GetComponent<EventTrigger>().triggers.Add(entry);

            load.transform.SetParent(content.transform);
        }
    }

    public void loadSave(string saveID) {
        PlayerPrefs.SetInt("Load", 1);
        PlayerPrefs.SetString("Save", saveID);
        SceneManager.LoadScene("Main Game");
    }

    public void loadSave()
    {
        if (selectedSaveText.text.Equals("No Saves Found.")) {
            return;
        }
        if (save.Equals("")) {
            selectedSaveText.text = "Please Select a Save.";
            return;
        }
        PlayerPrefs.SetInt("Load", 1);
        PlayerPrefs.SetString("Save", save);
        SceneManager.LoadScene("Main Game");
    }

    public void deleteSave() {
        if (selectedSaveText.text.Equals("No Saves Found."))
        {
            return;
        }
        if (save.Equals(""))
        {
            selectedSaveText.text = "Please Select a Save.";
            return;
        }

        SaveManager.saveDelete_Static(save);
        loadFill();
    }

    /// <summary>
    /// Quits the Game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Makes Options Menu Appear
    /// </summary>
    public void Options() {
        DefaultMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        updateVolumeBars();
    }

    /// <summary>
    /// Makes About menu Appear
    /// </summary>
    public void About()
    {
        DefaultMenu.SetActive(false);
        AboutMenu.SetActive(true);
    }

    /// <summary>
    /// Makes Default Menu Appear
    /// </summary>
    public void Back() {
        DefaultMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        AboutMenu.SetActive(false);
        difficultyMenu.hide();
        selectSave("");
        LoadMenu.SetActive(false);
        saveOptions();
    }

}
