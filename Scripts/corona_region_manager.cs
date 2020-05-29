using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeighbourRegion {

    public corona_region_manager scr_RegionManiager;
    //what percentage of living people has traveled to this sector after every 1 Game Tick?
    public float f_FactorTravel;
    public string s_RegionName;

    NeighbourRegion(corona_region_manager scr_InRegionManager, float f_InFactorSpread, string s_InRegionName) {
        scr_RegionManiager = scr_InRegionManager;
        f_FactorTravel = f_InFactorSpread;
        //s_RegionName = s_InRegionName; //not needed
    }

    public void Travel(int i_infected) {
        if ((int)(f_FactorTravel * i_infected) <= 0) return;
        scr_RegionManiager.Infect((int)(f_FactorTravel * i_infected * scr_RegionManiager.f_FactorSpread * scr_RegionManiager.f_BubbleFactorSpread));
        return;
    }
}

public class corona_region_manager : MonoBehaviour {

    public string s_name;
    public int i_citizens;
    public int i_untouched;
    public int i_Infected;
    public int i_dead;
    public int i_Cured;
    //how many people does 1 infected infect after every 1 Game Tick.
    public float f_FactorSpread;
    public float f_BubbleFactorSpread = 1;
    //how likely is it for an infected person to die after every 1 Game Tick.
    public float f_FactorDeath;
    //how likely is it for an infected to be cured after every 1 Game Tick.
    public float f_FactorCure;
    public NeighbourRegion[] reg_Regions;
    public bool b_Riot = false;
    public bool b_LockDown = false;
    public float f_RiotThreshold;

    float f_DoInfect;
    float f_DoDieOfCorona;
    float f_DoCure;

    GameManager gm_GameManager;

    //current panic EXCLUDING death panic. (death panic: percent of citizens dead translates directly to percent of panic)
    public float f_Panic;
    //factor by which the current panic is multiplied after 1 Game Tick.
    public float f_PanicFactor; //NEVER USED

    //flat value of how much money the sector produces per 1 Game Tick.
    public int i_EconomyOutput;
    //factor by which the economy output is influenced (percent)
    public float f_OutputFactor;

    //Start is called before the first frame update.
    void Start() {
        i_untouched = i_citizens - i_Infected - i_dead - i_Cured;
        gm_GameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();
    }

    //Game Update is called once per Game Tick by GameManager.
    public void GameUpdate() {
        f_DoInfect += i_Infected * f_FactorSpread * f_BubbleFactorSpread;
        //Debug.Log(i_infected + " infected times factor " + f_FactorSpread + " equals " + f_DoInfect + " people to infect.");
        f_DoDieOfCorona += i_Infected * f_FactorDeath;
        f_DoCure += i_Infected * f_FactorCure;
        while (f_DoInfect >= 1) {
            f_DoInfect -= 1;
            Infect(1);
        }
        while (f_DoDieOfCorona >= 1) {
            f_DoDieOfCorona -= 1;
            //TODO save variable for infected people until end of udate!
            DieOfCorona(1);
        }
        while (f_DoCure >= 1) {
            f_DoCure -= 1;
            Cure(1);
        }
        foreach (NeighbourRegion reg_NeighbourRegion in reg_Regions) {
            if (!b_LockDown) {
                reg_NeighbourRegion.Travel(i_Infected);
            }
        }
        int i_Economy = gm_GameManager.GetCurrentEconomy();
        //float f_Alive = (float)(i_citizens - i_dead) / (float)i_citizens;
        //gm_GameManager.SetCurrentEconomy((int)((float)i_Economy + (float)i_EconomyOutput * f_OutputFactor * f_Alive));
        gm_GameManager.SetCurrentEconomy((int)((float)i_Economy + (float)i_EconomyOutput * GetOverallOutputFactor()));
        //Debug.Log("Output factor is " + GetOverallOutputFactor() + ", consisting of an output factor of " + f_OutputFactor + " and a living percentage of " + ((float)(i_citizens - i_dead) / (float)i_citizens) * 100 + "%.");

        //if panic too high, start riot
        if (f_Panic + ((float)i_dead / i_citizens) > f_RiotThreshold) {
            if (b_Riot) return;
            b_Riot = true;
            gm_GameManager.CallEvent(gm_GameManager.evt_RiotEvent);
            Time.timeScale = 0;
        }
    }

    //cure the amount of people. Returns how many people it couldn't Cure due to a lack of infected people.
    public int Cure(int i_people) {
        int i_failed = 0;
        i_Infected -= i_people;
        i_Cured += i_people;
        if (i_Infected < 0) {
            i_failed = -i_Infected;
            i_Infected = 0;
            i_Cured -= i_failed;
        }
        if (i_citizens != i_untouched + i_Infected + i_dead + i_Cured) {
            Debug.Log("Something went wrong curing " + i_people + " people in corona_region_manager of " + gameObject.name + ". Variable \"i_failed\" was " + i_failed + ".");
            return -1;
        }
        return i_failed;
    }

    //infect the amount of people. Returns how many people it couldn't infect due to a lack of untouched people.
    public int Infect(int i_people) {
        int i_failed = 0;
        i_Infected += i_people;
        i_untouched -= i_people;
        if (i_untouched < 0) {
            i_failed = -i_untouched;
            i_untouched = 0;
            i_Infected -= i_failed;
        }
        if (i_citizens != i_untouched + i_Infected + i_dead + i_Cured) {
            Debug.Log("Something went wrong infecting " + i_people + " people in corona_region_manager of " + gameObject.name + ". Variable \"i_failed\" was " + i_failed + ".");
            return -1;
        }
        return i_failed;
    }

    //let the amount of people die of corona. Returns how many people couldn't die due to a lack of infected people.
    public int DieOfCorona(int i_people) {
        int i_failed = 0;
        i_Infected -= i_people;
        i_dead += i_people;
        if (i_Infected < 0) {
            i_failed = -i_Infected;
            i_Infected = 0;
            i_dead -= i_failed;
            //f_Panic += (i_people - i_failed) * f_PanicPerDeath;
        }
        if (i_citizens != i_untouched + i_Infected + i_dead + i_Cured) {
            Debug.Log("Something went wrong making " + i_people + " people die in corona_region_manager of " + gameObject.name + ". Variable \"i_failed\" was " + i_failed + ".");
            return -1;
        }
        return i_failed;
    }

    //let the amount of people die. Returns how many people couldn't die due to a lack of living people.
    public int Die(int i_Amount) {
        if (i_Amount <= 0) return 0;
        int i_alive = i_citizens - i_dead;
        int i_failed = 0;

        //reduce i_Amount if there aren't enough people alive.
        if (i_Amount > i_alive) {
            i_failed = i_Amount - i_alive;
            i_Amount -= i_failed;
        }

        //calculations to spread kills relatively evenly to all people
        float f_Rand1 = Random.Range(0.22f, 0.44f);
        float f_Rand2 = Random.Range(0.4f, 0.6f);
        int i_KillUntouched = Mathf.RoundToInt(f_Rand1 * i_Amount);
        i_Amount -= i_KillUntouched;
        int i_KillInfected = Mathf.RoundToInt(f_Rand2 * i_Amount);
        i_Amount -= i_KillInfected;
        int i_KillCured = i_Amount;
        if (i_KillUntouched > i_untouched) {
            int i_TooMany = i_KillUntouched - i_untouched;
            i_KillUntouched -= i_TooMany;
            i_KillInfected += i_TooMany;
        }
        if (i_KillInfected > i_Infected) {
            int i_TooMany = i_KillInfected - i_Infected;
            i_KillInfected -= i_TooMany;
            i_KillCured += i_TooMany;
        }
        if (i_KillCured > i_Cured) {
            int i_TooMany = i_KillCured - i_Cured;
            i_KillCured -= i_TooMany;
            i_KillUntouched += i_TooMany;
        }
        //f_Panic += (i_Amount - i_failed) * f_PanicPerDeath;
        return i_failed;
    }

    //immunize the amount of healthy people. Returns how many people it couldn't immunize due to a lack of healthy people.
    public int ImmunizeHealthy(int i_people) {
        int i_failed = 0;
        i_untouched -= i_people;
        i_Cured += i_people;
        if (i_untouched < 0) {
            i_failed = -i_untouched;
            i_untouched = 0;
            i_Cured -= i_failed;
        }
        if (i_citizens != i_untouched + i_Infected + i_dead + i_Cured) {
            Debug.Log("Something went wrong immunizing " + i_people + " healthy people in corona_region_manager of " + gameObject.name + ". Variable \"i_failed\" was " + i_failed + ".");
            return -1;
        }
        return i_failed;
    }

    //de-immunize the amount of cured people. Returns how many people it couldn't de-immunize due to a lack of cured/immune people.
    public int DeImmunize(int i_people) {
        int i_failed = 0;
        i_Cured -= i_people;
        i_untouched += i_people;
        if (i_Cured < 0) {
            i_failed = -i_Cured;
            i_Cured = 0;
            i_untouched -= i_failed;
        }
        if (i_citizens != i_untouched + i_Infected + i_dead + i_Cured) {
            Debug.Log("Something went wrong de-immunizing " + i_people + " cured/immune people in corona_region_manager of " + gameObject.name + ". Variable \"i_failed\" was " + i_failed + ".");
            return -1;
        }
        return i_failed;
    }

    //returns percentage of citizens that are either infected, dead or cured.
    public float GetVirus() {
        return ((float)(i_Infected + i_dead + i_Cured)) / (float)i_citizens;
    }

    //returns percentage of citizens that are either infected or dead.
    public float GetBadState() {
        return ((float)(i_Infected + i_dead)) / (float)i_citizens;
    }

    //returns percentage of citizens that are dead.
    public float GetDead() {
        return i_dead / (float)i_citizens;
    }

    public float GetOverallPanic() {
        return f_Panic + ((float)i_dead / i_citizens);
    }

    public float GetOverallOutputFactor() {
        float f_Alive = (float)(i_citizens - i_dead) / (float)i_citizens;
        return f_OutputFactor * f_Alive;
    }
}