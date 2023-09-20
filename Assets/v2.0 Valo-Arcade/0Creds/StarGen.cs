using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGen : MonoBehaviour
{
    [SerializeField] GameObject starObject;
    [SerializeField] int AmountOfStars = 10;
    Ray curRay;

    // Start is called before the first frame update
    void Start()
    {
        curRay.origin = transform.position;
        curRay.direction = transform.forward * 10;
        
    }


    // Update is called once per frame
    void Update()
    {
        //curRay.origin = transform.position;
        //curRay.direction = transform.forward * 10;
        //Debug.DrawRay(curRay.origin, curRay.direction * 10, Color.green);

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.transform.position, 1);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(this.transform.position, Vector3.forward*10);
    }
}
