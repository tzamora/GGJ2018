using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using matnesis.TeaTime;

public class AllySpawnerController : MonoBehaviour {

    public enum AllyTypeEnum { Cleric, Animal, ghost }

    public AllyTypeEnum AllyType;

    public Transform spawnPosition;

    public GameObject ClericPrefab;

    public GameObject GhostPrefab;

    public GameObject AnimalPrefab;

    public GameObject ClericEnemyPrefab;

    public GameObject GhostEnemyPrefab;

    public GameObject AnimalEnemyPrefab;


    public int price = 5;

    public bool readyToUse = false;

    public float cooldownTime = 10;

    // Use this for initialization
    void Start () {

        this.tt("spawnerRoutine").Add(() =>
        {
            readyToUse = true;

        }).Add(cooldownTime).Repeat();
	}

    public void spawnAlly(UnitController parentUnit) {

        int availableMineral = GameContext.Get.mineralAmount;

        if (availableMineral < price)
        {

            // TODO: mensaje de error


            return;
        }

        GameObject unitPrefab = null;

        if (parentUnit.unitType == UnitController.UnitTypeEnum.ally)
        {
            switch (AllyType)
            {
                case AllyTypeEnum.Animal:
                    unitPrefab = AnimalPrefab;
                    break;
                case AllyTypeEnum.ghost:
                    unitPrefab = GhostPrefab;
                    break;
                case AllyTypeEnum.Cleric:
                    unitPrefab = ClericPrefab;
                    break;
            }

            GameObject newAlly = GameObject.Instantiate(unitPrefab, spawnPosition.position, Quaternion.identity);
            
            newAlly.GetComponent<UnitController>().unitType = UnitController.UnitTypeEnum.ally;

            GameContext.Get.allyUnits.Add(newAlly.GetComponent<UnitController>());
        }
        else
        {

            switch (AllyType)
            {
                case AllyTypeEnum.Animal:
                    unitPrefab = AnimalEnemyPrefab;
                    break;
                case AllyTypeEnum.ghost:
                    unitPrefab = GhostEnemyPrefab;
                    break;
                case AllyTypeEnum.Cleric:
                    unitPrefab = ClericEnemyPrefab;
                    break;
            }

            GameObject newAlly = GameObject.Instantiate(unitPrefab, spawnPosition.position, Quaternion.identity);

            newAlly.GetComponent<UnitController>().unitType = UnitController.UnitTypeEnum.enemy;

            GameContext.Get.enemyUnits.Add(newAlly.GetComponent<UnitController>());
        }
        
        readyToUse = false;

    }

    //void spawnAlly(GameObject allyPrefab)
    //{

    //    GameObject newAlly = GameObject.Instantiate(allyPrefab, spawnPosition.position, Quaternion.identity);
        
    //    GameContext.Get.allyUnits.Add(newAlly.GetComponent<UnitController>());

    //}
}
