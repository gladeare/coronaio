using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Corona_Map_Region_Selection : MonoBehaviour
{
    public bool b_ChangeMat;
    public Material mat_Default;
    public Material mat_MouseOver;
    public float f_RegionGrowth;
    public float f_LiftTime;
    public PieChartScript pcs_PieChart;
    public Hover_Bar_Script hbs_HoverBar;

    int i_Grow = 0; // 0 == stay, 1 == grow, 2 == shrink
    Vector3 v_RegionGrowth;
    Transform t_MeshTransform;
    GameObject go_HoverCanvas;
    bool b_Selected;
    MeshRenderer mr_MeshRenderer;
    public bool b_Clicked;

    // Start is called before the first frame update
    void Start() {
        t_MeshTransform = transform.GetChild(0);
        go_HoverCanvas = transform.Find("Canvas_HoverInfo").gameObject;
        mr_MeshRenderer = transform.Find("default").GetComponent<MeshRenderer>();
        mr_MeshRenderer.material = new Material(mr_MeshRenderer.material);
    }

    // Update is called once per frame
    void Update() {
        //checks whether region has to lift or settle
        if (i_Grow != 0) {
            float f_TickGrowth = Time.deltaTime / f_LiftTime * f_RegionGrowth;
            if (i_Grow == 1) {  //when lifting
                if (t_MeshTransform.localPosition.y + f_TickGrowth >= f_RegionGrowth) {
                    t_MeshTransform.localPosition = new Vector3(0, f_RegionGrowth, 0);
                } else {
                    t_MeshTransform.localPosition += new Vector3(0, f_TickGrowth, 0);
                }
            } else {            //when settling
                if (t_MeshTransform.localPosition.y - f_TickGrowth <= 0) {
                    t_MeshTransform.localPosition = Vector3.zero;
                } else {
                    t_MeshTransform.localPosition -= new Vector3(0, f_TickGrowth, 0);
                }
            }
        }
        if(b_Clicked) mr_MeshRenderer.material.SetFloat(mr_MeshRenderer.material.shader.GetPropertyNameId(2), (Mathf.Sin((Time.timeSinceLevelLoad % 1f) * 360 * Mathf.PI / 180) + 1f) / 3 + 0.2f);
    }

    private void OnMouseExit() {
        if(b_Selected)
        DeactivateHover();
    }

    private void OnMouseOver() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            if (b_Selected) DeactivateHover();
        } else if (!b_Selected) ActivateHover();
        else {
            pcs_PieChart.GameUpdate();
            hbs_HoverBar.GameUpdate();
            //mr_MeshRenderer.material.SetFloat(mr_MeshRenderer.material.shader.GetPropertyNameId(2), (Mathf.Sin((Time.timeSinceLevelLoad % 1f) * 360 * Mathf.PI / 180) + 1f) / 4 + 0.5f);
        }
    }

    private void ActivateHover() {
        if (t_MeshTransform.localPosition.y < f_RegionGrowth) i_Grow = 1;
        else i_Grow = 0;
        if (b_ChangeMat) t_MeshTransform.GetComponent<MeshRenderer>().material = mat_MouseOver;
        go_HoverCanvas.SetActive(true);
        b_Selected = true;
        pcs_PieChart.GameUpdate();
        hbs_HoverBar.GameUpdate();
        //mr_MeshRenderer.material.SetFloat(mr_MeshRenderer.material.shader.GetPropertyNameId(2), (Mathf.Sin((Time.timeSinceLevelLoad % 1f) * 360 * Mathf.PI / 180) + 1f) / 4 + 0.5f);
    }

    private void DeactivateHover() {
        if (t_MeshTransform.localPosition.y > 0) i_Grow = 2;
        else i_Grow = 0;
        if (b_ChangeMat) t_MeshTransform.GetComponent<MeshRenderer>().material = mat_Default;
        go_HoverCanvas.SetActive(false);
        b_Selected = false;
        //mr_MeshRenderer.material.SetFloat(mr_MeshRenderer.material.shader.GetPropertyNameId(2), 0.386f);
    }

    private void OnMouseUpAsButton() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //Debug.Log("Trying to select sector " + gameObject.name + ".");
        GameObject.Find("EventSystem").GetComponent<GameManager>().SetSelectedRegion(GetComponent<corona_region_manager>());
    }

    public void DeClick() {
        mr_MeshRenderer.material.SetFloat(mr_MeshRenderer.material.shader.GetPropertyNameId(2), 0.386f);
        b_Clicked = false;
    }
}