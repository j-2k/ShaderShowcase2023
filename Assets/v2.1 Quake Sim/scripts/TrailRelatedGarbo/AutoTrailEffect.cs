using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrailEffect : MonoBehaviour
{
    [SerializeField] float timeSpawnRateMS = 0.034f;
    public static float fadeTimeMultiplier = 3;

    [SerializeField] GameObject trailingRanger;
    [SerializeField] GameObject target;
    [SerializeField] SkinnedMeshRenderer smrREF;
    public static SkinnedMeshRenderer smr;
    public static float[] smrIndices = new float[6];

    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        smr = smrREF;
        if (smr == null)
        {
            Debug.LogWarning("SKINNED MESH RENDERER IS MISSING!");
        }
        rend = target.GetComponent<Renderer>();
    }
    Renderer rend;
    

    // Update is called once per frame
    void Update()
    {
        if(rend.isVisible)//200 * 0.1 //200/10
        {
            t += Time.deltaTime;
            for (int i = 0; i < 6; i++)//canbeoptimized
            {
                smrIndices[i] = smr.GetBlendShapeWeight(i + 6);
            }
            if(t > timeSpawnRateMS) //0.034 = 3 enabled @ one MAX//canbeoptimized
            {
                //Instantiate is really bad but idc for now. Subsititue for ObjectPool or just an enabling algo.
                GameObject ranger = Instantiate(trailingRanger, target.transform.position - target.transform.forward*0.2f, target.transform.rotation,target.transform);
                ranger.transform.parent = null;
                //GameObject.Destroy(currentTrail, 0.1f);
                t = 0;
            }
        }
    }


}
