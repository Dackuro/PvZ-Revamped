using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeHeadZombie : Zombies // Zombie Caracono
{
    [SerializeField] private float movementSpeed = 0.6f;

    void Start()
    {
        anim = GetComponent<Animator>();
        stats.SetupVida(400, 0, 200, gameObject);
        isEating = false;
        isFrozen = false;
    }
    void Update()
    {
        Movement(movementSpeed);
    }
}
