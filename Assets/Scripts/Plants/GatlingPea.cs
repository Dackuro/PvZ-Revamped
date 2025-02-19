using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPea : Plants
{
    [SerializeField] private float attackSpeed;
    bool isShooting;
    void Start()
    {
        attackSpeed = 1.5f;
        canTakeDamage = true;
        canShoot = true;
        stats.SetupVida(100, 0, 100, gameObject);
        shootScript = FindAnyObjectByType<ShootScript>();
    }
    void Update()
    {
        Detect();
        if (canShoot && !isShooting && zombieDetected)
        {
            StartCoroutine(QuadrupleShot());
            StartCoroutine(ShootTimer(attackSpeed));
        }
    }
    IEnumerator QuadrupleShot()
    {
        isShooting = true;
        Shoot(gameObject);
        yield return new WaitForSeconds(0.1f);
        Shoot(gameObject);
        yield return new WaitForSeconds(0.1f);
        Shoot(gameObject);
        yield return new WaitForSeconds(0.1f);
        Shoot(gameObject);
        isShooting = false;
    }
}
