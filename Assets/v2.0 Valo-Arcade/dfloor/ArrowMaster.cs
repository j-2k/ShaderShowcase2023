using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static Unity.VisualScripting.Metadata;

public class ArrowMaster : MonoBehaviour
{
    [SerializeField] Material[] arrowMat;
    [SerializeField] DanceFloorController dfc;
    Material floorMat;
    float t = 0;
    float timeMultiplier = 1;
    int children = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        dfc = GetComponentInParent<DanceFloorController>();
        floorMat = dfc.GetComponent<Renderer>().material;
        children = transform.childCount;
        arrowMat = new Material[children];
       for(int i = 0; i < children; i++)
       {
            arrowMat[i] = GetComponentsInChildren<Renderer>()[i].material;
       }
        //AllLerp(1 + p, l * 0.5f, 5 + l * 5);
    }

    float l = 0;//, p = 0;
    // Update is called once per frame
    void Update()// the if elses here are nasty and can be fixed with a state enum but im too lazy rn
    {
        t += Time.deltaTime * timeMultiplier;
        if (t <= 1)
        {
            //p = Mathf.PingPong(t, 1);
            //l = Mathf.Lerp(1, 0, t);
            //AllLerp(1.5f - l*0.5f, l*0.5f, 6+l*5);
            if(!isCustomLerped)
            {
                l = Mathf.Lerp(1, 0, t);
                AllLerp(1.5f - l*0.5f, l*0.5f, 6+l*5);
            }
            else
            {
                if (randomLerp)
                {
                    //l = Mathf.Lerp(1, 0, t);
                    l = Mathf.PingPong(t*2f, Mathf.SmoothStep(0,0.5f,t*2f));
                    AllLerp(1.5f - l * 0.5f, l * 0.5f, 6 + l * 5);

                }
                else 
                {
                    //i cant find a good way yet to make it all use just 1 lerp so i made 3 just to continue forward
                    //l = Mathf.Lerp(0, 1, t);
                    float lSize = Mathf.Lerp(startSize, _size, t);
                    float lStr = Mathf.Lerp(startStr, _str, t);
                    float lBloom = Mathf.Lerp(startBloom, _bloom, t);

                    AllLerp(lSize, lStr, lBloom);
                }
            }
        }
        else
        {
            if(randomLerp)
            {
                t = 0;
            }
            else
            {
                this.enabled = false;
            }

        }
    }

    public void StartArrowLerp(float timeToFade = 1)
    {
        t = 0;
        timeToFade = (timeToFade == 0) ? 1 : timeToFade;
        timeMultiplier = 1 / timeToFade;
    }

    float _size, _str, _bloom;

    public bool isCustomLerped = false, randomLerp = false;

    public void AllLerp(float size,float str, float bloom)
    {
        for (int i = 0; i < children; i++)
        {
            arrowMat[i].SetFloat("_Size", size);
            arrowMat[i].SetFloat("_ArrowStr", str);
            arrowMat[i].SetFloat("_Bloom", bloom);
        }
        floorMat.SetFloat("_HitLight", str*0.35f);
        floorMat.SetFloat("_Bloom", bloom*0.75f);
    }

    float startSize, startStr, startBloom;
    public void InitializeStartingVals()
    {
        startSize = arrowMat[0].GetFloat("_Size");
        startStr = arrowMat[0].GetFloat("_ArrowStr");
        startBloom = arrowMat[0].GetFloat("_Bloom");
    }

    public void TargetLerps(float newSize, float newStr, float newBloom)
    {
        randomLerp = false;
        InitializeStartingVals();
        _size = newSize;
        _str = newStr;
        _bloom = newBloom;
    }

    private void OnDisable()
    {
        isCustomLerped = false;
    }

    public void StartRandomControl()
    {
        isCustomLerped = true;
        randomLerp = true;
    }

    public void StopRandomLerp()
    {

    }

}
