using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class StoneLightAndParticles : MonoBehaviour {

    public Color publicColorA = Color.red;
    public Color publicColorB = Color.blue;
    public float duration = 6;
    public float speed= 0.5f;
    SpriteRenderer colorComponent;
    public GameObject StoneLights;
    bool activated = false;
    public AudioClip beaconSound;

    void Start () {

        colorComponent = GetComponent<SpriteRenderer>();

    }


    public void OnTriggerEnter2D (Collider2D other)
    {
        var ally = other.GetComponent<UnitController>();

        if (ally && !activated)

            if (ally.unitType == UnitController.UnitTypeEnum.ally)
            {
                print("ha entrado una vez");
                activated = true;
                SoundManager.Get.PlayClip(beaconSound, false);
                BeaconController.allActivated++;
                Animation();
            }

    }


    public void Animation()
    {

        Instantiate(StoneLights, transform.position, Quaternion.identity);

        this.tt().Loop(duration, delegate (ttHandler handler)
        {

            colorComponent.color = Color.Lerp(colorComponent.color, publicColorA, speed * Time.deltaTime);

        }).Loop(duration, delegate (ttHandler handler)
        {

            colorComponent.color = Color.Lerp(colorComponent.color, publicColorB, speed * Time.deltaTime);

        }).Repeat();
    }
}
