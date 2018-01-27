using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class UnitController : MonoBehaviour {

    // Use this for initialization
    void Start () {

        MoveRoutine();

		
	}


    void MoveRoutine() {

        this.tt().Loop((handler)=> {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            //print(mousePosition);
        });

    }
}
