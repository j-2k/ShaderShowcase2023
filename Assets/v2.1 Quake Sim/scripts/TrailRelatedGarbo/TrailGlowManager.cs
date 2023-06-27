using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailGlowManager : MonoBehaviour
{
    //i dont like this but method but whatever
    [SerializeField] SkinnedMeshRenderer smr;   //index 6 - 11
    [SerializeField] Material fresnelMat;
    Material copyFresnelMat;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            smr.SetBlendShapeWeight(i + 6, TrailEffect.smrIndices[i]);
        }
        copyFresnelMat = new Material(fresnelMat);
        smr.material = copyFresnelMat;
    }
    float t = 0;
    // Update is called once per frame
    void Update()
    {
        if(t >= 1)
        {
            Destroy(gameObject);
        }
        else
        {
            copyFresnelMat.SetFloat("_Cutoff", t);
            t += (TrailEffect.fadeTimeMultiplier * Time.deltaTime);
        }
    }
}
