using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolicyScript : MonoBehaviour{
    //public
    [Header("DYNAMIC VARIABLES")]
    [Header("Prerequisites")]
    public float f_MinVirus;
    public float f_MinPanic;
    [Header("Effects")]
    public bool b_CanDeactivate;
    public float f_EconomyDelta;
    public float f_VirusDelta;
    public float f_MortalityDelta;
    public float f_CuringDelta;
    public float f_PanicDelta;
    public int i_UniqueCost;
    public int i_WeeklyCost;
    public string s_Headline;
    [TextArea]
    public string s_Information;
    
    [Space(20)]
    [Header("Colors")]
    [Header("PERMANENT VARIABLES")]
    public Color col_NegativeMain;
    public Color col_NegativeBackground;
    public Color col_PositiveMain;
    public Color col_PositiveBackground;
    [Space(10)]
    [Header("Textures")]
    public Sprite spr_Arrow1;
    public Sprite spr_Arrow2;
    public Sprite spr_Arrow3;
    [Space(10)]
    [Header("Thresholds")]
    public float f_EconomyMedium;
    public float f_EconomyLarge;
    public float f_VirusMedium;
    public float f_VirusLarge;
    public float f_PanicMedium;
    public float f_PanicLarge;
    [Space(10)]
    [Header("Game Objects")]
    public GameObject go_EconomyArrow;
    public GameObject go_VirusArrow;
    public GameObject go_PanicArrow;
    public GameObject go_EconomyIcon;
    public GameObject go_VirusIcon;
    public GameObject go_PanicIcon;

    //private
    private Text t_InformationText;
    private Text t_HeadlineText;
    UI_ClickSoundScript scr_ClickSoundScript;
    Toggle tog_Toggle;

    private void Start() {
        t_InformationText = transform.parent.parent.parent.Find("Information Panel").Find("Description").GetComponentInChildren<Text>();
        t_HeadlineText = transform.parent.parent.parent.Find("Information Panel").Find("Headline").GetComponentInChildren<Text>();
        scr_ClickSoundScript = GameObject.Find("Audio Source").GetComponent<UI_ClickSoundScript>();
    }

    private void OnEnable() {
        tog_Toggle = GetComponent<Toggle>();
        //Debug.Log("Starting Policy: Toggle found? " + tog_Toggle + ".");

        //Economy
        if (f_EconomyDelta != 0) {
            go_EconomyArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            go_EconomyIcon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (Mathf.Abs(f_EconomyDelta) >= f_EconomyMedium) {
                if (Mathf.Abs(f_EconomyDelta) >= f_EconomyLarge) go_EconomyArrow.GetComponent<Image>().sprite = (Sprite)spr_Arrow3;
                else go_EconomyArrow.GetComponent<Image>().sprite = spr_Arrow2;
            } else go_EconomyArrow.GetComponent<Image>().sprite = spr_Arrow1;
            if (f_EconomyDelta > 0) {
                go_EconomyArrow.GetComponent<Image>().color = col_PositiveBackground;
                go_EconomyIcon.GetComponent<Image>().color = col_PositiveMain;
            } else {
                go_EconomyArrow.GetComponent<Image>().color = col_NegativeBackground;
                go_EconomyArrow.transform.Rotate(180, 0, 0);
                go_EconomyIcon.GetComponent<Image>().color = col_NegativeMain;
                go_EconomyIcon.transform.Rotate(180, 0, 0);
            }
        }
        //Virus
        if (f_VirusDelta != 0) {
            go_VirusArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            go_VirusIcon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (Mathf.Abs(f_VirusDelta) >= f_VirusMedium) {
                if (Mathf.Abs(f_VirusDelta) >= f_VirusLarge) go_VirusArrow.GetComponent<Image>().sprite = (Sprite)spr_Arrow3;
                else go_VirusArrow.GetComponent<Image>().sprite = spr_Arrow2;
            } else go_VirusArrow.GetComponent<Image>().sprite = spr_Arrow1;
            if (f_VirusDelta > 0) {
                go_VirusArrow.GetComponent<Image>().color = col_NegativeBackground;
                go_VirusIcon.GetComponent<Image>().color = col_NegativeMain;
            } else {
                go_VirusArrow.GetComponent<Image>().color = col_PositiveBackground;
                go_VirusArrow.transform.Rotate(180, 0, 0);
                go_VirusIcon.GetComponent<Image>().color = col_PositiveMain;
                go_VirusIcon.transform.Rotate(180, 0, 0);
            }
        }
        //Panic
        if (f_PanicDelta != 0) {
            go_PanicArrow.transform.localRotation = Quaternion.Euler(0, 0, 0);
            go_PanicIcon.transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (Mathf.Abs(f_PanicDelta) >= f_PanicMedium) {
                if (Mathf.Abs(f_PanicDelta) >= f_PanicLarge) go_PanicArrow.GetComponent<Image>().sprite = (Sprite)spr_Arrow3;
                else go_PanicArrow.GetComponent<Image>().sprite = spr_Arrow2;
            } else go_PanicArrow.GetComponent<Image>().sprite = spr_Arrow1;
            if (f_PanicDelta > 0) {
                go_PanicArrow.GetComponent<Image>().color = col_NegativeBackground;
                go_PanicIcon.GetComponent<Image>().color = col_NegativeMain;
            } else {
                go_PanicArrow.GetComponent<Image>().color = col_PositiveBackground;
                go_PanicArrow.transform.Rotate(180, 0, 0);
                go_PanicIcon.GetComponent<Image>().color = col_PositiveMain;
                go_PanicIcon.transform.Rotate(180, 0, 0);
            }
        }
    }

    public void MouseEnter() {
        t_InformationText.text = s_Information;
        t_HeadlineText.text = s_Headline;
        //Debug.Log("Hovering over Policy");
    }

    public void MouseExit() {
        t_InformationText.text = "Hover over a policy to see a more detailed description.";
        t_HeadlineText.text = "Policy Info";
        //Debug.Log("No longer hovering over Policy");
    }

    public void PlayClick() {
        corona_region_manager crm_RegionManager = GameObject.Find("EventSystem").GetComponent<GameManager>().GetSelectedRegion();
        if (crm_RegionManager.b_Riot || crm_RegionManager.GetVirus() < f_MinVirus || crm_RegionManager.f_Panic < f_MinPanic) {
            Debug.Log("Can't activate Policy");
            tog_Toggle.isOn = false;
            return;
        }

        if (tog_Toggle.isOn) {  //turn policy on
            Debug.Log("Activating Policy");
            if (!b_CanDeactivate) tog_Toggle.interactable = false;

            //TODO scr_ClickSoundScript.PlayClick();

            crm_RegionManager.f_FactorSpread += f_VirusDelta;
            crm_RegionManager.f_OutputFactor += f_EconomyDelta;
            crm_RegionManager.f_FactorDeath += f_MortalityDelta;
            crm_RegionManager.f_PanicFactor += f_PanicDelta;
            Debug.Log("Activated Policy " + gameObject.name + " on Sector " + crm_RegionManager.gameObject.name + ".");

        } else {    //turn policy off

            //TODO scr_ClickSoundScript.PlayClick();

            crm_RegionManager.f_FactorSpread -= f_VirusDelta;
            crm_RegionManager.f_OutputFactor -= f_EconomyDelta;
            crm_RegionManager.f_FactorDeath -= f_MortalityDelta;
            crm_RegionManager.f_PanicFactor -= f_PanicDelta;
        }
    }
}
