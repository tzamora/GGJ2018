using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class MouseManagerController : MonoBehaviour {

    Vector3 _box_start_pos;

    Vector3 _box_end_pos;

    public Texture selectionTexture;

    // Use this for initialization
    void Start () {

        MouseDragHandlerRoutine();

        MouseClickHandlerRoutine();

	}

    void OnGUI()
    {
        // If we are in the middle of a selection draw the texture.
        if (_box_start_pos != Vector3.zero && _box_end_pos != Vector3.zero)
        {
            // Create a rectangle object out of the start and end position while transforming it
            // to the screen's cordinates.
            var rect = new Rect(_box_start_pos.x, Screen.height - _box_start_pos.y,
                                _box_end_pos.x - _box_start_pos.x,
                                -1 * (_box_end_pos.y - _box_start_pos.y));
            // Draw the texture.
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
            GUI.DrawTexture(rect, selectionTexture);

            //
            // check if we have items inside the rect
            //

            List<UnitController> playerUnits = GameContext.Get.playerUnits;

            foreach (UnitController unit in playerUnits)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(unit.transform.localPosition);

                screenPosition.y = Screen.height - screenPosition.y;

                if (rect.Contains(screenPosition))
                {
                    unit.GetComponent<UnitController>().select();
                }
                
            }
        };
    }

    void MouseDragHandlerRoutine()
    {
        this.tt().Loop((handler) =>{

            if (Input.GetMouseButtonDown(1)) {

                foreach (UnitController unit in GameContext.Get.selectedUnits) {

                    unit.action(Input.mousePosition);

                }

            }

        });
    }

    void MouseClickHandlerRoutine() {

        this.tt().Loop((handler) => {

            // Called while the user is holding the mouse down.
            if (Input.GetKey(KeyCode.Mouse0))
            {
                // Called on the first update where the user has pressed the mouse button.
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    _box_start_pos = Input.mousePosition;
                else  // Else we must be in "drag" mode.
                    _box_end_pos = Input.mousePosition;
            }
            else
            {
                // Handle the case where the player had been drawing a box but has now released.
                if (_box_end_pos != Vector3.zero && _box_start_pos != Vector3.zero)
                    //HandleUnitSelection();

                // Reset box positions.
                _box_end_pos = _box_start_pos = Vector2.zero;
            }

        });

	}
}
