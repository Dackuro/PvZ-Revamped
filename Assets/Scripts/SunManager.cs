using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SunManager : MonoBehaviour // Control general de los soles.
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Objects")]
    public GameObject sunPrefab;
    SunScript sunScript;
    public Transform lastPoint;
    public float travelTime;
    bool canBePicked;

    void Start()
    {
        travelTime = 1f;
        InvokeRepeating("SpawnSun", Random.Range(3f, 4f), Random.Range(8f, 9f)); // Llama al generador de soles al principio entre 3 y 4 segundos, y lo vuelve a hacer cada 8 a 9 segundos.
    }

    void Update()
    {
        PickSun();
    }

    void SpawnSun() // Spawnea aleatoriamente sobre el plano jugable.
    {
        float randomX = Random.Range(0, 45f);
        float randomZ = Random.Range(0, 20f);
        Vector3 randomPosition = new Vector3(randomX, 50f, randomZ);

        GameObject newSun = Instantiate(sunPrefab, randomPosition, Quaternion.identity);
    }

    void PickSun() // Detecta si el ratón está por encima del sol.
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Sun"))
            {
                StartCoroutine(CollectSun(hitObject));
            }
        }
    }

    IEnumerator CollectSun(GameObject sun) // Movimiento de sol al holder.
    {
        sunScript = sun.GetComponent<SunScript>();
        canBePicked = sunScript.onGround;
        if (canBePicked)
        {
            if (!sunScript.sound)
            {
                audioSource = sun.GetComponent<AudioSource>();
                audioSource.Play();
                sunScript.sound = true;
            }            

            sunScript.StopDestroy();
            Vector3 sunPos = sun.transform.position;
            Vector3 lastPos = lastPoint.position;
            float waitTime = 0f;

            while (waitTime < travelTime && sun != null)
            {
                float t = waitTime / travelTime;
                Vector3 endPos = Vector3.Lerp(sunPos, lastPos, t);
                sun.transform.position = endPos;
                waitTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            yield return null;
        }
    }
}