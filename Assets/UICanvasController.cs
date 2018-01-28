using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using matnesis.TeaTime;

public class UICanvasController : MonoBehaviour {

    public Text mineralAmountText;

	// Use this for initialization

	void Start () {

        this.tt("updateMineralAmount").Add((handler)=> {
            
            mineralAmountText.text = GameContext.Get.mineralAmount + "";

        }).Add(0.5f).Repeat();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
