using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamPrioTriggers : MonoBehaviour
{
    [SerializeField] CamPrioManager cinemachineManager;
    [SerializeField] int targetIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cinemachineManager.SetPriorityCamera(targetIndex);
        }
    }

}
