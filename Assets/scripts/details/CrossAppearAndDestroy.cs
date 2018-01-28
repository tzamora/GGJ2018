using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class CrossAppearAndDestroy : MonoBehaviour {

    public float speed = 0.1f;
    public float time = 1;
    public float rotation = 1;


    void Start () {

        int r = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, r));

        MakeSmaller();

    }
	
	void MakeSmaller () {

        this.tt().Loop(time, delegate (ttHandler handler)
        {

            transform.Rotate(0,0, rotation);
            transform.localScale -= new Vector3(speed, speed, speed) * Time.deltaTime;

        }).Add( t => { Destroy(gameObject);  } );

	}
}
