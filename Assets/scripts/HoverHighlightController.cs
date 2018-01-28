using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlightController : MonoBehaviour {

    Material originalMaterial;

    public Material highlightMaterial;

    public void Start()
    {
        
        
    }

    public void hover(Vector3 mousePosition) {

        mousePosition.z = 0;

        Cursor.visible = false;

        GameContext.Get.cursorHand.transform.position = mousePosition;
    }

    public void exit()
    {
        Cursor.visible = true;
    }
}
