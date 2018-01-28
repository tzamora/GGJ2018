using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class StoneLightAndParticles : MonoBehaviour {

    public Color publicColorA = Color.red;
    public Color publicColorB = Color.blue;
    public Color publicColorC = Color.green;
    public float duration = 6;
    public float speed= 0.5f;
    SpriteRenderer colorComponent;
    public GameObject StoneLights;

	void Start () {

        colorComponent = GetComponent<SpriteRenderer>();
        Animation();

    }



    void Animation()
    {

        Instantiate(StoneLights, transform.localPosition, Quaternion.identity);

        this.tt().Loop(duration, delegate (ttHandler handler)
        {

            colorComponent.color = Color.Lerp(colorComponent.color, publicColorA, speed * Time.deltaTime);

        }).Loop(duration, delegate (ttHandler handler)
        {

            colorComponent.color = Color.Lerp(colorComponent.color, publicColorB, speed * Time.deltaTime);

        }).Loop(duration, delegate (ttHandler handler)
        {

            colorComponent.color = Color.Lerp(colorComponent.color, publicColorC, speed * Time.deltaTime);

        }).Loop(duration, delegate (ttHandler handler)
        {

            // GET CAMERA CANVAS TEXTURE AND GET COLOR ALPHA FROM 0 TO 255

        });

    }
}
