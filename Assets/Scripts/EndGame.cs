using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class EndGame : MonoBehaviour // Script extremadamente básico para detectar, terminar la partida y sacar el PopUp de fin.
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip endClip;

    [Header("Scripts")]
    [SerializeField] private IngameMenuController iMC;
    [SerializeField] WaveSpawner waveSpawner;

    [Header("Objetos")]
    [SerializeField] TMP_Text textoWaves;
    [SerializeField] GameObject endGamePopUp;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject menu;
    public Camera cam;

    [Header("Variables")]
    public bool gameOver;

    void Start()
    {
        gameOver = false;
        textoWaves.text = 0.ToString("");
        waveSpawner = GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Vector3 zombiePos = new Vector3(other.transform.position.x, other.transform.position.y + 1.5f, other.transform.position.z);
            Debug.Log("Partida terminada");
            gameOver = true;
            cam.fieldOfView = 20;
            cam.transform.LookAt(zombiePos);
            audioSource.PlayOneShot(endClip);
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            textoWaves.text = waveSpawner.currentWave.ToString("");
            StartCoroutine(Ending());
        }
    }
    
    IEnumerator Ending()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        iMC.OnMenuBool(false);
        menu.SetActive(true);
        pauseMenu.SetActive(false);
        endGamePopUp.SetActive(true);
    }
}
