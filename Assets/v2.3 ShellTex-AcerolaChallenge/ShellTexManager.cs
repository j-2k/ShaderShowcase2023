using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTexManager : MonoBehaviour
{
    [SerializeField] Mesh _mesh;
    MeshRenderer mr;
    [SerializeField] Shader _shellTexShader;

    [SerializeField] float maxHeight;
    [Range(0,128)][SerializeField] int density;


    float _MaxHeight;
    int _Density;

    [SerializeField] bool isUpdating = false;
    [SerializeField] GameObject[] sheets;

    // Start is called before the first frame update
    void Start()
    {
        _MaxHeight = maxHeight;
        _Density = density;
        sheets = new GameObject[density];

        Color randCol;
        GameObject quad;
        float heightOffset = 0;

        for (int i = 0; i < _Density; i++)
        {
            randCol = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
            quad = new GameObject("Shell Texture " + i);
            quad.transform.parent = transform;
            quad.transform.rotation = transform.rotation;

            if (i == 0) { heightOffset = 0; } else { heightOffset = _MaxHeight / i; }// i hate this solution sfm probably should just set the start outside&before the forloop.
            quad.transform.position = transform.position + new Vector3(0, heightOffset, 0);
            
            quad.AddComponent<MeshFilter>().mesh = _mesh;
            quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
            quad.GetComponent<Renderer>().material.SetColor("_Color", randCol);

            sheets[i] = quad;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdating)
        {
            if(_Density != density || _MaxHeight != maxHeight)
            {
                _Density = density;
                _MaxHeight = maxHeight;

                
                //handle changes here maybe through a array? plan is to just use a array or maybe a list since i want dynamic amount of obs /density
                for (int i = 0; i < _Density; i++)
                {

                }
            }
        }
    }



}
