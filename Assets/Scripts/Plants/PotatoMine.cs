using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotatoMine : Plants
{
    public float explosionArea;
    public bool isArmed;

    void Start()
    {
        explosionArea = 6f;   
        canTakeDamage = true;

        stats.SetupVida(100, 0, 100, gameObject);
        if (isActive)
        {
            Invoke("PrepareToHide", 0.1f);
            Invoke("PreparingToExplode", 15f);
        }
    }

    void PrepareToHide()
    {
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        Vector3 startPos = transform.position;

        float hideTime = 0.2f;
        float waitTime = 0f;
        while (waitTime < hideTime)
        {
            float t = waitTime / hideTime;

            Vector3 endPos = Vector3.Lerp(startPos, new Vector3(startPos.x, startPos.y - 0.4f, startPos.z), t);
            transform.position = endPos;
            waitTime += Time.deltaTime;

            yield return null;
        }
    }
    void PreparingToExplode()
    {
        StartCoroutine(Rearm());
    }
    IEnumerator Rearm() // Lógica de rearmado de la patata.
    {
        Vector3 startPos = transform.position;

        float armTime = 0.5f;
        float waitTime = 0f;
        while (waitTime < armTime && !isArmed)
        {
            float t = waitTime / armTime;

            Vector3 endPos = Vector3.Lerp(startPos, new Vector3(startPos.x, startPos.y + 0.4f, startPos.z), t);
            transform.position = endPos;
            waitTime += Time.deltaTime;

            yield return null;
        }
        isArmed = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombie"))
        {
            if (!isActive)
                return;

            if (isExploding)
                return;

            if (!isArmed)
            {
                if (stats.vida > 0 && canTakeDamage)
                {
                    TakeDamage(30);
                    StartCoroutine(Invulnerability());
                }
                else if (stats.vida <= 0)
                {
                    Zombies script = other.GetComponent<Zombies>();
                    if (script != null)
                    {
                        script.StopEating();
                        StartCoroutine(Destruction());
                    }
                }
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea);
            foreach (Collider c in colliders)
            {
                if (c.CompareTag("Zombie"))
                {
                    StartCoroutine(Explosion(c, gameObject));
                    isExploding = true;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            if (!isActive)
                return;

            if (isExploding)
                return;

            if (!isArmed)
            {
                if (stats.vida > 0 && canTakeDamage)
                {
                    TakeDamage(30);
                    StartCoroutine(Invulnerability());
                }
                else if (stats.vida <= 0)
                {
                    Zombies script = other.GetComponent<Zombies>();
                    if (script != null)
                    {
                        script.StopEating();
                        StartCoroutine(Destruction());
                    }
                }
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea);
            foreach (Collider c in colliders)
            {
                if (c.CompareTag("Zombie"))
                {
                    StartCoroutine(Explosion(c, gameObject));
                    isExploding = true;
                }
            }
        }
    }
}