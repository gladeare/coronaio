using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corona_Sphere_Region_Selection : MonoBehaviour
{
    public Material mat_Default;
    public Material mat_MouseOver;
    public float f_RegionGrowth;
    public float f_ScaleTime;

    int i_Grow = 0; // 0 == stay, 1 == grow, 2 == shrink
    Vector3 v_RegionGrowth;
    Transform t_MeshTransform;

    // Start is called before the first frame update
    void Start(){
        v_RegionGrowth = new Vector3(f_RegionGrowth, f_RegionGrowth, f_RegionGrowth);
        t_MeshTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update(){
        //checks whether region has to grow or shrink
        if (i_Grow != 0) {
            float f_TickGrowth = Time.deltaTime / f_ScaleTime * f_RegionGrowth;
            if (i_Grow == 1) {  //when growing
                if (t_MeshTransform.localScale.x + f_TickGrowth >= 1 + f_RegionGrowth) {
                    t_MeshTransform.localScale = Vector3.one + v_RegionGrowth;
                } else {
                    t_MeshTransform.localScale += new Vector3(f_TickGrowth, f_TickGrowth, f_TickGrowth);
                }
            } else {            //when shrinking
                if (t_MeshTransform.localScale.x - f_TickGrowth <= 1) {
                    t_MeshTransform.localScale = Vector3.one;
                } else {
                    t_MeshTransform.localScale -= new Vector3(f_TickGrowth, f_TickGrowth, f_TickGrowth);
                }
            }
        }
    }

    private void OnMouseEnter() {
        if (t_MeshTransform.localScale.x < 1 + f_RegionGrowth) i_Grow = 1;
        else i_Grow = 0;

        t_MeshTransform.GetComponent<MeshRenderer>().material = mat_MouseOver;
        //transform.localScale += v_RegionGrowth;
    }

    private void OnMouseExit() {
        if (t_MeshTransform.localScale.x > 1) i_Grow = 2;
        else i_Grow = 0;

        t_MeshTransform.GetComponent<MeshRenderer>().material = mat_Default;
        //transform.localScale = Vector3.one;
    }
}
