using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleInteract : MonoBehaviour{
    public AudioClip ac_BubblePopSound;
    public int i_BubbleType; // 0 == cure, 1 == research, 2 == spread
    public int i_CureAmount;
    public float f_SpreadDelta;
    public float f_ResearchFactor;
    bool b_HasBeenClicked = false;

    AudioSource as_AudioSource;
    GameManager gm_GameManager;

    private void Start() {
        gm_GameManager = GameObject.Find("EventSystem").GetComponent<GameManager>();
    }


    private void OnMouseDown() {
        if (b_HasBeenClicked) return;
        as_AudioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        as_AudioSource.PlayOneShot(ac_BubblePopSound);

        switch (i_BubbleType) {
            case 0: { //cures i_CureAmount people in a random region
                    corona_region_manager crm_RegionManager = gm_GameManager.Sector[Random.Range(0, gm_GameManager.Sector.Length)].GetComponent<corona_region_manager>();
                    crm_RegionManager.Cure(i_CureAmount);
                    Debug.Log("funktioniert (cure)");
                    break;
                }
            case 1:
                //gm_GameManager.ss_Science.SetCureProgress(gm_GameManager.ss_Science.GetCureProgress() + f_ResearchFactor);
                gm_GameManager.SetCurrentCure(gm_GameManager.GetCurrentCure() + f_ResearchFactor);
                Debug.Log("funktioniert (research)");
                break;
            case 2: {
                    corona_region_manager crm_RegionManager = gm_GameManager.Sector[Random.Range(0, gm_GameManager.Sector.Length)].GetComponent<corona_region_manager>();
                    if (crm_RegionManager.f_BubbleFactorSpread >= f_SpreadDelta) crm_RegionManager.f_BubbleFactorSpread -= f_SpreadDelta;
                    Debug.Log("funktioniert (spread)");
                    break;
                }
            default:
                Debug.Log("unexpected switch behaviour in BubbleInteract");
                break;
        }
        b_HasBeenClicked = true;
        StartCoroutine(DestroyBubble());
    }

    IEnumerator DestroyBubble() {
        transform.parent.GetComponentInChildren<Animator>().Play("Bubble_Pickup");
        yield return new WaitForSeconds(0.4f);
        End();
    }

    void End() {
        Destroy(transform.parent.gameObject);
    }
}