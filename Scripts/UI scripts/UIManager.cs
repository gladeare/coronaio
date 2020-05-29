using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{
    public GameObject go_PolicyManagement;
    public Text t_PolicyRegionName; 

    GameObject go_VirusPanel;
    GameObject go_EconomyPanel;
    GameObject go_PanicPanel;
    public GameObject go_SectorPanel_Factory;
    public GameObject go_SectorPanel_Entertainment;
    public GameObject go_SectorPanel_Ruling;
    public GameObject go_SectorPanel_Science;
    public GameObject go_SectorPanel_Mining;
    public GameObject go_SectorPanel_Urban;
    public GameObject go_SectorPanel_Agriculture;

    SectorPanelScript sps_ActiveSectorPanel;

    void CloseAllSectorPanels() {
        go_SectorPanel_Factory.SetActive(false);
        go_SectorPanel_Entertainment.SetActive(false);
        go_SectorPanel_Ruling.SetActive(false);
        go_SectorPanel_Science.SetActive(false);
        go_SectorPanel_Mining.SetActive(false);
        go_SectorPanel_Urban.SetActive(false);
        go_SectorPanel_Agriculture.SetActive(false);
    }

    void CloseAllTypePanels() {
        go_VirusPanel.SetActive(false);
        go_EconomyPanel.SetActive(false);
        go_PanicPanel.SetActive(false);
    }

    public void OpenPolicyManagement() {
        GameManager gm_GameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();
        if (!gm_GameManager.GetSelectedRegion()) return;
        
        Time.timeScale = 0;
        go_PolicyManagement.SetActive(true);
        t_PolicyRegionName.text = gm_GameManager.GetSelectedRegion().s_name;
        CloseAllSectorPanels();

        switch (t_PolicyRegionName.text) {
            case "Factory Sector":
                go_SectorPanel_Factory.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Factory.GetComponent<SectorPanelScript>();
                break;
            case "Entertainment Sector":
                go_SectorPanel_Entertainment.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Entertainment.GetComponent<SectorPanelScript>();
                break;
            case "Ruling Sector":
                go_SectorPanel_Ruling.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Ruling.GetComponent<SectorPanelScript>();
                break;
            case "Science Sector":
                go_SectorPanel_Science.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Science.GetComponent<SectorPanelScript>();
                break;
            case "Mining Sector":
                go_SectorPanel_Mining.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Mining.GetComponent<SectorPanelScript>();
                break;
            case "Urban Sector":
                go_SectorPanel_Urban.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Urban.GetComponent<SectorPanelScript>();
                break;
            case "Agriculture Sector":
                go_SectorPanel_Agriculture.SetActive(true);
                sps_ActiveSectorPanel = go_SectorPanel_Agriculture.GetComponent<SectorPanelScript>();
                break;
            default:
                Debug.Log("Something went wrong in UIManager, region \"" + t_PolicyRegionName.text + "\"not found");
                break;
        }

        go_VirusPanel = sps_ActiveSectorPanel.go_VirusPanel;
        go_EconomyPanel = sps_ActiveSectorPanel.go_EconomyPanel;
        go_PanicPanel = sps_ActiveSectorPanel.go_PanicPanel;
        CloseAllTypePanels();
    }

    public void ClosePolicyManagement() {
        go_PolicyManagement.SetActive(false);
        Time.timeScale = 1;
    }

    public void OpenVirusPanel() {
        CloseAllTypePanels();
        go_VirusPanel.SetActive(true);
    }

    public void OpenEconomyPanel() {
        CloseAllTypePanels();
        go_EconomyPanel.SetActive(true);
    }

    public void OpenPanicPanel() {
        CloseAllTypePanels();
        go_PanicPanel.SetActive(true);
    }
}