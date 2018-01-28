using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraMove : MonoBehaviour {

    Vector2 mousePosition;

    public float speed = 0.14f;
    public float speedButton = 0.4f;

    float xa = 47;
    float xb = -85;
    float ya = 108.5f;
    float yb = -6.5f;

    float x;
    float y;

    bool maximumX = false;
    bool minimumX = false;
    bool maximumY = false;
    bool minimumY = false;

    void Update () {

        x = Input.mousePosition.x;
        y = Input.mousePosition.y;

        mousePosition = new Vector2(x, y);

        ConditionsHoverEdges();
        ConditionsMouseButton();

        if (transform.position.x > xa) maximumX = true; else maximumX = false;
        if (transform.position.x < xb) minimumX = true; else minimumX = false;
        if (transform.position.y > ya) maximumY = true; else maximumY = false;
        if (transform.position.y < yb) minimumY = true; else minimumY = false;

    }

    void ConditionsHoverEdges()
    {
        if (x > Screen.width * 0.9f && !maximumX)
            transform.Translate(speed, 0,0);

        if (x < Screen.width * 0.1f && !minimumX)
            transform.Translate(-speed, 0, 0);

        if (y > Screen.height * 0.9f && !maximumY)
            transform.Translate(0, speed, 0);

        if (y < Screen.height * 0.1f && !minimumY)
            transform.Translate(0, -speed, 0);
    }

    void ConditionsMouseButton()
    {

        if (Input.GetMouseButton(2))
        {

            if (Input.GetAxis("Mouse X") > -0.1f && !maximumX)
                transform.Translate(speedButton, 0, 0);
            if (Input.GetAxis("Mouse X") < 0.1f && !minimumX)
                transform.Translate(-speedButton, 0, 0);
            if (Input.GetAxis("Mouse Y") > -0.1f && !maximumY)
                transform.Translate(0, speedButton, 0);
            if (Input.GetAxis("Mouse Y") < 0.1f && !minimumY)
                transform.Translate(0, -speedButton, 0);
        }

    }


}
