using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePea : Plants
{
    [SerializeField] private float attackSpeed;
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
        if (canShoot && zombieDetected)
        {
            Shoot(gameObject);
            StartCoroutine(ShootTimer(attackSpeed));
        }
    }
}
