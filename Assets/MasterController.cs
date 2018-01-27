using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class MasterController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        float speed = 5;

        float xAxis = 0;

        float yAxis = 0;

        this.tt("move").Loop((handler) => {

            xAxis = Input.GetAxis("Horizontal");

            yAxis = Input.GetAxis("Vertical");

            this.transform.position = new Vector3(xAxis, yAxis, 0) * speed;

        });

	}
	
	// Update is called once per frame
	void Update () {
		


	}
}
