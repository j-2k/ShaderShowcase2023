using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfRanger : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform end;
    [SerializeField] float speed;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position +=  transform.forward * speed * Time.deltaTime;
        t += Time.deltaTime;
        transform.position = Vector3.Lerp(start.position, end.position, t);
        if (t > 1)
        {
            t = 0;
        }
    }
}
