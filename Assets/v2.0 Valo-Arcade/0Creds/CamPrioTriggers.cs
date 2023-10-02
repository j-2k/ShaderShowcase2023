using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamPrioTriggers : MonoBehaviour
{
    [SerializeField] CamPrioManager cinemachineManager;
    [SerializeField] int targetIndex;
    [SerializeField] bool triggerOnce;

    private void Start()
    {
        targetIndex = int.Parse(transform.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cinemachineManager.SetPriorityCamera(targetIndex);
            if(triggerOnce)
            {
                this.enabled = false;
            }
        }
        
    }

}
