using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamPrioManager : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> vCams;

    [SerializeField] CinemachineVirtualCamera currVCAM;// 10 -> 5/100
    [SerializeField] int prevPrio;

    // Start is called before the first frame update
    void Start()
    {
        prevPrio = currVCAM.Priority;
    }

    /*
    [SerializeField] int indexTest = 0;
    // Update is called once per frame
    void Update()
    {
        //testing if this works properly.
        if(Input.GetKeyDown(KeyCode.O))
        {
            SetPriorityCamera(indexTest);
        }
    }
    */

    public void SetPriorityCamera(int index)
    {
        if(index >= vCams.Count || index < 0)
        {
            Debug.Log("INDEX NUMBER IS OUT OF BOUNDS");
            return;
        }
        CinemachineVirtualCamera newVcam = vCams[index];
        //Below here basically im just trying to restore the old previous priority number back into
        //the old vcam that is currently tracking, i could just keep the same numbers but i think this is nicer
        currVCAM.Priority = prevPrio;
        prevPrio = newVcam.Priority;
        newVcam.Priority = 100;
        currVCAM = newVcam;
    }

    
}
