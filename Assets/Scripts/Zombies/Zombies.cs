using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombies : MonoBehaviour // Clase general para los Zombies.
{
    public Stats stats;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip frozenClip;

    [Header("Animación")]
    public Animator anim;

    [Header("Stats Ofensivas")]
    public bool isEating = false;
    public bool isFrozen = false;
    float finalSpeed;

    private List<Material> originalMaterials = new List<Material>();
    float freezeDuration = 3f;

    [System.Serializable]
    public class Stats
    {
        public int vida, vidaMin, vidaMax;
        public void SetupVida(int vida, int vidaMin, int vidaMax, GameObject obj)
        {
            this.vidaMin = vidaMin = 0;
            this.vida = vida = vidaMax;
            this.vidaMax = vidaMax;
        }
    }

    public void Movement(float speed)
    {
        if (isFrozen)
        {
            finalSpeed = speed * 0.5f;
        }
        else
        {
            finalSpeed = speed;
        }
        
        if (!isEating)
        {
            transform.Translate(Vector3.forward * finalSpeed * Time.deltaTime);
        }
    }
    public virtual void TakeDamage(int dmg)
    {
        stats.vida -= dmg;
        if (stats.vida <= stats.vidaMin)
        {
            Destroy(gameObject);
        }
    }
    void OnDestroy() // Únicamente para que las oleadas puedan volver a lanzarse.
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Plant"))
        {
            isEating = true;
            anim.SetBool("isEating", true);
        }
        if (other.transform.CompareTag("Bullet"))
        {
            audioSource.PlayOneShot(hitClip);
            TakeDamage(20);           
        }
        if (other.transform.CompareTag("IceBullet"))
        {
            audioSource.PlayOneShot(hitClip);
            TakeDamage(20);
            FreezeZombie();
            if (!isFrozen)
            {
                audioSource.PlayOneShot(frozenClip);
                Debug.Log("Frozen");
            }
        }
        if (other.transform.CompareTag("LawnMower"))
        {
            TakeDamage(100000);
        }
    }

    public void StopEating()
    {
        isEating = false;
        anim.SetBool("isEating", false);
    }
    void FreezeZombie()
    {
        originalMaterials.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            originalMaterials.Add(renderer.material);
        }
        foreach (Material originalMaterial in originalMaterials)
        {
            Material blueMaterial = new Material(originalMaterial);
            blueMaterial.color = Color.blue;

            foreach (Renderer renderer in renderers)
            {
                renderer.material = blueMaterial;
            }
        }
        StartCoroutine(RestoreMaterial());
    }

    IEnumerator RestoreMaterial()
    {
        isFrozen = true;
        yield return new WaitForSeconds(freezeDuration);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = originalMaterials[i];
        }
        isFrozen = false;
    }
}
