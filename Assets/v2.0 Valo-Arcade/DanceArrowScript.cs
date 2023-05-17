using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceArrowScript : MonoBehaviour
{
    public float speed = 1;
    public float acceleration = 1;
    public bool isVertical = false;
    public static float allSpeed = 1;
    // Update is called once per frame

    float originalStartingSpeed = 0;
    private void Start()
    {
        originalStartingSpeed = speed;
    }

    //float t = 0;
    void Update()
    {
        if(!isVertical)
        {
            //t += Time.deltaTime;
            //speed += Mathf.Lerp(acceleration/2,acceleration*2, t) * Time.deltaTime;
            speed += acceleration * 1f * Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            allSpeed += acceleration * Time.deltaTime;
            transform.position += transform.forward * allSpeed * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if(!isVertical)
        {
            speed = originalStartingSpeed;
            //t = 0;
        }
    }
}
