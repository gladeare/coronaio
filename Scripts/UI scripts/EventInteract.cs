using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteract : MonoBehaviour{
    public GameObject go_Name;
    public GameObject go_description;
    public GameObject go_Image;
    public GameObject go_Button;

    public void OnClick() {
        transform.GetComponentInChildren<Event>().ExecuteEvent();
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
}
