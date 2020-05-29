using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover_canvas_turn_script : MonoBehaviour{

    public bool pitch;

    Transform t_Camera;

    private void Start() {
        t_Camera = GetComponent<Canvas>().worldCamera.transform;
    }

    void Update(){
        transform.LookAt(t_Camera);
        if (!pitch) transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}