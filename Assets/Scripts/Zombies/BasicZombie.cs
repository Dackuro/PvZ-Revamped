using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombie : Zombies // Zombie básico
{
    [SerializeField] private float movementSpeed = 0.7f;

    void Start()
    {
        anim = GetComponent<Animator>();
        stats.SetupVida(200, 0, 200, gameObject);
        isEating = false;
        isFrozen = false;
    }
    void Update()
    {
        Movement(movementSpeed);
    }
}
