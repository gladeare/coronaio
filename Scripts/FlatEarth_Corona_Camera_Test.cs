using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatEarth_Corona_Camera_Test : MonoBehaviour
{
    public float f_VertMoveSpeed;
    public float f_HorMoveSpeed;
    public float f_ScrollSpeed;
    public float f_ScrollMaxDist;
    public float f_ScrollMinDist;
    public Vector4 v4_BoundrariesUpDownLeftRight;

    Vector3 v3_NewMousePos;
    Vector3 v3_LastMousePos;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        //fix for teleportation when losing focus
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            v3_NewMousePos = Input.mousePosition;
            v3_LastMousePos = v3_NewMousePos;
        }

        //scroll and drag the camera
        Scrolling();
        Dragging();
    }

    // Handles camera zoom when scrolling
    private void Scrolling() {
        float f_ScrollData = Input.mouseScrollDelta.y * f_ScrollSpeed;
        float f_NewCamDist = transform.position.y - f_ScrollData;
        if (f_ScrollData != 0 && f_NewCamDist >= f_ScrollMinDist && f_NewCamDist <= f_ScrollMaxDist) {
            transform.position -= new Vector3(0, f_ScrollData, 0);
        }
    }

    // Handles camera moving around planet while dragging
    private void Dragging() {
        //when mouse0 is down
        if (!Input.GetKey(KeyCode.Mouse0)) return;

        v3_NewMousePos = Input.mousePosition;
        //convert mouse movement into time-, resolution- and zoom- fixed rotation.
        Vector3 v3_Movement = v3_NewMousePos - v3_LastMousePos;
        //Debug.Log("MOUSE: (x:" + v3_Movement.x + "|y:" + v3_Movement.y + "|z:" + v3_Movement.z + ")");
        v3_Movement = new Vector3(v3_Movement.y, 0, -v3_Movement.x);
        //Debug.Log("MOVEMENT: (x:" + v3_Movement.x + "|z:" + v3_Movement.z + ")");

        //apply speed
        v3_Movement = new Vector3(v3_Movement.x * f_VertMoveSpeed, 0, v3_Movement.z * f_HorMoveSpeed);
        //fix delta time
        v3_Movement *= Time.deltaTime;
        //fix resoltion (to screen height only, it defines base resolution)
        v3_Movement = new Vector3(v3_Movement.x * 1000 / Screen.height, 0, v3_Movement.z * 1000 / Screen.height);
        //fix zoom
        v3_Movement *= transform.position.y;

        //Debug.Log("MOVEMENT: (x:" + v3_Movement.x + "|z:" + v3_Movement.z + ")");

        transform.position += v3_Movement;

        if (transform.position.x < v4_BoundrariesUpDownLeftRight.x) transform.position = new Vector3(v4_BoundrariesUpDownLeftRight.x, transform.position.y, transform.position.z);  //UP
        if (transform.position.x > v4_BoundrariesUpDownLeftRight.y) transform.position = new Vector3(v4_BoundrariesUpDownLeftRight.y, transform.position.y, transform.position.z);  //DOWN
        if (transform.position.z < v4_BoundrariesUpDownLeftRight.z) transform.position = new Vector3(transform.position.x, transform.position.y, v4_BoundrariesUpDownLeftRight.z);  //LEFT
        if (transform.position.z > v4_BoundrariesUpDownLeftRight.w) transform.position = new Vector3(transform.position.x, transform.position.y, v4_BoundrariesUpDownLeftRight.w);  //RIGHT       
    }

    private void LateUpdate() {
        v3_LastMousePos = Input.mousePosition;
    }
}
