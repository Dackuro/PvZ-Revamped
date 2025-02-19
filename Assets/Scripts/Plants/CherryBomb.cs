using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CherryBomb : Plants
{
    public float explosionArea;
    public float lifeTime;
    void Start()
    {
        lifeTime = 3f;
        explosionArea = 6f;
        stats.SetupVida(1000, 0, 100, gameObject);  
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionArea);
            foreach (Collider c in colliders)
            {
                if (c.CompareTag("Zombie"))
                {
                    StartCoroutine(Explosion(c, gameObject));
                }
            }
        }      
    }
}