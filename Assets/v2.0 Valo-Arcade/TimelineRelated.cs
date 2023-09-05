using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineRelated : MonoBehaviour
{
    [SerializeField] Transform destination1;
    [SerializeField] GameObject cypherClone;
    Camera cam;
    int stage = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 0)
        {
            if (Vector3.Distance(destination1.position, cam.transform.position) <= 3)
            {
                stage++;
                cypherClone.SetActive(true);
            }
        }
        else
        {

        }

    }
}
