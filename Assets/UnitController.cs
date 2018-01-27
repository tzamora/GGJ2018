using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class UnitController : MonoBehaviour {

    public int hp = 10;

    public int attack = 10;

    public enum UnitTypeEnum { ally, enemy };

    public UnitTypeEnum unitType;

    Material originalMaterial;

    public Material highlightMaterial;

    public float movementSpeed = 10f;

    // Use this for initialization
    void Start () {
    

		
	}

    public void action(Vector2 destination, GameObject other) {

        destination = Camera.main.ScreenToWorldPoint(destination);
        //destination.y = Screen.height - destination.y;

        this.tt("MoveRoutine").Reset().Loop((handler) => {

            //Animator.play();

            transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * movementSpeed);

            if (transform.position == (Vector3)destination)
            {
                handleAction(other);

                // .stop

                handler.EndLoop();
            }

        });

    }

    void DeathRoutine()
    {

        this.tt("MoveRoutine").Loop((handler) =>
        {

            if (hp <= 0)
            {

                // death animation

                // destroy

            }

        });
    }

    void handleAction(GameObject other) {

        if(other is UnitController)
        {

           UnitController unit = other.GetComponent<UnitController>();

            if (unit.unitType == UnitTypeEnum.enemy)
            {
                attack();
            }

        }

    }

    public void select() {

        Renderer renderer = GetComponent<Renderer>();

        originalMaterial = renderer.material;

        renderer.material = highlightMaterial;

        //
        // add to selected list
        //

        if (!GameContext.Get.selectedUnits.Contains(this)) {
            GameContext.Get.selectedUnits.Add(this);
        }
        

    }

    void deselect() {
        GameContext.Get.selectedUnits.Remove(this);
    }
}
