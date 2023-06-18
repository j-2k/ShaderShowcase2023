using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailGlowManager : MonoBehaviour
{
    //i dont like this but method but whatever
    [SerializeField] SkinnedMeshRenderer smr;   //index 6 - 11
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            smr.SetBlendShapeWeight(i + 6, AnimationIndices.smrIndices[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
