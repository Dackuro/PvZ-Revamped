using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantsDatabase : ScriptableObject // Base de datos de las plantas existentes.
{
    public List<PlantsData> plantsData;
}

[Serializable]
public class PlantsData // Variables que posee cada planta.
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
