using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class UnitController : MonoBehaviour {

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;

    // Use this for initialization
    void Start () {
    

		
	}


    void MoveRoutine(Vector2 destination) {

        destination = Camera.main.ScreenToWorldPoint(destination);
        //destination.y = Screen.height - destination.y;

        this.tt("MoveRoutine").Reset().Loop((handler)=> {

            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * movementSpeed);
            if (transform.position == (Vector3)destination) {
                print("llegamos");
                handler.EndLoop();
            }

        });

    }

    public void action(Vector2 destination) {

        MoveRoutine(destination);

    }

    public void select() {

        Renderer renderer = GetComponent<Renderer>();

        originalMaterial = renderer.material;

        renderer.material = highlightMaterial;

        //
        // add to selected list
        //

        GameContext.Get.selectedUnits.Add(this);

    }

    void deselect() {
        GameContext.Get.selectedUnits.Remove(this);
    }
}
