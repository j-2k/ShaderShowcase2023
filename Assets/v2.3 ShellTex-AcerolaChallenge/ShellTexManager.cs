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
    [Range(128,512)][SerializeField] int densityHARDLIMIT;

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

            if (i == 0) { heightOffset = 0; } else { heightOffset = (i / (float)(_Density - 1)) * _MaxHeight; }// i hate this solution sfm probably should just set the start outside&before the forloop.
            quad.transform.position = transform.position + new Vector3(0, heightOffset, 0);
            
            quad.AddComponent<MeshFilter>().mesh = _mesh;
            quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
            quad.GetComponent<Renderer>().material.SetColor("_Color", randCol);

            sheets[i] = quad;
        }
    }

    void AddSheets(int i)
    {
        Color randCol;
        GameObject quad;

        randCol = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
        quad = new GameObject("Shell Texture " + i);
        quad.transform.parent = transform;
        quad.transform.rotation = transform.rotation;

        quad.AddComponent<MeshFilter>().mesh = _mesh;
        quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
        quad.GetComponent<Renderer>().material.SetColor("_Color", randCol);

        sheets[i] = quad;
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdating)
        {
            if(_Density != density || _MaxHeight != maxHeight)
            {
                Debug.Log("Something isnt equal. UPDATING...");
                //handle density, i think this is a really fast way? not really sure atleast i dont need to reinitialize new memory in arrays
                //& need to keep moving memory to new spaces if cap is reached,
                /*
                if (_Density < density)
                {
                    for (int i = _Density; i < density; i++)    //UPCASE
                    {
                        AddSheets(i);
                    }
                    _Density = density;
                }
                else if(_Density > density) 
                {
                    for (int i = _Density; i > density; i--)    //DOWNCASE
                    {
                        sheets[i] = null;
                    }
                    _Density = density;
                }*/

                _MaxHeight = maxHeight;

                //handle changes here maybe through a array? plan is to just use a array or maybe a list since i want dynamic amount of obs /density

                for (int i = 1; i < _Density; i++)
                {
                    sheets[i].transform.position = transform.position + new Vector3(0, (i / (float)(_Density - 1)) * _MaxHeight, 0);
                }
            }
        }
    }



}
