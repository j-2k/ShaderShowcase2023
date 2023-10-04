using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsComputeShaderSupported : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Compute Shader Support : " + SystemInfo.supportsComputeShaders);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
