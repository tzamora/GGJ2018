using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpawnerController : MonoBehaviour {

    public enum AllyTypeEnum { Cleric, Animal, ghost }

    public AllyTypeEnum AllyType;

    public Transform spawnPosition;

    public GameObject ClericPrefab;

    public GameObject GhostPrefab;

    public GameObject AnimalPrefab;

    public int price = 5;

    public float respawnTime = 10;

    // Use this for initialization
    void Start () {
        activateSpawner();
	}

    public static int counter = 0;

    void activateSpawner() {

        GameObject newAlly = GameObject.Instantiate(ClericPrefab, spawnPosition.position, Quaternion.identity);
        newAlly.name = newAlly.name + counter++;
        GameContext.Get.allyUnits.Add(newAlly.GetComponent<UnitController>());

    }

    void spawnAlly(GameObject allyPrefab)
    {

        GameObject newAlly = GameObject.Instantiate(allyPrefab, spawnPosition.position, Quaternion.identity);
        
        GameContext.Get.allyUnits.Add(newAlly.GetComponent<UnitController>());

    }

    public void trySpawn() {

        int availableMineral = GameContext.Get.mineralAmount;

        if (availableMineral < price) {

            // TODO: mensaje de error


            return;
        }

        GameObject allyPrefab = null;

        switch (AllyType) {
            case AllyTypeEnum.Animal:
                allyPrefab = AnimalPrefab;
            break;
            case AllyTypeEnum.ghost:
                allyPrefab = GhostPrefab;
                break;
            case AllyTypeEnum.Cleric:
                allyPrefab = ClericPrefab;
                break;
        }
        spawnAlly(allyPrefab);


    }
}
