using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    [SerializeField] Quake1Move qm;
    [SerializeField] GameObject trailingRanger;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        qm = GetComponentInParent<Quake1Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if(qm.currentSpeed > 150/10)
        {
            t += Time.deltaTime;
            if(t > 0.25f)
            {
                //Instantiate is really bad but idc for now. Subsititue for ObjectPool or just an enabling algo.
                GameObject currentTrail = Instantiate(trailingRanger,qm.transform.position,qm.transform.rotation);
                GameObject.Destroy(currentTrail, 1);
                t = 0;
            }
        }
    }


}
