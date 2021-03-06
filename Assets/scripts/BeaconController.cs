﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using matnesis.TeaTime;
using UnityEngine.SceneManagement;

public class BeaconController : MonoBehaviour {

    public static int allActivated = 0;
    public Camera cameraAnimation;
    public int requiredBeacons;
    public float speed;
    public float scalingSpeed;
    public float fadeSpeed;
    public GameObject tilemapA;
    public GameObject tilemapB;
    public Image fadeToWhite;
    public Text timesText;

    bool once=false;

    void Start () {

        requiredBeacons = Random.Range(3,5);
        timesText.text = "This time requires to activate " + requiredBeacons + " things.";

        this.tt().Add(10).Add( t => Destroy(timesText) );

    }

    void Update () {

        if (allActivated == requiredBeacons)
        { once = true; }

        if (once == true)
        {

            AllBeaconsReady();
            requiredBeacons = 999;
            once = false;
        }
    }

    void AllBeaconsReady()
    {

        this.tt().Loop(4, delegate (ttHandler handler)
        {
            Destroy(cameraAnimation.GetComponent<MouseCameraMove>());


        }).Loop(30f, delegate (ttHandler handler)
        {

            cameraAnimation.transform.position =
                Vector3.MoveTowards(cameraAnimation.transform.position, new Vector3(-20, 50, cameraAnimation.transform.position.z), speed);

            cameraAnimation.orthographicSize += scalingSpeed * Time.deltaTime;

            tilemapA.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.None;
            tilemapB.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.None;

        });


        this.tt().Loop(20, delegate (ttHandler handler)
        {


        }).Loop(10f, delegate (ttHandler handler)
        {

            fadeToWhite.color = Color.Lerp
                (fadeToWhite.color, new Vector4(1, 1, 1, 1), fadeSpeed * Time.deltaTime);

        }).Add(delegate (ttHandler handler)
        {
            SceneManager.LoadScene("Outro");
        });
    }
}
