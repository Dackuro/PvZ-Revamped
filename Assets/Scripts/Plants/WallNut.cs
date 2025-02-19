using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plants
{
    void Start()
    {
        canTakeDamage = true;
        stats.SetupVida(1200, 0, 1200, gameObject); 
        UpdateModel();
    }
    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        UpdateModel();
    }
}
