using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class subo : MonoBehaviour
{
    public Action<subo> esconderme;
    Rigidbody rgb;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.tag == "Weapon")
        {
            
            esconderme?.Invoke(this);
            gameObject.SetActive(false);
            Destroy(gameObject,2);
        }
    }
}
