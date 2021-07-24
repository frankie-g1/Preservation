using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Formulas
{
    static Queue DeathQueue = new Queue();
    //Calculates the next days newly infected and newly dead population.
    public static int CalculateNewInfected(Data data)
    {
        if (data.infected == 0) {
            return 0;
        }
        int daysInfected = data.day - data.dDay;

        (double E, double P) = getEandP(); // E & P stop out at 0.
        double Rate = E * P;

        
        //new infected = E(average number of people who one person can expsoe a day) * P(the probability of each exposure being real)^the day * the number of cases on a given day)  
        //Debug.Log("E * P =  " + Rate);
        
        //To keep our numbers low


        
       // float _nInfected = Mathf.Pow( (float) (1 + (E * P)), daysInfected) * 36; //exponential one made from simplifying other found equations and also based on the videos and
        float _nInfected = (int)(1 + (E * P)) ^ data.day * 36;
        int nInfected = (int) _nInfected;

        if(nInfected < 0) // Huge ass negative number fixer perhaps?
        {
            nInfected = data.healthy;
        }


        if(data.infected + nInfected >= data.healthy) // here it possibly goes overboard, so this smooth's out the numbers (really only used to make the infected population make sense at a losing screen)
        {
            nInfected = data.healthy;
        }
        
        return nInfected;


        //Upcoming equation below 

        //float totalPopulation = data.infected + data.healthy;
        //int nInfected = (int)(Mathf.Pow((float)(1 + (E * P)), data.day) * ((float)data.infected / (totalPopulation)) * 36);  //logamirthic equation that i want to use but is being weird

    }

    public static void refreshDeathQueue()
    {
        DeathQueue = new Queue();
    }

    public static (int, int) CalculateNewDeaths(int nInfected, Data data)   // Here, we don't modify currData at all. Say if 70 people get infected, 3 days later we calculate how many of them die and how many recover. 
    {                                                                       // Easy subtraction with return variables is done in game manager.
        double deathRate = .003;                                            // The return variables : recover adds to healthy. Those who die add to deaths                
        int recovered = 0;                                                  // Side note, currData.infected is not prior to this method, only after. 
        int died = 0;

        if(nInfected > 0)
        {
            DeathQueue.Enqueue(nInfected);
        }
        int beenSick; //    becomes Dequeue


        if(DeathQueue.Count > 3) // People initially start dying 3 days after D day. Needs a queue of three to run... Not good but i use an else if for when its been three days past dday.
        {
            Debug.Log("Ran queue");
            beenSick = (int) DeathQueue.Dequeue();
            recovered = beenSick - (Random.Range(0, (int)(beenSick * deathRate))); //with deathrate being a bit more volitile this will simulate it well working on getting a new equation 
            died = beenSick - recovered;
        }

        else if(DeathQueue.Count < 3 && data.infected > 0 && data.day - data.dDay > 3)      //Prevent us from being stuck at 0 nInfected. Never runs really unless they are doing a great job
        {
            Debug.Log("Ran data.infected");
            recovered = data.infected - (Random.Range(0, (int)(data.infected * deathRate))); // Some extra die, but recovered stays the same. 
            died = data.infected - recovered;
        }

        if(DeathQueue.Count > 14)               //Happens when recovered is added back to the queue 11 times 
        {
            Debug.Log("14+ Queue Size woah");
            int oldRecovered = recovered;
            recovered -= (Random.Range(0, (int)(recovered * deathRate)));
            died += (oldRecovered - recovered);
        }
        else if(recovered * 2 < data.infected)  // 1.) Way too many are infected. 2.) More than half of the infected population was from 1 particular day.
        {
            Debug.Log("Kept infected");
            int remainder = recovered % 2;
            if(remainder == 0)
            {
                DeathQueue.Enqueue(recovered /2);      //  Recovered + infected are the playable population
                recovered = recovered / 2;  
            }
            else
            {                
                DeathQueue.Enqueue(recovered /2);      //  Recovered + infected are the playable population
                recovered = (recovered / 2) + remainder;
            }

        }

        return (died, recovered); //We let people get infected twice... Should we make a second deathQueue?

    }
    //this will check if certain policies are passed to see if the rate an infected person will infect more people
    private static (double, double) getEandP()
    {
        (bool[] passedHospital, bool[] passedRestrictions, bool[] passedPSA, bool[] passedTravel, bool[] passedSick) = GameManager.getInstance().getPassedPolicies();
        (Policy[] Hospital, Policy[] Restrictions, Policy[] PSA, Policy[] Travel, Policy[] Sick) = PolicyManager.getPolicies();

        double E = 5.1;//Default E value
        double P = .9;//Default P Value

        double eCum = 0;//Cumulative E
        double pCum = 0;//Cumulative P
        int i = 0;

        //Searches through passed Policies for their E and P values.
        while(i < passedHospital.Length)
        {
            if(passedHospital[i])
            {
                eCum += Hospital[i].getE();
                pCum += Hospital[i].getP();
            }
            i++;
        }

        i = 0;
        while(i < passedRestrictions.Length)
        {
            if(passedRestrictions[i])
            {
                eCum += Restrictions[i].getE();
                pCum += Restrictions[i].getP();
            }
            i++;
        }

        i = 0;
        while (i < passedPSA.Length)
        {
            if (passedPSA[i])
            {
                eCum += PSA[i].getE();
                pCum += PSA[i].getP();
            }
            i++;
        }

        i = 0;
        while (i < passedTravel.Length)
        {
            if(passedTravel[i])
            {
                eCum += Travel[i].getE();
                pCum += Travel[i].getP();
            }
            i++;
        }

        i = 0;
        while(i < passedSick.Length)
        {
            if(passedSick[i])
            {
                eCum += Sick[i].getE();
                pCum += Sick[i].getP();
            }
            i++;
        }

        //Adds cumulatives to total E and P
        E += eCum;
        P += pCum;
        if(E < 0)
        {
            E = 0;
        }
        if(P < 0)
        {
            P = 0;
        }
        return (E, P);

    }
   
}
