using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTexManager : MonoBehaviour
{
    [SerializeField] Mesh _mesh;
    MeshRenderer mr;
    [SerializeField] Shader _shellTexShader;

    [SerializeField] float maxHeight;
    [SerializeField] int density;

    // Start is called before the first frame update
    void Start()
    {
        if(maxHeight == 0)//avoiding div by 0
        {
            maxHeight = 1;
        }

        Color randCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1);

        for(int i = 0; i < density; i++)
        {
            GameObject quad = new GameObject("Shell Texture " + i);
            quad.transform.parent = transform;
            quad.transform.rotation = transform.rotation;
            quad.transform.position = transform.position + new Vector3(0, maxHeight / i, 0);
            



            quad.AddComponent<MeshFilter>().mesh = _mesh;
            quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
            quad.GetComponent<Renderer>().material.SetColor("_Color", randCol);


        }





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
