using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationIndices : MonoBehaviour
{
    [SerializeField] Quake1Move qm;
    [SerializeField] SkinnedMeshRenderer smrREF;
    public static SkinnedMeshRenderer smr;
    public static float[] smrIndices = new float[6];  

    // Start is called before the first frame update
    void Start()
    {
        smr = smrREF;
        if (smr == null)
        {
            Debug.LogWarning("SKINNED MESH RENDERER IS MISSING!");
        }

        if(qm == null)
        {
            qm = GetComponentInParent<Quake1Move>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (qm.currentSpeed > 150 / 10)
        {
            for (int i = 0; i < 6; i++)
            {
                smrIndices[i] = smr.GetBlendShapeWeight(i + 6);
            }
        }
    }
}
