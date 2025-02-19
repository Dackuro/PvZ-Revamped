using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour // PoolManager, simplemente lo dado en el temario.
{
    [SerializeField] private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("PoolManager is null");
            }
            return instance;
        }
    }

    [Header("Objetos")]
    [SerializeField] private GameObject container;
    private Dictionary<GameObject, List<GameObject>> prefabPools = new Dictionary<GameObject, List<GameObject>>();
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();
    [SerializeField] private int amount = 10;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitializePrefabs();
    }
    void InitializePrefabs()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            List<GameObject> prefabPool = GeneratePrefabs(prefabList[i], amount);
            prefabPools.Add(prefabList[i], prefabPool);
        }
    }
    List<GameObject> GeneratePrefabs(GameObject prefab, int cantidad)
    {
        List<GameObject> prefabPool = new List<GameObject>();
        for (int i = 0; i < cantidad; i++)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.transform.parent = container.transform;
            newPrefab.SetActive(false);
            prefabPool.Add(newPrefab);
        }
        return prefabPool;
    }
    public GameObject RequestPrefab(GameObject prefab)
    {
        if (!prefabPools.ContainsKey(prefab))
        {
            prefabPools[prefab] = new List<GameObject>();
        }

        List<GameObject> pool = prefabPools[prefab];

        GameObject obj = pool.Find(p => !p.activeInHierarchy);
        if (obj != null)
        {
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.transform.parent = container.transform;
            pool.Add(newPrefab);
            return newPrefab;
        }
    }
}