using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class BulletScript : MonoBehaviour
{
    float speed = 20f;

    private void OnEnable()
    {
        transform.position = Vector3.zero;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Zombie")
        {
            gameObject.SetActive(false);
        }
        else if (other.transform.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}