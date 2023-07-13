using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TimerBasedController : MonoBehaviour
{
    [SerializeField] Material matToControll;
    public bool isExploding;
    [SerializeField,Range(1,3)] float timeScale = 1;
    bool oneRun = false;


    Vector3 coreOriginPos;
    Vector3 originPos;
    Vector3 targetPos;
    [SerializeField] bool testBounce;
    private void Start()
    {
        coreOriginPos = transform.position;
        originPos = transform.position;
        targetPos = transform.position + Vector3.up * 0.5f;
    }

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
                transform.localScale -= (transform.localScale * 5f) * Time.deltaTime;
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
                transform.localScale = Vector3.one;
                oneRun = false;
            }
        }


        //matToControll.SetFloat("_DownPow", TimeToFinish(0,2,ref t,1));
        //result = TimeToFinish(0, 2, ref t, 5);
        //Debug.Log("Res: " + result + " | currTime: " + t);

        if(testBounce)
        {
            bTime += Time.deltaTime * 4;
            if(bTime > 1.0f)
            {
                Vector3 tempPos = originPos;
                originPos = targetPos;
                targetPos = tempPos;
                bTime = 0;
            }
            Bounce(bTime);
        }
        else
        {

        }
    }
    float bTime = 0;


    [SerializeField] float t = 0;
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


    public int bouncesInQueue = 4;
    public bool finishedBounce = false;
    public bool isBouncing = false;
    public void Bounce(float timerSpeed)
    {
        //transform.position = Vector3.Lerp(originPos, targetPos, timerSpeed);
        float bounceAmt = Mathf.SmoothStep(originPos.y, targetPos.y, timerSpeed);
        if(isBouncing)
        {
            transform.position = new Vector3(transform.position.x, bounceAmt, transform.position.z);
        }
        //everything under here is hell, i could have checked if it was going up or down & decided but the outcome would be similar
        if (Vector3.Distance(coreOriginPos, transform.position) < 0.05f)
        {
            finishedBounce = true;
        }
        else
        {
            finishedBounce = false;
        }

        if(finishedBounce)
        {
            if(!runOnce)
            {
                if(bouncesInQueue > 0)
                {
                    bouncesInQueue -= 1;
                    isBouncing = true;
                }
                else
                {
                    isBouncing = false;
                }
                runOnce = true;
            }
        }
        else
        {
            runOnce = false;
        }
    }
    public bool runOnce = false;
}
