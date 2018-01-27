using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpawnerController : MonoBehaviour {

    public enum AllyType { Cleric, Animal, ghost }

    public Transform spawnPosition;

    public GameObject ClericPrefab;

    public GameObject GhostPrefab;

    public GameObject AnimalPrefab;

    // Use this for initialization
    void Start () {
        activateSpawner();
	}

    void activateSpawner() {

        GameObject newAlly = GameObject.Instantiate(ClericPrefab, spawnPosition.position, Quaternion.identity);

        GameContext.Get.playerUnits.Add(newAlly.GetComponent<UnitController>());

    }
}
