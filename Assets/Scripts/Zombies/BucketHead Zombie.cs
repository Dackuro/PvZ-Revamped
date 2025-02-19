using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketHeadZombie : Zombies // Zombie CaraCubo
{
    [SerializeField] private float movementSpeed = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        stats.SetupVida(800, 0, 200, gameObject);
        isEating = false;
        isFrozen = false;
    }
    void Update()
    {
        Movement(movementSpeed);
    }
}
