using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CostManager : MonoBehaviour // Control general de los soles como monedas. Compras y ventas.
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioBuy;
    public AudioClip audioError;

    [Header("Scripts")]
    public PlacementSystem pSystem;

    [Header("Sun")]
    public TMP_Text sunDisplay;
    public int sunAmount;
    public int startingSunAmount;

    void Start()
    {
        startingSunAmount = 50;
        AddSun(startingSunAmount);
    }

    public void PlantData(string data) // Recibe los datos de la carta de la planta. Primero los descifra y luego llama a la compra.
    {
        int ID = int.Parse(data.Substring(0, 2));
        int cost = int.Parse(data.Substring(2, 3));
        BuyPlant(ID, cost);
    }
    public void BuyPlant(int ID, int cost) // Compra la planta si hay dinero suficiente.
    {
        if (sunAmount - cost < 0)
        {
            audioSource.PlayOneShot(audioError);
        }
        else 
        {
            pSystem.StartPlacement(ID, cost);
            audioSource.PlayOneShot(audioBuy);   
        }
    }
    public void AddSun(int amount) // Añade sol.
    {
        sunAmount += amount;
        sunDisplay.text = sunAmount.ToString("0");
    }
    public void SubstractSun(int amount) // Resta sol.
    {
        sunAmount -= amount;
        if (amount < 0)
        {
            amount = 0;
        }
        sunDisplay.text = sunAmount.ToString("0");
    }
}
