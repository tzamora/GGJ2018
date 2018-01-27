﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class CrossAppearAndDestroy : MonoBehaviour {

    public float speed = 0.1f;
    public float time = 1;


    void Start () {

        MakeSmaller();

    }
	
	void MakeSmaller () {

        this.tt().Loop(time, delegate (ttHandler handler)
        {

            transform.localScale -= new Vector3(speed, speed, speed) * Time.deltaTime;

        }).Add( t => { Destroy(gameObject);  } );

	}
}
