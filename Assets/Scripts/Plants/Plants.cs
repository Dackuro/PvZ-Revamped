using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Plants : MonoBehaviour
{  
    [Header("Datos")]
    public Stats stats;

    public Vector3Int gridPos;
    private GridCell plantData;
    private ObjectPlacer objectPlacer;
    private PlacementSystem placementSystem;

    public bool isActive;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip deathClip;

    [Header("Plantas Ofensivas")]
    public GameObject bulletPrefab;
    public ShootScript shootScript;
    public bool canShoot = true, zombieDetected;

    public bool isExploding = false;
    public ParticleSystem explosionParticles;

    [Header("Plantas Defensivas")]
    [SerializeField] GameObject prefabFullHP;
    [SerializeField] GameObject prefabHalfHP;
    [SerializeField] GameObject prefabLowHP;
    GameObject currentModel;

    public bool isDestroyed;
    public bool canTakeDamage = true;

    [SerializeField] MeshRenderer explosionSphere;


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

    private void Awake()
    {
        isActive = true;
        isDestroyed = false;
        GetComponent<Collider>().enabled = true;

        objectPlacer = FindAnyObjectByType<ObjectPlacer>();
        placementSystem = FindAnyObjectByType<PlacementSystem>();
        if (placementSystem != null)
        {
            plantData = placementSystem.plantData;
        }       
    }

    public void GenerateSun(Vector3 position)
    {
        if (isActive)
        {           
            GameObject newSun = Instantiate(bulletPrefab, position, Quaternion.identity);
            newSun.GetComponent<SunScript>().onGround = true;
        }
    }
    public void Detect()
    {    
        if (isActive)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            float dist = 55f;
            int layerMask = LayerMask.GetMask("Zombie", "Wall");
            if (Physics.Raycast(ray, out hit, dist, layerMask))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.tag == "Zombie")
                {
                    zombieDetected = true;
                }
                else
                {
                    zombieDetected = false;
                }
            }          
        } 
    }
    public void Shoot(GameObject origin)
    {
        audioSource.PlayOneShot(shootClip);
        shootScript.Shoot(bulletPrefab, origin);
    }
    public void UpdateModel()
    {
        if (stats.vida > stats.vidaMax / 2)
        {
            SetModel(prefabFullHP);
        }
        else if (stats.vida <= stats.vidaMax / 2 && stats.vida > stats.vidaMax / 4)
        {
            SetModel(prefabHalfHP);
        }
        else if (stats.vida <= stats.vidaMax / 4 && stats.vida > 0)
        {
            SetModel(prefabLowHP);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    void SetModel(GameObject modelPrefab)
    {
        if (currentModel != null)
        {
            currentModel.SetActive(false);
        }
        modelPrefab.SetActive(true);
        currentModel = modelPrefab;
    }
    public IEnumerator ShootTimer(float t)
    {
        canShoot = false;
        yield return new WaitForSeconds(t);
        canShoot = true;
    }
    public IEnumerator Explosion(Collider other, GameObject explosive)
    {
        MonoBehaviour script = other.GetComponent<MonoBehaviour>();
        if (script != null)
        {
            explosionParticles.Play();
            audioSource.PlayOneShot(deathClip);

            StartCoroutine(CreateExplosionArea(1.5f));

            System.Reflection.MethodInfo method = script.GetType().GetMethod("TakeDamage", new System.Type[] { typeof(int) });
            if (method != null)
            {
                method.Invoke(script, new object[] { 10000 });
            }
            explosive.GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(Destruction());
        }        
    }
    
    public IEnumerator CreateExplosionArea(float t)
    {
        explosionSphere.enabled = true;

        float waitTime = 0f;
        float rotationsPerMinute = 10f;

        while (waitTime < t)
        {
            transform.Rotate(0, 6 * rotationsPerMinute * Time.deltaTime, 0);
            waitTime += Time.deltaTime;
            yield return null;
        }

        explosionSphere.enabled = false;
    }

    public virtual void TakeDamage(int dmg)
    {
        stats.vida -= dmg;
        StartCoroutine(Invulnerability());
        if (stats.vida <= 0)
        {
            StartCoroutine(Destruction());
        }      
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Zombie"))
        {
            if (canTakeDamage && !isDestroyed)
            {
                TakeDamage(30);
            }

            if (stats.vida <= 0)
            {
                Zombies script = other.GetComponent<Zombies>();
                if (script != null)
                {
                    script.StopEating();
                }
            }
        }
    }
  
    public IEnumerator Destruction()
    {
        if (isDestroyed)
            yield break;        
        
        isDestroyed = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.3f);

        plantData.RemovePlantAt(gridPos);
        objectPlacer.RemoveObjectByDeath(gameObject);
    }

    public IEnumerator Invulnerability()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.75f);
        canTakeDamage = true;
    }
}
