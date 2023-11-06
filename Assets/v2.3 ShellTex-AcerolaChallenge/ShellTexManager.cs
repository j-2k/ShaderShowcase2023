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
        Color randCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1);

        GameObject quad = new GameObject("Shell Texture");
        quad.transform.parent = transform;
        quad.transform.rotation = transform.rotation;
        quad.AddComponent<MeshFilter>().mesh = _mesh;
        quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
        quad.GetComponent<Renderer>().material.SetColor("_Color", randCol);






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
