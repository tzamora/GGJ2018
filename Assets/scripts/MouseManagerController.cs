using System.Collections;
using System.Collections.Generic;
using matnesis.TeaTime;
using UnityEngine;

public class MouseManagerController : MonoBehaviour {

    Vector3 _box_start_pos;

    Vector3 _box_end_pos;

    public Texture selectionTexture;

    public GameObject crossHelper;

    // Use this for initialization
    void Start () {

        MouseDragHandlerRoutine();

        MouseClickHandlerRoutine();

        MouseOverHighlight();
        
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

            List<UnitController> allUnits = new List<UnitController>();

            allUnits.AddRange(GameContext.Get.allyUnits);

            allUnits.AddRange(GameContext.Get.enemyUnits);

            //
            // evaluate player units
            //

            foreach (UnitController unit in allUnits)
            {
                if (unit == null) {
                    continue;
                }

                Vector2 screenPosition = Camera.main.WorldToScreenPoint(unit.transform.localPosition);

                screenPosition.y = Screen.height - screenPosition.y;

                if (rect.Contains(screenPosition))
                {
                    unit.select();
                }

            }
        };
    }

    void MouseClickHandlerRoutine()
    {
        this.tt().Loop((handler) =>{

            if (Input.GetMouseButtonDown(1)) {

                //
                // show the cursor
                //

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Instantiate(crossHelper, new Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);

                //
                // now get the mousePos
                //

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                GameObject hitGameObject = null;

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                // cross helper 
                //Instantiate(crossHelper, mousePos, Quaternion.identity);

                if (hit)
                {
                    hitGameObject = hit.transform.gameObject;

                    print(hitGameObject);

                }

                foreach (UnitController unit in GameContext.Get.selectedPlayerUnits) {

                 unit.MoveRoutine(mousePos, hitGameObject);

                }

            }

            if (Input.GetMouseButton(0)) {

                foreach (UnitController unit in GameContext.Get.selectedPlayerUnits) {

                    unit.resetRoutine();

                }

            }

        });
    }

    void MouseDragHandlerRoutine() {

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
     
    void MouseOverHighlight() {

        this.tt("HoverHighlightRoutine").Loop((handler) => {

            //
            // now get the mousePos
            //

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject hitGameObject = null;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                print("pegamos");

                hitGameObject = hit.transform.gameObject;

                if (hitGameObject.tag == "hover")
                {

                    mousePos.z = 0;

                   Cursor.visible = false;

                    GameContext.Get.cursorHand.transform.position = mousePos + new Vector3(0.1f, -0.25f, 0.1f);

                }
                

                //HoverHighlightController highlightHover = hitGameObject.GetComponent<HoverHighlightController>();
                //HoverHighlightController backupHighlightHover = highlightHover;
                ////
                //// esto se va a llamar como loco
                ////
                //if (highlightHover)
                //{
                //    highlightHover.hover(mousePos);
                //}
                //else {
                //    if(backupHighlightHover) backupHighlightHover.exit();
                //}
            }
            else
            {
                Cursor.visible = true;
                GameContext.Get.cursorHand.transform.position = new Vector3(1000f, 1000f, 1000f);
            }
        });
    }
}
