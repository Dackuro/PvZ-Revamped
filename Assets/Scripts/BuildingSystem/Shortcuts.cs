using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shortcuts : MonoBehaviour
{
    [Header("Scritps")]
    [SerializeField] private CostManager costManager;
    [SerializeField] private PlacementSystem placementSystem;

    [SerializeField] private GridCell gridCell;

    void Update()
    {
        Controles();
    }

    void Controles()
    {
        switch (Input.inputString)
        {
            case "1": // Shortcut: Sunflower
                costManager.PlantData("00050");
                break;

            case "2": // Shortcut: Twin Sunflower
                costManager.PlantData("01200");
                break;

            case "3": // Shortcut: Peashooter
                costManager.PlantData("02100");
                break;

            case "4": // Shortcut: Ice Pea
                costManager.PlantData("04175");
                break;

            case "5": // Shortcut: Repeater
                costManager.PlantData("03200");
                break;

            case "6": // Shortcut: Gatling Pea
                costManager.PlantData("10450");
                break;

            case "7": // Shortcut: WallNut
                costManager.PlantData("08050");
                break;

            case "8": // Shortcut: TallWallNut

                costManager.PlantData("09125");
                break;

            case "9": // Shortcut: Potato Mine
                costManager.PlantData("06025");
                break;

            case " ": // Shortcut: Shovel
                placementSystem.StartRemoving();
                break;

            default:
                break;
        }
    }
}
