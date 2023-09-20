using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGen : MonoBehaviour
{
    [SerializeField] GameObject starObject;
    [SerializeField] int AmountOfStars = 10;
    [SerializeField] float starScale = 1;

    [SerializeField] bool isCompleted = false;
    [SerializeField] int curStarCount = 0;
    [SerializeField] float phi;
    [SerializeField] float theta;
    [SerializeField] float radius;
    //Ray curRay;
    //GameObject sphere;
    // Start is called before the first frame update
    void Start()
    {
        /*
        curRay.origin = transform.position;
        curRay.direction = transform.forward * 10;
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        */

        StartCoroutine(StarGenRoutine());
    }

    Vector3 fin;
    //float t = 0;


    // Update is called once per frame
    void Update()
    {
        //curRay.origin = transform.position;
        //curRay.direction = transform.forward * 10;
        //Debug.DrawRay(curRay.origin, curRay.direction * 10, Color.green);

        /*
        fin = RotateVectorAll();
        t += Time.deltaTime;

        phi += Time.deltaTime * 1;
        theta += Time.deltaTime * 20;

        if (t >= 0.05)
        {
            t = 0;
            Instantiate(sphere, fin, Quaternion.identity);
        }*/
    }

    /*
    Vector3 RotateVectorAroundY(float yTheta)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * yTheta), 0, Mathf.Cos(Mathf.Deg2Rad * yTheta));
    }

    Vector3 RotateVectorAroundX(float xTheta)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * xTheta), Mathf.Cos(Mathf.Deg2Rad * xTheta), 0);
    }
    */

    Vector3 RotateVectorAll()
    {
        radius = Random.Range(100f, 200f);
        theta = Random.Range(0f, 360f);
        phi = Random.Range(0f, 360f);

        float sinTheta = Mathf.Sin(theta);
        float cosTheta = Mathf.Cos(theta);
        float sinPhi = Mathf.Sin(phi);
        float cosPhi = Mathf.Cos(phi);

        float x = sinPhi * cosTheta;
        float y = sinPhi * sinTheta;
        float z = cosPhi;
        return new Vector3(x,y,z) * radius;
    }

    IEnumerator StarGenRoutine()
    {
        
        isCompleted = false;
        for (int i = 0; i < AmountOfStars; i++)
        {
            curStarCount = i;
            Quaternion rot = Quaternion.Euler(theta,phi,radius);//<this is funny
            Instantiate(starObject, RotateVectorAll(), rot, this.transform).transform.localScale = Vector3.one * starScale;
            //yield return new WaitForSeconds(0.0001f);
        }
        isCompleted = true;
        yield return new WaitForSeconds(1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(this.transform.position, 1);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(this.transform.position, Vector3.forward*10);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(Vector3.zero, fin);
    }
}
