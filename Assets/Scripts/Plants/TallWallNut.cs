using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallWallNut : Plants
{
    void Start()
    {
        canTakeDamage = true;
        stats.SetupVida(2400, 0, 2400, gameObject);
        UpdateModel();
    }
    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        UpdateModel();
    }
}
