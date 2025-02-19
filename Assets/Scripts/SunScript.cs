using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunScript : MonoBehaviour // Controlador de los soles como objeto.
{
    [Header("Script")]
    CostManager cost;

    [Header("Variables")]
    public bool sound;
    public float fallTime;
    public float lifeTime;
    private Vector3 startPos;
    public bool onGround = false, picked;

    void Start()
    {
        cost = FindAnyObjectByType<CostManager>();
        startPos = transform.position;
        fallTime = 5f;
        lifeTime = 5f;

        StartDescent();
    }
    void Update()
    {
        if (onGround && !picked)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) // Timer para que desaparezca de no ser recogido.
            {
                Destroy(gameObject);
            }
        }
    }
    void StartDescent()
    {
        StartCoroutine(Descend());
    }
    public void StopDestroy() // Evita su destrucción. Se le llama desde otro SunManager.
    {
        picked = true;
    }
    IEnumerator Descend() // Lógica de caída del sol.
    {
        float waitTime = 0f;
        while (waitTime < fallTime && !onGround)
        {
            float t = waitTime / fallTime;
            Vector3 endPos = Vector3.Lerp(startPos, new Vector3(startPos.x, 0, startPos.z), t);
            transform.position = endPos;
            waitTime += Time.deltaTime;
            yield return null;
        }
        onGround = true;
    } 

    IEnumerator DestroySelf()
    {
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Wall"))
        {
            cost.AddSun(25);
            StartCoroutine(DestroySelf());
        }
    }

    public void CanBePickedFunction()
    {
        onGround = true;
    }
}
