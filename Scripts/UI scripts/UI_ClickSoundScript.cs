using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ClickSoundScript : MonoBehaviour{

    public AudioClip[] ac_Klicks;

    AudioSource as_AudioSource;

    // Start is called before the first frame update
    void Start(){
        as_AudioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void PlayClick() {
        as_AudioSource.PlayOneShot(ac_Klicks[Random.Range(0, ac_Klicks.Length)]);
    }
}
