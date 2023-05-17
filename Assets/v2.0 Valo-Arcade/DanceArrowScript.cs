using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceArrowScript : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float acceleration = 1;
    public bool isUniversal = false;
    public static float allSpeed = 1;
    // Update is called once per frame

    void Update()
    {
        if(!isUniversal)
        {
            speed += acceleration * Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            allSpeed += acceleration * Time.deltaTime;
            transform.position += transform.forward * allSpeed * Time.deltaTime;
        }
    }
}
