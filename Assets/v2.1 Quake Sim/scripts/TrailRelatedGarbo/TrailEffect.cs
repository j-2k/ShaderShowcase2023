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
            if(t > 0.034f) //0.034 = 3 enabled @ one MAX
            {
                //Instantiate is really bad but idc for now. Subsititue for ObjectPool or just an enabling algo.
                GameObject currentTrail = Instantiate(trailingRanger,qm.transform.position + (Vector3.up * 1.25f) - qm.currentVelocityVector.normalized,qm.transform.rotation);
                GameObject.Destroy(currentTrail, 0.1f);
                t = 0;
            }
        }
    }


}
