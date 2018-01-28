using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using matnesis.TeaTime;

public class Story : MonoBehaviour {

    public string nextLevel;

    public Image picA;
    public Image picB;
    public Image picC;
    public Text txtA;
    public Text txtB;
    public Text txtC;

    public Color txtColor;
    public Color picColor;

    public float speedSlowFade;
    public float speedFastFade;
    public float shortWait;
    public float longWait;

    public void Start()
    {
        txtA.color = Color.clear;
        txtB.color = Color.clear;
        txtC.color = Color.clear;
        picA.color = Color.clear;
        picB.color = Color.clear;
        picC.color = Color.clear;

        this.tt().Loop(0.5f, t =>
        {

        }).Loop(longWait, t =>
        {
            txtA.color = Color.Lerp(txtA.color, txtColor, speedSlowFade);
            picA.color = Color.Lerp(picA.color, picColor, speedFastFade);

        }).Loop(shortWait, t =>
        {
            txtA.color  = Color.Lerp(txtA.color, Color.clear, speedFastFade);
            picA.color  = Color.Lerp(picA.color, Color.clear, speedSlowFade);

        }).Loop(longWait, t =>
        {
            txtA.color = Color.clear;
            picA.color = Color.clear;
            txtB.color = Color.Lerp(txtB.color, txtColor, speedSlowFade);
            picB.color = Color.Lerp(picB.color, picColor, speedFastFade);

        }).Loop(shortWait, t =>
        {
            txtB.color  = Color.Lerp(txtB.color, Color.clear, speedFastFade);
            picB.color  = Color.Lerp(picB.color, Color.clear, speedSlowFade);

        }).Loop(longWait, t =>
        {
            txtB.color = Color.clear;
            picB.color = Color.clear;
            txtC.color = Color.Lerp(txtC.color, txtColor, speedSlowFade);
            picC.color = Color.Lerp(picC.color, picColor, speedFastFade);

        }).Loop(shortWait, t =>
        {
            txtC.color  = Color.Lerp(txtC.color, Color.clear, speedFastFade);
            picC.color  = Color.Lerp(picC.color, Color.clear, speedSlowFade);

        }).Add(t => SceneManager.LoadScene(nextLevel));

    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(nextLevel);
        };
    }
}
