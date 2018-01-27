using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class StartBackGroundMove : MonoBehaviour {

    public float speed;
    public float wait;

    void Start () {

        MoveRoutine();

    }

    void MoveRoutine() {

        this.tt("MoveRoutine").Loop(wait, delegate (ttHandler handler) {

            transform.Translate(speed, speed, 0);

        }).Loop(wait, delegate( ttHandler handler) {

            transform.Translate(-speed, -speed, 0);

        }).Repeat();

    }





}
