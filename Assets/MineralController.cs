using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class MineralController : MonoBehaviour {

    public float amount = 10;

    public SpriteRenderer filledSprite;

    public SpriteRenderer emptySprite;

    Material originalMaterial;

    public Material highlightMaterial;

    public void extract() {

        Renderer renderer = GetComponent<Renderer>();

        originalMaterial = filledSprite.material;

        filledSprite.material = highlightMaterial;

        this.tt("extractMineralRoutine").Add(() => {

            if (amount > 0)
            {
                amount--;
                GameContext.Get.mineralAmount++;
            }
            else
            {
                filledSprite.gameObject.SetActive(false);
            }

        }).Add(0.3f).Repeat();

    }
}
