using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("Scripts")]
    [SerializeField] private InputManager inputManager;

    [Header("Objects")]
    [SerializeField] private List<GameObject> placedPlants = new();

    public int PlaceObject(GameObject prefab, Vector3 position, Vector3Int gridPosition)
    {
        GameObject newPlant = Instantiate(prefab);
        newPlant.transform.position = position;
        placedPlants.Add(newPlant);
        audioSource.PlayOneShot(audioClip);
        newPlant.SetActive(true);
        newPlant.transform.position = inputManager.hit.transform.position;

        Plants plants;
        plants = newPlant.GetComponent<Plants>();
        plants.gridPos = gridPosition;

        return placedPlants.Count -1;
    }

    internal void RemoveObject(int selectedPlantIndex)
    {
        if (placedPlants.Count <= selectedPlantIndex || placedPlants[selectedPlantIndex] == null)
        {
            return;
        }
        audioSource.PlayOneShot(audioClip);

        Destroy(placedPlants[selectedPlantIndex]);
        placedPlants[selectedPlantIndex] = null;
    }

    public void RemoveObjectByDeath(GameObject plant)
    {
        int index = placedPlants.IndexOf(plant);

        Destroy(placedPlants[index]);
        placedPlants[index] = null;
    }
}
