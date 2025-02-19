using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class LawnMowerScript : MonoBehaviour // Lógica de los cortacésped.
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip lawnMowerClip;

    [Header("Variables")]
    public float speed;
    public bool activated;
    public bool sound;

    private Transform basePos;
    void Start()
    {
        basePos = gameObject.transform;
        activated = false;
        sound = false;
        speed = 0.3f;
    }

    void Update()
    {
        if (activated)
        {
            if (!sound)
            {
                audioSource.PlayOneShot(lawnMowerClip);
                sound = true;
            }
            transform.position = Vector3.Lerp(basePos.position, new Vector3(150f ,basePos.position.y, basePos.position.z), speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            activated = true;
        }
    }
}
