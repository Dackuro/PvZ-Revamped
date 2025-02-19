using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinSunflower : Plants
{
    Vector3 position;
    void Start()
    {
        canTakeDamage = true;
        stats.SetupVida(100, 0, 100, gameObject);

        position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        InvokeRepeating("DoubleSunCall", Random.Range(8f, 9f), Random.Range(8f, 9f));
    }
    void DoubleSunCall()
    {
        StartCoroutine(DoubleSun());
    }
    IEnumerator DoubleSun()
    {
        GenerateSun(position);
        yield return new WaitForSeconds(0.2f);
        GenerateSun(position);
    }
}
