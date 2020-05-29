using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour{
    public GameObject[] bubbles;
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMaxWait;
    public float spawnMinWait;
    public int startWait;
    public Camera cam_Camera;

    private int randomBubble;

    void Start(){
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update(){
        
    }

    IEnumerator Spawner(){
        yield return new WaitForSeconds(startWait);
        yield return new WaitForSeconds(Random.Range(spawnMinWait, spawnMaxWait));

        while (true){
            randomBubble = Random.Range(0, 3);
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 2, Random.Range(-spawnValues.z, spawnValues.z));

            //GameObject go_bubble = Instantiate(bubbles[randomBubble], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            //go_bubble.GetComponent<Canvas>().worldCamera = cam_Camera;
            Instantiate(bubbles[randomBubble], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }
}