using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class CrossAppearAndDestroy : MonoBehaviour {

    public float speed = 1;
    public float time = 1;


    void Start () {

        MakeSmaller();

    }
	
	void MakeSmaller () {

        this.tt().Loop(time, delegate (ttHandler handler)
        {
            transform.localScale = new Vector3(transform.localScale.x - speed, transform.localScale.y - speed, 1);

        }).Add( t => { Destroy(gameObject);  } );

	}
}
