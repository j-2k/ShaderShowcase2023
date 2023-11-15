using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTexManager : MonoBehaviour
{
    [Header("CURRENTLY NO GUARD FOR ARRAY OVERFLOW!")]

    [SerializeField] Mesh _mesh;
    MeshRenderer mr;
    [SerializeField] Shader _shellTexShader;
    [SerializeField] bool isRandomColors;
    [SerializeField] Color _shellTexColor;

    [SerializeField] float maxHeight;
    [Range(0,256)][SerializeField] int density;
    [Range(128, 256)][SerializeField] int densityHARDLIMIT;

    [Range(-3,1)][SerializeField] float thickness;

    float _MaxHeight;
    int _Density;

    float _Thick;

    [SerializeField] bool isUpdating = false;
    [SerializeField] GameObject[] sheets;//should probably change this to material array? cuz i keep getting the mat component kinda garbaging in update but shouldbe fine since its not every frame technically? idk maybe later

    // Start is called before the first frame update
    void Start()
    {
        //ignore this first line
        if(transform.GetChild(0).name == "PostionMarkerIgnore") { transform.GetChild(0).gameObject.SetActive(false); }

        _MaxHeight = maxHeight;
        _Density = density;
        _Thick = thickness;
        sheets = new GameObject[densityHARDLIMIT];

        GameObject quad;
        //float heightOffset = 0;
        float sheetIndexNormalized = 0;
        Material mat;

        for (int i = 0; i < _Density; i++)
        {

            
            quad = new GameObject("Shell Texture " + i);
            quad.transform.parent = transform;
            quad.transform.localScale = Vector3.one;
            quad.transform.rotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);

            //transform displacement old(was ignoring vertex displacement for abit to understand some stuff)
            //if (i == 0) { heightOffset = 0; } else { heightOffset = (i / (float)(_Density - 1)) * _MaxHeight; }// i hate this solution sfm probably should just set the start outside&before the forloop.
            //quad.transform.position = transform.position + new Vector3(0, heightOffset, 0);

            quad.AddComponent<MeshFilter>().mesh = _mesh;
            quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
            mat = quad.GetComponent<Renderer>().material;

            if (isRandomColors){_shellTexColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);}
            mat.SetColor("_Color", _shellTexColor);
            //vertex displacement new
            sheetIndexNormalized = (i / (float)(_Density - 1));
            mat.SetFloat("_SheetIndexNormalized", sheetIndexNormalized);
            mat.SetFloat("_Distance", _MaxHeight);
            mat.SetInt("_SheetIndex", i);
            mat.SetInt("_SheetDensity", _Density);
            mat.SetFloat("_Thick", _Thick);


            sheets[i] = quad;
        }
    }

    void AddSheets(int i)
    {
        GameObject quad;
        Material mat;

        quad = new GameObject("Shell Texture " + i);
        quad.transform.parent = transform;
        quad.transform.localScale = Vector3.one;
        quad.transform.rotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);

        quad.AddComponent<MeshFilter>().mesh = _mesh;
        quad.AddComponent<MeshRenderer>().material.shader = _shellTexShader;
        mat = quad.GetComponent<Renderer>().material;
        if (isRandomColors) { _shellTexColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1); }
        mat.SetColor("_Color", _shellTexColor);
        mat.SetFloat("_SheetIndexNormalized", (i / (float)(_Density - 1)));
        mat.SetFloat("_Distance", _MaxHeight);
        mat.SetInt("_SheetIndex", i);
        mat.SetInt("_SheetDensity", _Density);
        mat.SetFloat("_Thick", _Thick);

        sheets[i] = quad;
    }

    // Update is called once per frame
    void Update()
    {
        if(isUpdating)
        {
            if(_Density != density || _MaxHeight != maxHeight || _Thick != thickness)
            {
                Debug.Log("Something isnt equal. UPDATING...");
                //handle density, i think this is a really fast way? not really sure atleast i dont need to reinitialize new memory in arrays
                //& need to keep moving memory to new spaces if cap is reached,
                
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
                        Destroy(sheets[i-1]);
                        sheets[i - 1] = null;// not needed? just doing it since it says missing?
                    }
                    _Density = density;
                }

                _MaxHeight = maxHeight;
                _Thick = thickness;

                //handle changes here maybe through a array? plan is to just use a array or maybe a list since i want dynamic amount of obs /density

                for (int i = 1; i < _Density; i++)
                {
                    //old transform displacement
                    //sheets[i].transform.position = transform.position + new Vector3(0, (i / (float)(_Density - 1)) * _MaxHeight, 0);

                    //new vert displacement .. getting every frame is prob not ideal but whatever look next to the sheets arr comment for a possible fix
                    Material mat = sheets[i].GetComponent<Renderer>().material;
                    mat.SetFloat("_SheetIndexNormalized", (i / (float)(_Density - 1)));
                    mat.SetFloat("_Distance", _MaxHeight);
                    mat.SetInt("_SheetIndex", i);
                    mat.SetInt("_SheetDensity", _Density);
                    mat.SetFloat("_Thick", _Thick);
                }
            }
        }
    }



}
