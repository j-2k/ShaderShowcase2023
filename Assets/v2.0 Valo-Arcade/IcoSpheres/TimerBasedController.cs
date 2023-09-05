using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerBasedController : MonoBehaviour
{
    [SerializeField] Material matToControll;
    public bool isExploding = false;
    [SerializeField, Range(2, 4)] float timeScale = 3;
    bool oneRun = false;
    [SerializeField] ParticleSystem shinePS;
    public bool isDead = false;

    Vector3 originPos;
    Vector3 targetPos;
    float t = 0;
    float t2 = 0;
    public bool isBouncing = false;

    private void Start()
    {
        originPos = transform.position;
        targetPos = transform.position + Vector3.up * 0.5f;
    }

    public void UpdatePositions()
    {
        originPos = transform.position;
        targetPos = transform.position + Vector3.up * 0.5f;
    }


    // Update is called once per frame
    void Update()
    {
        
        if(t2 < 1 && isDead)
        {
            t2 += Time.deltaTime;
            transform.localScale = Vector3.one * t2;
        }
        else if(!isDead)
        {
            t2 = 0;
            transform.localScale = Vector3.one * t2;
        }

        

        if (isExploding)
        {
            t += Time.deltaTime * timeScale;

            float x = (Mathf.Pow(t, 2) + t) * 2;
            float t2 = t * x;
            transform.rotation = Quaternion.identity;
            matToControll.SetFloat("_DownPow", Mathf.Lerp(0, 2, x));
            matToControll.SetFloat("_UpPow", Mathf.Lerp(0, -2, t2));

            if (t2 > 1 && x > 1)
            {
                matToControll.SetFloat("_DownPow", 0);
                matToControll.SetFloat("_UpPow", 0);
                if (!oneRun)
                {
                    transform.position = transform.position - transform.up * 4;
                    oneRun = true;
                }
                transform.localScale -= (transform.localScale * 5f) * Time.deltaTime*2;
            }
        }
        else
        {
            transform.Rotate(30 * Time.deltaTime, 60 * Time.deltaTime, 90 * Time.deltaTime);
            t = 0;
            matToControll.SetFloat("_DownPow", 0);
            matToControll.SetFloat("_UpPow", 0);
            if (oneRun)
            {
                shinePS.Play();
                transform.position = originPos;
                transform.localScale = Vector3.one;
                oneRun = false;
            }
        }

        if(isBouncing)
        {
            if(isExploding)
            {
                isBouncing = false;
                isMultiBouncing = false;
            }
            BounceManager();

            if(isMultiBouncing)
            {
                MultiBounceManager();
            }
        }
    }

    [SerializeField] int bouncesAmount = 0;
    [SerializeField] float bSpeedMultiplier = 5;
    float bT = 0;
    void BounceManager()
    {
        if(bouncesAmount > 0)
        {
            bT += Time.deltaTime * bSpeedMultiplier;                                                //Thanks gpt for reminding me about cool pingpong function cuz i forgor abt it
            Vector3 lerpedPos = new Vector3(originPos.x, Mathf.SmoothStep(originPos.y, targetPos.y, Mathf.PingPong(bT, 1)), originPos.z);
            transform.position = lerpedPos;

            if(bT > 2)
            {
                bT = 0;
                bouncesAmount--;
            }
        }
    }

    public bool isMultiBouncing = false;
    void MultiBounceManager()
    {
        Vector3 movementVec = new Vector3(Random.Range(-1, 2), Random.Range(0, 2), Random.Range(-1, 2));
        transform.position += (movementVec * 2) * Time.deltaTime;
    }

    public void ControlBounce(int amountToAdd, float bounceSpeedMultiplier)
    {
        bouncesAmount += amountToAdd;
        bSpeedMultiplier = bounceSpeedMultiplier;
    }

    public void StopParticleEmitting()
    {
        shinePS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    public void StartUpsizing()
    {
        t2 = 0;
    }

}








    /*
        //matToControll.SetFloat("_DownPow", TimeToFinish(0,2,ref t,1));
        //result = TimeToFinish(0, 2, ref t, 5);
        //Debug.Log("Res: " + result + " | currTime: " + t);

        if(testBounce)
        {
            float timeElapsed = (Time.time - startTime) / duration;
            float bounceTime = Mathf.PingPong(timeElapsed, 1f / numberOfBounces);

            float smoothStepValue = Mathf.SmoothStep(0f, 1f, bounceTime);
            float currentHeight = Mathf.Lerp(0f, maxHeight, smoothStepValue);

            Vector3 newPosition = initialPosition + Vector3.up * currentHeight;
            transform.position = newPosition;

            if (timeElapsed >= 1f)
            {
                completedBounces++;
                startTime = Time.time;
                if (completedBounces >= numberOfBounces)
                {
                    testBounce = false; // Disable the script once all bounces are completed
                }
            }
        }

        if(newBounces)
        {
            SetNumberOfBounces(5);
        }
    }
    public bool newBounces = false;
    // Method to change the number of bounces during runtime
    public void SetNumberOfBounces(int bounces)
    {
        numberOfBounces = bounces;
        completedBounces = 0;
        startTime = Time.time;

        testBounce = true; // Enable the script again if it was disabled
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
    */