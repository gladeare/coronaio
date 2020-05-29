using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    [Header("DYNAMIC VARIABLES")]
    public int i_StartEventImpact;
    public int i_EventType;
    public float EconomyImpact;
    public int i_EconomyFlatImpact;
    public float VirusImpact;
    public float PanicImpact;
    public float f_ResearchProgress;
    public int i_KillAmount;
    public string s_name;
    public Sprite Event_Popup_Sprite;
    [TextArea]
    public string s_Information;
    public string s_ButtonText;
    [Space(20)]
    [Header("EventSprite")]
    

    EventInteract evtInt_PopupScreen;
    GameManager gm_GameManager;

    UI_ClickSoundScript scr_ClickSoundScript;


    private void Start(){
        gm_GameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();
        //scr_ClickSoundScript = GameObject.Find("Audio Source").GetComponent<UI_ClickSoundScript>();

        evtInt_PopupScreen = transform.parent.GetComponent<EventInteract>();
        evtInt_PopupScreen.go_Name.GetComponent<Text>().text = s_name;
        evtInt_PopupScreen.go_description.GetComponent<Text>().text = s_Information;
        evtInt_PopupScreen.go_Image.GetComponent<Image>().sprite = Event_Popup_Sprite;

        switch (i_EventType) {
            case 0: { //Start Event, infect people in random sector. Isolated.
                    evtInt_PopupScreen.go_Button.GetComponentInChildren<Text>().text = s_ButtonText;
                    int i_rand;
                    i_rand = Random.Range(0, gm_GameManager.Sector.Length - 2);
                    if (i_rand > 1) i_rand += 2;
                    gm_GameManager.Sector[i_rand].GetComponent<corona_region_manager>().Infect(i_StartEventImpact);
                    Debug.Log("Start event,  infected " + i_StartEventImpact + " people in sector " + i_rand + ".");
                    break;
                }
            case 1: //Single Random Sector Event Flat Values
                evtInt_PopupScreen.go_Button.GetComponentInChildren<Text>().text = s_ButtonText;
                corona_region_manager crm_RandomRegionManager = gm_GameManager.Sector[Random.Range(0, gm_GameManager.Sector.Length)].GetComponent<corona_region_manager>();
                crm_RandomRegionManager.Die(i_KillAmount);
                crm_RandomRegionManager.f_Panic += PanicImpact;
                crm_RandomRegionManager.f_OutputFactor += EconomyImpact;
                crm_RandomRegionManager.f_FactorSpread += VirusImpact;
                gm_GameManager.SetCurrentEconomy(gm_GameManager.GetCurrentEconomy() + i_EconomyFlatImpact);
                break;
            case 2: //Finisher Event, Game Over (good or bad, win or lose)
                //nothing, just instantiating the prefab is enough.
                break;
            case 3: { //Global Event. Isolated.
                    foreach (GameObject CurrentSector in gm_GameManager.Sector) {
                        corona_region_manager crm_RegionManager = CurrentSector.GetComponent<corona_region_manager>();
                        crm_RegionManager.Die(i_KillAmount);
                        crm_RegionManager.f_Panic += PanicImpact;
                        crm_RegionManager.f_OutputFactor += EconomyImpact;
                        crm_RegionManager.f_FactorSpread += VirusImpact;
                    }
                    gm_GameManager.SetCurrentEconomy(gm_GameManager.GetCurrentEconomy() + i_EconomyFlatImpact);
                    break;
                }
            default:
                Debug.Log("Unknown event type called in Event.cs");
                break;
        }
        if (i_EconomyFlatImpact != 0) {
            if (gm_GameManager.currentEconomy >= i_EconomyFlatImpact) {
                gm_GameManager.currentEconomy -= i_EconomyFlatImpact;
            } else {
                //gm_GameManager.CallEvent(); //TODO economic defeat event
                Debug.Log("CALL ECONOMIC DEFEAT IF IMPLEMENTED!!!");
            }
        }
        if (f_ResearchProgress >= 0) {
            gm_GameManager.SetCurrentCure(gm_GameManager.GetCurrentCure() + f_ResearchProgress);
        }
       
    }

    //unused
    public void ExecuteEvent() {
        //Debug.Log("Event Executed!");
    }
}