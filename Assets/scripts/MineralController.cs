using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class MineralController : MonoBehaviour {

    public float amount = 10;

    public SpriteRenderer filledSprite;

    public SpriteRenderer emptySprite;

    public float extractTime = 0.3f;

    Material originalMaterial;

    public Material highlightMaterial;

    public bool isDepleted = false;

    public AudioClip minning;

    public void extract(UnitController unit) {


        Renderer renderer = GetComponent<Renderer>();

        originalMaterial = filledSprite.material;

        filledSprite.material = highlightMaterial;

        this.tt("extractMineralRoutine").Add(() => {

            if (amount > 0)
            {
                amount--;

                SoundManager.Get.PlayClip(minning, false);

                if (unit.unitType == UnitController.UnitTypeEnum.ally)
                {
                    GameContext.Get.allyMineralAmount++;
                }
                else
                {
                    GameContext.Get.enemyMineralAmount++;
                }

                GameContext.Get.allyMineralAmount++;
            }
            else
            {
                filledSprite.gameObject.SetActive(false);
                isDepleted = true;
                unit.isBusy = false;
                this.tt("extractMineralRoutine").Release();
            }

        }).Add(extractTime).Repeat();

    }
}
