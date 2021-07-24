using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyMenu : MonoBehaviour
{
    public static DifficultyMenu instance;
    public Dropdown difficulty;
    public Dropdown money;
    public Dropdown happy;
    public Dropdown policyPoints;
    public Dropdown healthy;
    public Dropdown dDay;
    public InputField nameInput;
    public Text errorNameText;
    public GameObject difficultySelectionMenu;
    public GameObject nameInputMenu;

    private string ID;
    private bool switchingDifficulty = false;

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void show() {
        this.gameObject.SetActive(true);
        reset();
    }

    public void hide() {
        reset();
        this.gameObject.SetActive(false);
    }

    public void reset()
    {
        difficulty.value = 1;
        normalDifficulty();
        toNameMenu();
    }

    public void setDifficulty() {
        switchingDifficulty = true;
        int difficultyNum = difficulty.value;
        switch (difficultyNum) {
            case 0:
                easyDifficulty();
                break;
            case 1:
                normalDifficulty();
                break;
            case 2:
                hardDifficulty();
                break;
            default:
                break;
        }
        switchingDifficulty = false;
    }

    public void easyDifficulty() {
        //Set drop downs to the propper values for easy mode
        //Money = 10,000,000,000 = 10B
        money.value = 4;
        //Happy = 100%
        happy.value = 0;
        //PP = 5
        policyPoints.value = 2;
        //healthy = 300,000
        healthy.value = 5;
        //dDay = 10
        dDay.value = 3;
        //First policies on all beneficial trees unlocked.
    }
    public void normalDifficulty() {
        //Set drop downs to the propper values for normal mode
        //Money = 100,000,000
        money.value = 2;
        //Happy = 75%
        happy.value = 1;
        //PP = 1
        policyPoints.value = 1;
        //healthy = 60,000
        healthy.value = 1;
        //dDay = 7
        dDay.value = 2;
    }
    public void hardDifficulty() {
        //Set drop downs to the propper values for hard mode
        //Money = 50,000,000
        money.value = 1;
        //Happy = 50%
        happy.value = 2;
        //PP = 0
        policyPoints.value = 0;
        //healthy = 30,000
        healthy.value = 0;
        //dDay = 5
        dDay.value = 1;
    }

    public void switchToCustom() {
        if (difficulty.value != 3 && !switchingDifficulty) {
            difficulty.value = 3;
        }
    }

    public void startGame() {
        SaveManager.save_Static(ID, readGameInfo());
        SceneManager.LoadScene("Main Game");
    }

    private Data readGameInfo() {
        Data data = new Data(ID);
        data.setNewValues(getMoneyInfo(), getHappyInfo(), getPPInfo(), getHealthyInfo(), 0, 0, 1, getDDayInfo());
        return data;
    }

    public void toNameMenu() {
        nameInputMenu.SetActive(true);
        difficultySelectionMenu.SetActive(false);
    }

    public void toDifficultySelect() {
        if (checkName()) {
            nameInputMenu.SetActive(false);
            difficultySelectionMenu.SetActive(true);
        }
    }

    //Checks name Vadility.
    private bool checkName() {
        string name = nameInput.text;
        if (name.Equals(""))
        {
            errorNameText.text = "Name is too short";
            return false;
        }
        else if (name.Length > 16)
        {
            errorNameText.text = "Name is too long.";
            return false;
        }
        else if (name.ToLower().Equals("left empty") || name.ToLower().Equals("too short") || name.ToLower().Equals("too long"))
        {
            errorNameText.text = "Very funny. Choose a different name.";
            return false;
        }
        else if (name.ToLower().Equals("different name"))
        {
            errorNameText.text = "Haha. For real though. Enter a real name.";
            return false;
        }
        else if (name.ToLower().Equals("a real name") || name.ToLower().Equals("real name"))
        {
            errorNameText.text = "Alright we're done here. Do whatever you want.";
            return false;
        }
        else if (name.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0)
        {
            errorNameText.text = "Special Characters are not allowed.";
            return false;
        }
        else if (SaveManager.doesExist(name))
        {
            errorNameText.text = "Name Already Exists.";
            return false;
        }
        else
        {
            ID = name;
            PlayerPrefs.SetString("Save", name);
            return true;
        }
    }

    private int[] getMoneyInfo()
    {
        int moneyIndex = money.value;
        string moneyString = "";
        switch (moneyIndex)
        {
            case 0:
                moneyString = "1000000"; // 1M
                break;
            case 1:
                moneyString = "10000000"; // 10M
                break;
            case 2:
                moneyString = "100000000";// 100M
                break;
            case 3:
                moneyString = "1000000000";// 1B
                break;
            case 4:
                moneyString = "10000000000";// 10B
                break;
            case 5:
                moneyString = "100000000000";// 100B
                break;
        }
        return Formatter.stringToValue(moneyString);
    }
    private int getHappyInfo()
    {
        int happyIndex = happy.value;
        int happyPercent = 0;
        switch (happyIndex)
        {
            case 0:
                happyPercent = 100;
                break;
            case 1:
                happyPercent = 75;
                break;
            case 2:
                happyPercent = 50;
                break;
            case 3:
                happyPercent = 25;
                break;
        }
        return happyPercent;
    }
    private int getPPInfo()
    {
        int ppIndex = policyPoints.value;
        int pp = 0;
        switch (ppIndex)
        {
            case 0:
                pp = 0;
                break;
            case 1:
                pp = 3;
                break;
            case 2:
                pp = 5;
                break;
            case 3:
                pp = 7;
                break;
            case 4:
                pp = 10;
                break;
        }
        return pp;
    }
    private int getHealthyInfo()
    {
        int healthyIndex = healthy.value;
        int healthyVal = 0;
        switch (healthyIndex)
        {
            case 0:
                healthyVal = 30000;
                break;
            case 1:
                healthyVal = 60000;
                break;
            case 2:
                healthyVal = 90000;
                break;
            case 3:
                healthyVal = 120000;
                break;
            case 4:
                healthyVal = 240000;
                break;
            case 5:
                healthyVal = 300000;
                break;
            case 6:
                healthyVal = 500000;
                break;
            case 7:
                healthyVal = 1000000;
                break;
        }
        return healthyVal;
    }
    private int getDDayInfo()
    {
        int dDayIndex = dDay.value;
        int dDayVal = 0;
        switch (dDayIndex)
        {
            case 0:
                dDayVal = 1;
                break;
            case 1:
                dDayVal = 5;
                break;
            case 2:
                dDayVal = 7;
                break;
            case 3:
                dDayVal = 10;
                break;
        }
        return dDayVal;
    }

}
