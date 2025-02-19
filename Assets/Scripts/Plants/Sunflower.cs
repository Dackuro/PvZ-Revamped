using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plants
{
    Vector3 position;
    void Start()
    {
        canTakeDamage = true;
        stats.SetupVida(100, 0, 100, gameObject);

        position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        InvokeRepeating("SunCall", Random.Range(8f, 9f), Random.Range(8f, 9f));
    }
    void SunCall()
    {
        GenerateSun(position);
    }
}
