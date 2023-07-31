using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPairMats : MonoBehaviour
{
    public Material lightBase;
    public Material lightShaft;

    LightPairMats()
    {
        lightBase = transform.GetChild(0).GetComponent<Renderer>().material;
        lightShaft = transform.GetChild(1).GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
