using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Random Event Manger:
    public Event[] RandomEvents;
    public Event evt_StartEvent;
    public Event evt_RiotEvent;
    public Event evt_VirusEradicatedWin;
    public Event evt_RulingCollapsedLose;
    public Event evt_DeathThresholdLose;
    private int randomEvent;
    public float EventMaxWait;
    public float EventMinWait;
    public float EventWait;
    public int startWait;
    public GameObject EventPopUpScreen;
    public GameObject GameOverScreen;

    public GameObject[] Sector;
    public GameObject EconomyText;
    public GameObject PanicBar;
    public GameObject VirusBar;
    public GameObject WeekCounter;
    public GameObject CureBar;

    public float weekLength = 3f;
    private int weekCount;

    public int currentEconomy;
    private float currentPanic;
    private float f_currentVirus;


    private bool infectionStarted; //has the infection started?
    private bool unrest;  //is the virus publically known?
    Random r = new Random();
    private bool gameOver;

    corona_region_manager crm_SelectedRegion;
    public ScienceScript ss_Science;

    public corona_region_manager GetSelectedRegion() {
        return crm_SelectedRegion;
    }

    public void SetSelectedRegion(corona_region_manager crm_Select) {
        foreach (GameObject go_MySector in Sector) {
            //go_MySector.GetComponent<Corona_Map_Region_Selection>().b_Clicked = false;
            go_MySector.GetComponent<Corona_Map_Region_Selection>().DeClick();
        }
        crm_SelectedRegion = crm_Select;
        crm_Select.GetComponent<Corona_Map_Region_Selection>().b_Clicked = true;
        //Debug.Log("Selected Sector " + crm_Select.gameObject.name + ".");
    }

    public void SetCurrentEconomy(int i_newEconomy){
        //TODO change this to absolute
        currentEconomy = i_newEconomy;
        EconomyText.transform.Find("Text").GetComponent<Text>().text = currentEconomy.ToString();
        if (currentEconomy >= 1000000) EconomyText.transform.Find("Text").GetComponent<Text>().text = ((int)((float)currentEconomy / 1000000f)).ToString() + "m";
        else if (currentEconomy >= 1000) EconomyText.transform.Find("Text").GetComponent<Text>().text = ((int)((float)currentEconomy / 1000f)).ToString() + "k";
        
    }

    public int GetCurrentEconomy(){
        return currentEconomy;
    }

    public void SetCurrentPanic(float f_newPanic){
        currentPanic = f_newPanic;
        PanicBar.transform.Find("Fill").GetComponent<Image>().fillAmount = currentPanic;
    }

    public float GetCurrentPanic(){
        return currentPanic;
    }

    public void SetCurrentVirus(float f_newVirus){
        f_currentVirus = f_newVirus;
        VirusBar.transform.Find("Fill").GetComponent<Image>().fillAmount = f_currentVirus;
    }

    public float GetCurrentVirus(){
        return f_currentVirus;
    }

    //returns percentage of dead people from all citizens.
    public float GetCurrentDeath() {
        float f_DeathPercentage = 0;
        foreach (GameObject go_CurrentSector in Sector) {
            f_DeathPercentage += go_CurrentSector.GetComponent<corona_region_manager>().GetDead();
        }
        return f_DeathPercentage;
    }

    public void SetCurrentCure(float f_NewCureProgress) {
        ss_Science.SetCureProgress(f_NewCureProgress);
        CureBar.transform.Find("Fill").GetComponent<Image>().fillAmount = GetCurrentCure();
    }
    public float GetCurrentCure() {
        return ss_Science.GetCureProgress();
    }

    void Start(){
        //set values to a base for start
        //currentEconomy = 100000;
        currentPanic = 0;
        f_currentVirus = 0;
        infectionStarted = false;
        unrest = false;
        weekCount = 0;
        InvokeRepeating("gameUpdate", 1f, weekLength); //updates game state every x seconds where x is a week (with howevermany seconds we set for it)
        r = null;
        Sector = GameObject.FindGameObjectsWithTag("Sector");
        gameOver = false;
        StartCoroutine(RandomEventSpawner());
        EventPopUpScreen.SetActive(false);
    }

    private void gameUpdate(){
        weekCount++;
        WeekCounter.GetComponentInChildren<UnityEngine.UI.Text>().text = "Week: " + weekCount.ToString();
        float f_PVirus = 0;
        float f_PPanic = 0;
        foreach (GameObject go_Region in Sector){
            corona_region_manager crm_RegionManager;
            crm_RegionManager = go_Region.GetComponent<corona_region_manager>();
            crm_RegionManager.GameUpdate();
            f_PVirus += crm_RegionManager.GetVirus();
            f_PPanic += crm_RegionManager.GetOverallPanic();
        }
        if (weekCount == 3) {
            CallEvent(evt_StartEvent);
            Time.timeScale = 0;
        }
        ss_Science.CureUpdate();

        SetCurrentVirus(f_PVirus / Sector.Length);
        SetCurrentPanic(f_PPanic / Sector.Length);
        //below call is NOT redundant! Will update the progress bar when Cure progress is changed in events or policies.
        SetCurrentCure(GetCurrentCure());

        if (CureBar.transform.Find("Fill").GetComponent<Image>().fillAmount >= 1) {
            CallEvent(evt_VirusEradicatedWin);
            Time.timeScale = 0;
        } else if (Sector[1].GetComponent<corona_region_manager>().GetBadState() > 0.7) {
            CallEvent(evt_RulingCollapsedLose);
            Time.timeScale = 0;
        } else if (GetCurrentDeath() > 0.6) {
            CallEvent(evt_DeathThresholdLose);
            Time.timeScale = 0;
        }
    }

    IEnumerator RandomEventSpawner(){
        yield return new WaitForSeconds(Random.Range(EventMinWait, EventMaxWait));

        while (true){
            randomEvent = Random.Range(0, RandomEvents.Length);
            /*EventPopUpScreen.SetActive(true);
            Instantiate(RandomEvents[randomEvent]);*/
            CallEvent(RandomEvents[randomEvent]);
            Time.timeScale = 0;
            yield return new WaitForSeconds(EventWait);
        }
    }

    public void CallEvent(Event evt_NewEvent) {
        //EventPopUpScreen.SetActive(true);
        //GameOverScreen.SetActive(true);
        //Event evt = Instantiate(evt_NewEvent, EventPopUpScreen.transform);
        if (evt_NewEvent.i_EventType == 2) {
            GameOverScreen.SetActive(true);
            Instantiate(evt_NewEvent, GameOverScreen.transform);
        } else {
            EventPopUpScreen.SetActive(true);
            Instantiate(evt_NewEvent, EventPopUpScreen.transform);
        }
    }
}