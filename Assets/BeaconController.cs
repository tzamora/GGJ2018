using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeaconController : MonoBehaviour {

    public static int allActivated = 0;
    public Camera cameraAnimation;
    public int requiredBeacons;
    public float speed;
    public float scalingSpeed;
    public GameObject tilemapA;
    public GameObject tilemapB;

    void Start () {
		
	}
	
	void Update () {
        if (allActivated == requiredBeacons)
        { AllBeaconsReady(); }
    }

    void AllBeaconsReady()
    {

        cameraAnimation.transform.position = 
            Vector3.MoveTowards(cameraAnimation.transform.position, new Vector3(-20,50, cameraAnimation.transform.position.z), speed);

        cameraAnimation.orthographicSize += scalingSpeed * Time.deltaTime;

        tilemapA.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.None;
        tilemapB.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.None;

        Destroy(cameraAnimation.GetComponent<MouseCameraMove>());
    }
}
