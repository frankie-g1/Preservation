using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Assign this script to each Policy Button?
public class Policy
{

    // Policies are constructed and stored in the Manager array. 
    // These unique vars are required for:
    // Policy manager checking prerequisites.
    // & passing string info to the description window text.
    private int parentHospitalIndex;
    private int parentRestrictionIndex;
    private int parentPSAIndex;
    private int parentTravelIndex;
    private int parentSickIndex;
    private string name;
    private int points_needed;
    private int[] money_cost;
    private int turn_delay;
    private string description;
    private double e;
    private double p;

    //Super specific constructor
    public Policy(string name, string description, string money_cost, int points_needed, int turn_delay, int parentHospitalIndex, int parentRestrictionIndex, int parentPSAIndex, int parentTravelIndex, int parentSickIndex, double e, double p)
    {
        this.name = name;
        this.description = description;

        this.points_needed = points_needed;
        this.money_cost = Formatter.stringToValue(money_cost);

        this.turn_delay = turn_delay;

        this.parentHospitalIndex = parentHospitalIndex;
        this.parentRestrictionIndex = parentRestrictionIndex;
        this.parentPSAIndex = parentPSAIndex;
        this.parentTravelIndex = parentTravelIndex;
        this.parentSickIndex = parentSickIndex;
        
        this.e = e;
        this.p = p;

    }

    //Less specific constructor. Only 1 dependancy.
    public Policy(string name, string description, string money_cost, int points_needed, int turn_delay, int tree, int index, double e, double p) {
        this.name = name;
        this.description = description;

        this.points_needed = points_needed;
        this.money_cost = Formatter.stringToValue(money_cost);

        parentHospitalIndex = -1;
        parentRestrictionIndex = -1;
        parentPSAIndex = -1;
        parentTravelIndex = -1;
        parentSickIndex = -1;

        this.turn_delay = turn_delay;
        switch (tree) {
            case 1:
                parentHospitalIndex = index;
                break;
            case 2:
                parentRestrictionIndex = index;
                break;
            case 3:
                parentPSAIndex = index;
                break;
            case 4:
                parentTravelIndex = index;
                break;
            case 5:
                parentSickIndex = index;
                break;
            default:
                Debug.Log("Tree does not exist " + tree + ".");
                break;
        }
        this.e = e;
        this.p = p;
    }


    


    //Less specific constructor. No dependancies.
    public Policy(string name, string description, string money_cost, int points_needed, int turn_delay, double e, double p)
    {
        this.name = name;
        this.description = description;

        this.points_needed = points_needed;
        this.money_cost = Formatter.stringToValue(money_cost);

        parentHospitalIndex = -1;
        parentRestrictionIndex = -1;
        parentPSAIndex = -1;
        parentTravelIndex = -1;
        parentSickIndex = -1;

        this.e = e;
        this.p = p;

    }

    public Policy() : this("Filler", "Filler", "0", 0, 0, 0.0, 0.0) {}

    public string getName()
    {
        return name;
    }

    public string getDescription()
    {
        return description;
    }

    public double getE()
    {
        return e;
    }

    public double getP()
    {
        return p;
    }

    public string getDependancies() {
        return getParentHospital() + "," + getParentRestrictions() + "," + getParentPSA() + "," + getParentTravel() + "," + getParentTravel(); 
    } 

    public bool requiredPoliciesPassed(){
        if (getParentHospital() >= 0 || getParentRestrictions() >= 0 || getParentPSA() >= 0 || getParentTravel() >= 0 || getParentSick() >= 0) {
            (bool[] passedHospital, bool[] passedRestrictions, bool[] passedPSA, bool[] passedTravel, bool[] passedSick) = GameManager.getInstance().getPassedPolicies();
            (bool passed1, bool passed2, bool passed3, bool passed4, bool passed5) = (true,true,true,true,true);
            if (getParentHospital() > -1) {
                passed1 = passedHospital[getParentHospital()];
            }
            if (getParentRestrictions() > -1)
            {
                passed2 = passedRestrictions[getParentRestrictions()];
            }
            if (getParentPSA() > -1)
            {
                passed3 = passedPSA[getParentPSA()];
            }
            if (getParentTravel() > -1) {
                passed4 = passedTravel[getParentTravel()];
            }
            if (getParentSick() > -1)
            {
                passed5 = passedSick[getParentSick()];
            }
            return (passed1 && passed2 && passed3 && passed4 && passed5);
        }else
        {
            return true;
        }
    }
    
    public int getParentHospital()
    {
        return parentHospitalIndex;
    }

    public int getParentRestrictions()
    {
        return parentRestrictionIndex;
    }

    public int getParentPSA()
    {
        return parentPSAIndex;
    }

    public int getParentTravel()
    {
        return parentTravelIndex;
    }

    public int getParentSick()
    {
        return parentSickIndex;
    }

    public int[] getMoney()
    {
        return money_cost;
    }

    public int getPoints()
    {
        return points_needed;
    }

}




