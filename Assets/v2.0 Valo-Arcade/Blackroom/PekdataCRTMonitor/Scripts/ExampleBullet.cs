using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBullet : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 2, ForceMode.Impulse);bullet.GetComponent<Rigidbody>().useGravity = true;}
    }
}
