using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerBasedController : MonoBehaviour
{
    [SerializeField] Material matToControll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float result = 0;
    // Update is called once per frame
    void Update()
    {
        matToControll.SetFloat("_DownPow", TimeToFinish(0,2,ref t,1));
        //TimeToFinish(0, 2, ref t, 5);
        
        //Debug.Log("Res: " + result + " | currTime: " + t);
    }
    

    /*
    [SerializeField] float ct = 0;
    [SerializeField] float result = 0;
    void Update()
    {
        if (ct > 5)
        {
            isChangingDir = !isChangingDir;
        }
        else if(ct < 0)
        {
            isChangingDir = !isChangingDir;
        }

        if (!isChangingDir)
        {
            ct += Time.deltaTime;
        }
        else
        {
            ct -= Time.deltaTime;
        }

        result = Mathf.Lerp(0, 2, ct / 5);

        
    }
    */


    float t = 0;
    bool isChangingDir = false;
    float TimeToFinish(float start, float end, ref float currTime, float maxTime)
    {
        if(maxTime == 0) { return 0; }

        if (currTime > maxTime)
        {
            isChangingDir = !isChangingDir;
        }
        else if (currTime < 0)
        {
            isChangingDir = !isChangingDir;
        }

        if (!isChangingDir)
        {
            currTime += Time.deltaTime;
        }
        else
        {
            currTime -= Time.deltaTime;
        }

        //result = Mathf.Lerp(start, end, currTime / maxTime);
        return Mathf.Lerp(start, end, currTime / maxTime);
        //return currTime;
    }
}
