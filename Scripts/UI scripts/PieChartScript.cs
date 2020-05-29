using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChartScript : MonoBehaviour{

    public Image img_Untouched;
    public Image img_Infected;
    public Image img_Dead;
    corona_region_manager crm_RegionManager;

    public void GameUpdate() {
        if(!crm_RegionManager) crm_RegionManager = transform.parent.parent.GetComponent<corona_region_manager>();
        //Debug.Log("Called GameUpdate in PieChartScript");
        float f_PUntouched;
        float f_PInfected;
        float f_PDead;

        f_PUntouched = ((float)crm_RegionManager.i_untouched) / ((float)crm_RegionManager.i_citizens);
        f_PInfected = ((float)crm_RegionManager.i_Infected) / ((float)crm_RegionManager.i_citizens);
        f_PDead = ((float)crm_RegionManager.i_dead) / ((float)crm_RegionManager.i_citizens);

        img_Untouched.fillAmount = f_PUntouched;
        img_Infected.fillAmount = f_PUntouched + f_PInfected;
        img_Dead.fillAmount = f_PUntouched + f_PInfected + f_PDead;

    }
}
