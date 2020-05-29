using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScienceScript : MonoBehaviour{

    public float f_MinFunding;
    public float f_CurrentFunding;
    //How many percent of cure progress to add for every cureupdate with f_MinFunding
    public float f_CureFactor;
    //amount by which funding can be increased/decreased
    public float f_FundingStepAmount;

    public GameObject CureBar;
    public float f_CureProgress = 0;
    GameManager gm_GameManager;

    private void Start() {
        gm_GameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();
        f_CurrentFunding = f_MinFunding; //TODO move this to start event (5th day)
        CureBar.GetComponentInChildren<Text>().text = "100k$/week";
    }

    public void CureUpdate() {
        int i_Economy = gm_GameManager.GetCurrentEconomy();
        if (f_CurrentFunding >= f_MinFunding && i_Economy >= f_CurrentFunding) {
            float f_ProgressStep = (2f - Mathf.Pow(0.5f, f_CurrentFunding / f_MinFunding) * 2f) * f_CureFactor;
            f_CureProgress += f_ProgressStep;
            //Debug.Log("Increasing Progress by " + f_ProgressStep * 100f + "% with a funding value of " + f_CurrentFunding + "$. x = " + (f_CurrentFunding / f_MinFunding).ToString() + ".");
            gm_GameManager.SetCurrentEconomy(i_Economy - (int)f_CurrentFunding);
            gm_GameManager.SetCurrentCure(f_CureProgress);
        }         
    }

    public float GetCureProgress() {
        return f_CureProgress;
    }
    public void SetCureProgress(float f_NewCureProgress) {
        f_CureProgress = f_NewCureProgress;
    }

    //increase funding if increase = true, otherwise decrease
    public void IncreaseFunding(bool increase) {
        if (increase) {
            Debug.Log("increase funding");
            f_CurrentFunding += f_FundingStepAmount;
        } else if (f_CurrentFunding >= f_MinFunding + f_FundingStepAmount) {
            Debug.Log("decrease funding");
            f_CurrentFunding -= f_FundingStepAmount;
        }
        CureBar.GetComponentInChildren<Text>().text = (f_CurrentFunding / 1000).ToString() + "k$/week";
    }
}