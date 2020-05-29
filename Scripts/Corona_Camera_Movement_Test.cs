using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corona_Camera_Movement_Test : MonoBehaviour{

    public GameObject go_MyPlayer;   
    public float f_VertRotSpeed;
    public float f_HorRotSpeed;
    public float f_ScrollSpeed;

    Vector3 v3_LastMousePos;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        Scrolling();
        Dragging();
    }

    // Handles camera zoom when scrolling
    private void Scrolling() {
        float f_ScrollData = Input.mouseScrollDelta.y * f_ScrollSpeed;
        Transform tr_CamTransform = go_MyPlayer.transform.GetChild(0);
        float f_NewCamDist = tr_CamTransform.localPosition.z + f_ScrollData;
        if (f_ScrollData != 0 && f_NewCamDist <= -4 && f_NewCamDist >= -20) {
            tr_CamTransform.localPosition += new Vector3(0, 0, f_ScrollData);
        }
    }

    // Handles camera moving around planet while dragging
    private void Dragging() {
        //when mouse0 is down
        if (!Input.GetKey(KeyCode.Mouse0)) return;

        //convert mouse movement into time-, resolution- and zoom- fixed rotation.
        Vector3 v3_NewMousePos = Input.mousePosition;
        Vector3 v3_Rotation = v3_NewMousePos - v3_LastMousePos;
        float f_CamDist = go_MyPlayer.transform.GetChild(0).localPosition.z;
        v3_Rotation = new Vector3(-v3_Rotation.y, v3_Rotation.x, 0);

        //apply speed
        v3_Rotation = new Vector3(v3_Rotation.x * f_VertRotSpeed, v3_Rotation.y * f_HorRotSpeed, 0);
        //fix delta time
        v3_Rotation *= Time.deltaTime;
        //fix resoltion (to screen height only, it defines base resolution)
        v3_Rotation = new Vector3(v3_Rotation.x  * 1000 / Screen.height, v3_Rotation.y * 1000 / Screen.height, 0);
        //fix zoom
        v3_Rotation *= -f_CamDist;

        //DEBUG: Debug.Log("ROTATION: (x:" + v3_Rotation.x + "|y:" + v3_Rotation.y + ")");
        go_MyPlayer.transform.Rotate(v3_Rotation.x, v3_Rotation.y, 0);
    }

    private void LateUpdate() {
        v3_LastMousePos = Input.mousePosition;
    }
}
