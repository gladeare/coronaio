using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun_rotator_script : MonoBehaviour{
    public float f_speed;

    // Update is called once per frame
    void Update(){
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, f_speed * Time.deltaTime, 0));
    }
}
