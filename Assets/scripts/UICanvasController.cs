using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using matnesis.TeaTime;

public class UICanvasController : MonoBehaviour {

    public Text allyMineralAmountText;

    public Text enemyMineralAmountText;

    // Use this for initialization

    void Start () {

        this.tt("updateMineralAmount").Add((handler)=> {
            
            allyMineralAmountText.text = GameContext.Get.allyMineralAmount + "";

            enemyMineralAmountText.text = GameContext.Get.enemyMineralAmount + "";

        }).Add(0.5f).Repeat();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
