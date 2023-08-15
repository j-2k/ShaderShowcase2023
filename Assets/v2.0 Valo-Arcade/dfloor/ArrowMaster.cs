using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    float p = 0, l = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * timeMultiplier;
        if (t <= 1)
        {
            p = Mathf.PingPong(t, 1);
            l = Mathf.Lerp(1, 0, t);
            AllLerp(1 + p, l*0.5f, 6+l*5);
        }
        else
        {
            this.enabled = false;
        }
    }

    public void StartArrowLerp(float timeToFade = 1)
    {
        t = 0;
        timeToFade = (timeToFade == 0) ? 1 : timeToFade;
        timeMultiplier = 1 / timeToFade;
    }

    public void AllLerp(float size,float str, float bloom)
    {
        for (int i = 0; i < children; i++)
        {
            arrowMat[i].SetFloat("_Size", size);
            arrowMat[i].SetFloat("_ArrowStr", str);
            arrowMat[i].SetFloat("_Bloom", bloom);
        }
        floorMat.SetFloat("_HitLight", str*0.5f);
        floorMat.SetFloat("_Bloom", bloom);
    }

    
}
