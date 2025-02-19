using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class WaveSpawner : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip waveClip;

    [Header("Variables")]
    public bool startGame;
    public int currentWave;
    public int waveValue;

    public List<Enemy> enemies = new();
    public List<GameObject> enemiesToSpawn = new();
    public List<GameObject> spawnedEnemies = new();

    [Header("Objetos")]
    public List<GameObject> spawners;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private GameObject newWaveText;

    private void Start()
    {
        startGame = false;
        waveText.text = "0";
        StartCoroutine(EsperaInicial());
        InvokeRepeating("CheckZombies", 20, 10);
    }

    void CheckZombies()
    {
        if (spawnedEnemies.Count == 0)
        {
            GenerateWave();
        }
    }

    IEnumerator EsperaInicial()
    {
        yield return new WaitForSeconds(15f);
        startGame = true;
        GenerateWave();
    }


    public void GenerateWave() // Genera la wave para que se pueda crear poco a poco.
    {
        currentWave++;
        waveText.text = currentWave.ToString("");

        PlayerPrefs.SetString("Record", waveText.text);

        waveValue = 1 + currentWave * 3;
        GenerateEnemies();
        StartCoroutine(WaveWarning());
    }
    IEnumerator WaveWarning()
    {
        audioSource.PlayOneShot(waveClip);
        newWaveText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        newWaveText.SetActive(false);
    }

    public void GenerateEnemies() // Genera los enemigos.
    {
        List<GameObject> generatedEnemies = new();
        while (waveValue > 1 && generatedEnemies.Count <= 2)
        {
            int randEnemyID = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyID].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyID].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 1)
            {
                break;
            }
        }
        enemiesToSpawn = generatedEnemies;

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (startGame)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        for (int i = enemiesToSpawn.Count - 1; i >= 0; i--)
        {
            GameObject enemy = Instantiate(enemiesToSpawn[i], spawners[Random.Range(0, spawners.Count)].transform.position, Quaternion.Euler(0, -90, 0));
            spawnedEnemies.Add(enemy);
            enemiesToSpawn.RemoveAt(i);
            yield return new WaitForSeconds(2f);
        }
    }
}

[System.Serializable]
public class Enemy // Datos de los Zombies
{
    public GameObject enemyPrefab;
    public int cost;
}