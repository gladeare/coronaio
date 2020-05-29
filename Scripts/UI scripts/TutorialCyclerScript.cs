using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCyclerScript : MonoBehaviour{

    int i = 0;

    public GameObject[] go_Templates;

    public void Next() {
        go_Templates[i].SetActive(false);
        i++;
        if (i >= go_Templates.Length) i = 0;
        go_Templates[i].SetActive(true);
    }
}
