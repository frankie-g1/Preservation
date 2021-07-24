using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data 
{

    public string saveID;//Which save file you are

    // Q   T   B     M  TH   H      <----- Suffixes for those places
    //100,100,100,||100,100,101     <----- Value of 100,100,100,100,100,100 = One Hundred Quadrillion One Hundred Trillion One Hundred Billion One Hundred Million One Hundred Thousand One Hundred and One
    //  money[1]      money[0]
    //Floats could have been used but this method is almost infinetly expansive and also forgot about floats when initially developing
    //If you want to support more than Quadrillion then you have to change the Format class to support higher values and their suffixes.
    public int[] money;

    public int happiness;//Happiness Percentage
    public int PP;//Policy Points
    public int healthy;//Healthy Population
    public int infected;//Infected Population
    public int deaths;//Amount dead from infection of your population
    public int day;//Current Day
    public int dDay;//Start of infection Day (DoomsDay)


    public bool[] unlockedPoliciesTreeHospital;
    public bool[] unlockedPoliciesTreeRestrictions;
    public bool[] unlockedPoliciesTreePSA;
    public bool[] unlockedPoliciesTreeTravel;
    public bool[] unlockedPoliciesTreeSick;

    public Data(string saveID) {
        this.saveID = saveID;
        this.money = new int[2];
    }

    public void setNewValues(int[] money, int happiness, int PP, int healthy, int infected, int deaths, int day, int dDay) {
        this.money = money;
        this.happiness = happiness;
        this.PP = PP;
        this.healthy = healthy;
        this.infected = infected;
        this.deaths = deaths;
        this.day = day;
        this.dDay = dDay;
        initTrees();
    }

    //Initializes the trees to fill them with false boolean values.
    public void initTrees()
    {
        this.unlockedPoliciesTreeHospital = new bool[20];
        this.unlockedPoliciesTreeRestrictions = new bool[20];
        this.unlockedPoliciesTreePSA = new bool[20];
        this.unlockedPoliciesTreeTravel = new bool[20];
        this.unlockedPoliciesTreeSick = new bool[20];

        Formatter.resetTree(this.unlockedPoliciesTreeHospital);
        Formatter.resetTree(this.unlockedPoliciesTreeRestrictions);
        Formatter.resetTree(this.unlockedPoliciesTreePSA);
        Formatter.resetTree(this.unlockedPoliciesTreeTravel);
        Formatter.resetTree(this.unlockedPoliciesTreeSick);

    }

    //Default resetter for the game.
    public void reset() {
        this.money[0] = 100000000;
        this.money[1] = 0;
        this.happiness = 100;
        this.PP = 1;
        this.healthy = 684379;
        this.infected = 0;
        this.deaths = 0;
        this.day = 0;
        this.dDay = 7;
        initTrees();
    }

}
