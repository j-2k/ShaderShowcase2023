using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimerBasedController : MonoBehaviour
{
    [SerializeField] Material matToControll;
    public bool isExploding;
    [SerializeField,Range(1,3)] float timeScale = 1;
    bool oneRun = false;
    // Update is called once per frame
    void Update()
    {
        //TimeToFinish(0, 2, ref t, 1);
        if (isExploding)
        {
            t += Time.deltaTime * timeScale;

            float x = (Mathf.Pow(t, 2) + t) * 2;
            //float x = Mathf.Clamp01(Mathf.Log(t) + 1 + 0.1f);
            float t2 = t * x;

            matToControll.SetFloat("_DownPow", Mathf.Lerp(0,2, x));
            matToControll.SetFloat("_UpPow", Mathf.Lerp(0,-2, t2));

            if(t2 > 1 && x > 1)
            {
                matToControll.SetFloat("_DownPow", 0);
                matToControll.SetFloat("_UpPow", 0);
                if(!oneRun)
                {
                    transform.position = transform.position - transform.up * 4;
                    oneRun = true;
                }
            }
        }
        else
        {
            t = 0;
            matToControll.SetFloat("_DownPow", 0);
            matToControll.SetFloat("_UpPow", 0);
            if (oneRun)
            {
                transform.position = transform.position + transform.up * 4;
                oneRun = false;
            }
        }


        //matToControll.SetFloat("_DownPow", TimeToFinish(0,2,ref t,1));
        //result = TimeToFinish(0, 2, ref t, 5);
        //Debug.Log("Res: " + result + " | currTime: " + t);
    }


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
