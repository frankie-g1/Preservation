using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PolicyManager : MonoBehaviour
{

    private static GameManager gm;
    private static Policy[] hospital;
    private static Policy[] restrictions;
    private static Policy[] PSA;
    private static Policy[] travel;
    private static Policy[] sick;
    private int reqIndex;   // Policy Manager works with one Policy at a time, specified by button click and then grabbed within a certain generated Policy[] 
    private int reqTree;
    private string errorMsg;

    private void Awake()
    {
        gm = GameManager.getInstance();
        hospital = generateHospitalPolicyTree();
        restrictions = generateRestrictionsPolicyTree();
        PSA = generatePSAPolicyTree();
        travel = generateTravelPolicyTree();
        sick = generateSickPolicyTree();
    }


    
    public void passPolicy()                            // 1.) Set bool index to true 
                                                        // 2.) Game Manager subtracts money from data 
                                                        // 3.) Policy manager will subtract Policy points from data
                                                        // 4.) Send change to the infection rate.
                                                        // Requirements have already been checked prior to this method.
    {
        switch (reqTree)
        {
            case 1: 
                gm.currData.unlockedPoliciesTreeHospital[reqIndex] = true; 
                gm.currData.PP = gm.currData.PP - hospital[reqIndex].getPoints();
                gm.spendMoney(hospital[reqIndex]);
                break;
            case 2:
                gm.currData.unlockedPoliciesTreeRestrictions[reqIndex] = true;
                gm.currData.PP = gm.currData.PP - restrictions[reqIndex].getPoints();
                gm.spendMoney(restrictions[reqIndex]);
                break;
            case 3:
                gm.currData.unlockedPoliciesTreePSA[reqIndex] = true;
                gm.currData.PP = gm.currData.PP - PSA[reqIndex].getPoints();
                gm.spendMoney(PSA[reqIndex]);
                Debug.Log("Spending PAA money");
                break;
            case 4:
                gm.currData.unlockedPoliciesTreeTravel[reqIndex] = true;
                gm.currData.PP = gm.currData.PP - travel[reqIndex].getPoints();
                gm.spendMoney(travel[reqIndex]);
                break;
            case 5:
                gm.currData.unlockedPoliciesTreeSick[reqIndex] = true;
                gm.currData.PP = gm.currData.PP - sick[reqIndex].getPoints();
                gm.spendMoney(sick[reqIndex]);
                break;
            default:
                Debug.Log("No existing tree");
                break;
        }
    }


    public void setReqIndex(int i) {            //Every policy button needs these specified in the inspector's onclick
        this.reqIndex = i;
    }

    public void setReqTree(int t) {             //Every policy button needs these specified in the inspector's onclick
        this.reqTree = t;
    }

    public void showDesc()
    {
        Policy displayMe;
        switch (reqTree)
        {
            case 1:
                displayMe = hospital[reqIndex];
                Debug.Log("H tree");
                break;
            case 2:
                displayMe = restrictions[reqIndex];
                Debug.Log("R tree");
                break;
            case 3:
                displayMe = PSA[reqIndex];
                Debug.Log("P tree");
                break;
            case 4:
                displayMe = travel[reqIndex];
                Debug.Log("T tree");
                break;
            case 5:
                displayMe = sick[reqIndex];
                Debug.Log("Sick tree");
                break;

            default:
                Debug.Log("No existing tree");
                return;
        }

        if (passRequirements(displayMe))
        {
            String x = String.Format($"{displayMe.getDescription()}\n\nCosts below:\n\t{displayMe.getPoints()} policy points\n\t{Formatter.moneyString(displayMe.getMoney())} dollars");
            DescriptionWindow.showDescription_static(displayMe.getName(), x);
        }
        else
        {
            String x = String.Format($"{displayMe.getDescription()}\n\nCosts below:\n\t{displayMe.getPoints()} policy points\n\t{Formatter.moneyString(displayMe.getMoney())} dollars");
            DescriptionWindow.showDescription_staticError(displayMe.getName(), x, errorMsg);
        }

    }

    public void hideDesc()
    {
        DescriptionWindow.hideDesc_Static();
    }

    public bool passRequirements(Policy displayMe)    // Use boolean[] to find latest passed policies of each tree.
                                                        // Check if already have the policy, available money and policy points available. 
                                                        // Return earliest error string or empty string
                                         
    {   
        if(!displayMe.requiredPoliciesPassed())
        {
            errorMsg = getPolicyReqString();
            return false;
        }

        if(currIsPassed())
        {
            errorMsg = "You already own this policy!";
            return false;
        }

        int[] spending = displayMe.getMoney();
        int[] leftOver = new int[2];
        leftOver[1] = gm.currData.money[1] - spending[1]; 
        leftOver[0] = gm.currData.money[0] - spending[0];
        if (leftOver[1] < 0) {
            leftOver[1] = -1;
        }
        if (leftOver[0] < 0) {
            leftOver[0] = -1;
        }

        if(leftOver[1] > 0 && leftOver[0] < 0)
        {
            //do nothing
            //have enough billions to cover the missing millions
        }
        else if(leftOver[1] == 0 && leftOver[0] > 0)
        {
            //do nothing
            //nothing costs over a billion anyways
        }
        else if(leftOver[1] < 0 || leftOver[0] < 0)
        {
            errorMsg = String.Format($"You are missing {Formatter.moneyString(leftOver)}!");
            return false;
        }

        if(gm.currData.PP < displayMe.getPoints()) 
        {
            int dif = displayMe.getPoints() - gm.currData.PP;
            errorMsg = $"Need {dif} more policy points";
            return false;
        }       

        return true;  

    }

    public String getPolicyReqString()
    {
        (bool[] h, bool[] r, bool[] p, bool[] t, bool[] s) = gm.getPassedPolicies();
        String allReqs = "Required Policies:\n";
        Policy[] tree = new Policy[10];

        switch (reqTree) {
            case 1:
                tree = hospital;
                break;
            case 2:
                tree = restrictions;
                break;
            case 3:
                tree = PSA;
                break;
            case 4:
                tree = travel;
                break;
            case 5:
                tree = sick;
                break;
            default:
                Debug.Log("No Tree Exists at this index");
                break;
        }

        if (tree[reqIndex].getParentHospital() >= 0 && !h[tree[reqIndex].getParentHospital()])
        {
                allReqs += String.Format(hospital[tree[reqIndex].getParentHospital()].getName() + "\n");
        }
        if (tree[reqIndex].getParentRestrictions() >= 0 && !r[tree[reqIndex].getParentRestrictions()])
        {
                allReqs += String.Format(restrictions[tree[reqIndex].getParentRestrictions()].getName() + "\n");
        }
        if (tree[reqIndex].getParentPSA() >= 0 && !p[tree[reqIndex].getParentPSA()])
        {
                allReqs += String.Format(PSA[tree[reqIndex].getParentPSA()].getName() + "\n");
        }
        if (tree[reqIndex].getParentTravel() >= 0 && !t[tree[reqIndex].getParentTravel()])
        {
                allReqs += String.Format(travel[tree[reqIndex].getParentTravel()].getName() + "\n");
        }
        if (tree[reqIndex].getParentSick() >= 0 && !s[tree[reqIndex].getParentSick()])
        {
                allReqs += String.Format(sick[tree[reqIndex].getParentSick()].getName() + "\n");
        }

        return allReqs;
    }


    public bool currIsPassed() {
        switch (reqTree)
        {
            case 1:
                return gm.currData.unlockedPoliciesTreeHospital[reqIndex];
            case 2:
                return gm.currData.unlockedPoliciesTreeRestrictions[reqIndex];
            case 3:
                return gm.currData.unlockedPoliciesTreePSA[reqIndex];
            case 4:
                return gm.currData.unlockedPoliciesTreeTravel[reqIndex];
            case 5:
                return gm.currData.unlockedPoliciesTreeSick[reqIndex];
            default:
                Debug.Log("No existing tree");
                return false;

        }
    }


    //Always have 0 as filler policy
    public Policy[] generateHospitalPolicyTree()
    {
        Policy[] arr = new Policy[6];

        //establish field hospital tree   
        



        arr[0] = new Policy("Establish Field Hospital Teir 1", " Establish a field hospital to take pressure off hospitals", "1000000", 1, 0, -0.1, -0.1); //0
        arr[1] = new Policy("Establish Field Hospital Teir 2", " Establish a field hospital to take pressure off hospitals", "2000000", 2, 0, 1, 0, -0.1, -0.1); //1
        arr[2] = new Policy("Establish Field Hospital Teir 3", " Establish a field hospital to take pressure off hospitals", "3000000", 3, 0, 1, 1, -0.1, -0.1); //2
      




        //buy more ventilators tree
        arr[3] = new Policy("Buy More Ventilators", " Buy more ventilators off of the federal governments stockpile", "5000000", 1, 0, -0.1, -0.1); //3

        //ICU tree
        arr[4] = new Policy("Increase ICU Capacity", " ICU Capacity", "2000000", 1, 0, -0.1, -0.1); //2

        //alt tree
        arr[5] = new Policy("alternative path", "this path will have negative effects", "10000000", 1, 0, 0.1, 0.1); //2




        return arr;
    }

    public Policy[] generateRestrictionsPolicyTree()
    {
        Policy[] arr = new Policy[6];
        arr[0] = new Policy("Limit Large Gatherings", "Limit and monitor known large gatherings to help monitor and contain the spread", "20000", 2, 0, -0.1, -0.1); //0

        arr[1] = new Policy("Ban Large Gatherings", "Ban large gatherings to help slow the spread of the virus", "30000", 4, 0, 2, 0, -0.15, -0.1); //1

        arr[2] = new Policy("Limit Indoor Gatherings", "Limit and monitor known gatherings of people in an eclosed setting", "500000", 2, 0, 2, 1, -0.1, -0.05); //2

        arr[3] = new Policy("Limit Outdoor Gatherings", "Limit and monitor known gatherings of people in an open area", "500000", 2, 0, 2, 1, -0.1, -0.05); //3

        arr[4] = new Policy(" Further Limit Indoor Gatherings", " Further limit and monitor known gatherings of people in an enclosed setting", "500000", 3, 0, 2, 2, -.08, -0.05); //4



        arr[5] = new Policy("alternative path", "this path will have negative effects", "20000000", 1, 0, 0.1, 0.1); //5
        return arr;
    }

    public Policy[] generatePSAPolicyTree()         //Generate Policy Trees filled with Policies
    {
        Policy[] arr = new Policy[9];
        arr[0] = new Policy("Weekly COVID-19 Public Updates", "Personally update the public weekly on the current state of the virus and infected and death counts", "500000", 2, 0, -0.1, -0.1); //1
        arr[1] = new Policy("Daily COVID-19 Public Updates", "Personally update the public Daily on the current state of the virus and infected and death counts", "500000", 3, -1, 3,0, -0.2, -0.2); //2


        arr[2] = new Policy("Have Experts Speak", "Host a panel of leading experts on infectious dieseases and the COVID-19 virus", "50000", 4, -1, -0.2, -0.1); //2

        arr[3] = new Policy("Covid InfoGraphics", "Create a series of infographics to help visual important topic to the public", "150000", 2, -1, -0.2, -0.2); //3

        arr[4] = new Policy("Website", "Create a website that informs the public about important COVID related statistics and information", "485000", 1, -1, -0.2, -0.2); //3



        arr[5] = new Policy("Missinform", "Spread false information about the current situation and brush off the new virus", "1000", 1, -1, 0.2, 0.2); //4
        arr[6] = new Policy("Lie", "Decieve the public with lies and encourage them to not take any precautions", "1000", 1, -1, 3, 5, 0.2, 0.2); //5
        arr[7] = new Policy("Further lie", "Spread more lies and misinformation to the public causing them to completly ignore any precautions", "180000000", 1, -1, 3, 6, 0.2, 0.2); //6
        arr[8] = new Policy("Denouce experts", "Denounce and disavow any and all medical or research professionals that want to warn the public about the virus", "180000000", 1, -1, 3, 7, 0.2, 0.2); //7


        return arr;
    }
    
    public Policy[] generateTravelPolicyTree()
    {
        Policy[] arr = new Policy[8];
        arr[0] = new Policy("Impose interstate restritions", "Pass restrictions for citizens traveling between different states.", "1000000", 2, 0, -0.05,0); //1
        arr[1] = new Policy("Interstate travel Quarentine", "Institute a manditory quarentine for anyone traveling into the state.", "1000000", 2, -1, 4, 0, -0.1, 0.01); //2

        arr[2] = new Policy("Impose international restrictions", "Pass restrictions for citizens traveling between different countries", "1000000", 2, -1, -0.05, 0); //3
        arr[3] = new Policy("Internation travel Quarentine", "Institute a manditory quarentine for anyone traveling into the state", "1000000", 2, -1, 4, 2, -0.1, -.01); //4


        arr[4] = new Policy("Close Borders", "Start to close off the borders to specific states and countries", "5000000", 3, -1, -0.05, 0); //5
        arr[5] = new Policy("Close Land Borders", "Stop all nonessential land based travel to and from the state", "5000000", 4, -1, 4, 4, -0.15,0); //6
        arr[6] = new Policy("Close Water borders", "Stop all nonessential naval access to and from the state", "5000000", 4, -1, 4, 4, -0.15, 0); //7
        arr[7] = new Policy("Close Airports", "Stop all nonessential flights in and from the state", "5000000", 4, -1, 4, 4, -0.15, -0.01); //7

        return arr;
    }

    public Policy[] generateSickPolicyTree()
    {
        Policy[] arr = new Policy[10];

        //vaccine tree
        arr[0] = new Policy("Vaccine Funding", "Start using government funds to fund companies trying to rush a vaccine", "50000000", 1, 0, 0, 0); //0
        arr[1] = new Policy("Increase Funding", "Allocate more funds to help further vaccine research", "50000000", 1, 0, 5, 0, 0, 0); //1
        arr[2] = new Policy("Give Special Permissions to Companies", "Give companies special permissions on testing and benefits", "1000000", 4, 0, 5, 1, 0, 0); //2
        arr[3] = new Policy("Further Funding increase", "Further the amount of money given to companies", "50000000", 1, 0, 5, 2, 0,0); //3
        arr[4] = new Policy("Allow Human trials", "Some companies have had strides in animal testing with possible vaccines and now want to test on humans", "1000000", 5, 0, 5, 3, 0, 0); //4
        arr[5] = new Policy("Give FDA Approval", "The FDA gives emergency clearance to start producing and administering a vaccine for COVID-19", "1000000", 5, 0, 5, 4, 0, 0); //5
        arr[6] = new Policy("Develop a Vaccine", "A Vaccine is now developed and being admistered to the population", "50000000", 1, 0, 5, 5, -0.5, -0.25); //6

        //quarentine tree
        arr[7] = new Policy("7-Day Quarentine", "Implement a 7-Day quarentine for anyone who has a confirmed test", "100000", 1, 0, -0.08, 0); //7
        arr[8] = new Policy("14-Day Quarentine", "Extend the mandatory quarentine for any confirmed cases", "2000000", 1, 0, 5, 7, -.12, -0.1); //8

        //contact tracing
        arr[9] = new Policy("Contract Tracing", "Set up a system to trace people an infected person ", "15000000", 1, 0, -0.15, 0); //9
        return arr;
    }

    public static (Policy[], Policy[], Policy[], Policy[], Policy[]) getPolicies()
    {
        return  (hospital,restrictions,PSA,travel,sick);
    }
}
