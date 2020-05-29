using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover_Bar_Script : MonoBehaviour{

    corona_region_manager crm_RegionManager;
    public Image img_PanicBarFill;
    public Image img_EconomyBarFill;


    public void GameUpdate() {
        if (!crm_RegionManager) crm_RegionManager = transform.parent.GetComponent<corona_region_manager>();
        img_PanicBarFill.fillAmount = crm_RegionManager.GetOverallPanic();
        img_EconomyBarFill.GetComponentInChildren<Image>().fillAmount = crm_RegionManager.GetOverallOutputFactor();
    }
}
