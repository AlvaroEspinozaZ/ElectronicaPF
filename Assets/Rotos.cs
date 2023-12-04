using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotos : MonoBehaviour
{
    Rigidbody rgb;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        rgb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 2);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            rgb.useGravity = true;
        }
    }
}
