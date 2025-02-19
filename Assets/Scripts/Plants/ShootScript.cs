using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    private bool canShoot = true;
    Vector3 spawnPos;

    public void Shoot(GameObject bulletPrefab, GameObject origin)
    {
        spawnPos = origin.transform.position;
       
        if (canShoot)
        {
            GameObject bala = PoolManager.Instance.RequestPrefab(bulletPrefab);
            if (bala != null)
            {
                bala.transform.position = spawnPos;
            }
            else
            {
                Debug.LogWarning("No se pudo obtener el prefab de bala.");
            }
        }
    }
}