using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DanceFloorController : MonoBehaviour
{
    [SerializeField] GameObject floorLightsHolder;
    [SerializeField] ArrowMaster arrowControl;
    //[SerializeField] GameObject[] setLights;          //Added these here for debug, However moved them to start function to save memory -
    //[SerializeField] GameObject[] lightPairsHolder;   //when exiting start function & i dont need the refs anymore (uncomment these for debugging)

    /*
    class LightPairMats//maybe could have used a dictionary here but this seems nicer
    {
        public Material lightBase;
        public Material lightShaft;
    }
    */                  //NVM I forgot one small thing, i didnt think about how to manage the update for each alpha var (I want it to only update when the val is not 0 & disable it for optimization)
                        //so now I will change the whole structure to work on a script on the root of each lightpairholder
                        //so 80% of the start function is now useless LMFAO
                        //======
                        //I was going to think of another solution where i just forloop through the whole lightpair holders
                        //& update the values like that but i thought that would be bad? maybe im micro optimizing here since adding a
                        //whole script will likely result in the same result where it goes through all the objs updates???
                        //
                        //im goign to keep this mess here to i learn from my mistake in the future haha
    [SerializeField] LightPairMats[] lpMats;

    // Start is called before the first frame update
    void Start()
    {
        /*
        GameObject[] setLights;
        setLights = new GameObject[floorLightsHolder.transform.childCount];
        for (int i = 0; i < floorLightsHolder.transform.childCount; i++)
        {
            setLights[i] = floorLightsHolder.transform.GetChild(i).gameObject;
        }
        //setLights = floorLightsHolder.GetComponentsInChildren<Transform>();

        int maxSize = 0;//yes i could default this to (setLights.Length * 3) since each setlight has 3 but i wanted
                        // to prac dynamic cases / other niche cases
        for (int i = 0; i < setLights.Length; i++)
        {
            maxSize += setLights[i].transform.childCount;
        }

        GameObject[] lightPairsHolder;
        lightPairsHolder = new GameObject[maxSize];
        int indexLP = 0;
        for (int i = 0; i < setLights.Length; i++)
        {
            for (int j = 0; j < setLights[i].transform.childCount; j++)
            {
                lightPairsHolder[indexLP] = setLights[i].transform.GetChild(j).gameObject;
                indexLP++;
            }
        }

        lpMats = new LightPairMats[lightPairsHolder.Length];
        //Debug.Log(lpMats.Length);
        for (int i = 0; i < lightPairsHolder.Length; i++)
        {
            //lightPairsHolder[i].AddComponent<LightPairMats>(); <NO NEED TO DO THIS ANYMORE JUST ADD COMPONENT MANUALLY IS BETTER WILL SPEED LOADING TIME BY A LOT
            //^^ THIS MIGHT BE GOOD IN SPECIAL CASEs WHERE WE DONT HAVE COMPS ADDED TO CERTAIN OBJS & THE LOOPS ABOVE WILL HELP WITH IT
            
            */
        /*
        LightPairMats lpm = new LightPairMats();
        lpm.lightBase = lightPairsHolder[i].transform.GetChild(0).GetComponent<Renderer>().material;
        lpm.lightShaft = lightPairsHolder[i].transform.GetChild(1).GetComponent<Renderer>().material;
        lpMats[i] = lpm;

        }
        //Debug.Log(lpMats[0].lightBase + " " +  lpMats[0].lightShaft);
        */

        //NEW START METHOD condensed from 43 lines to 1 line 
        lpMats = GetComponentsInChildren<LightPairMats>();
        arrowControl = GetComponentInChildren<ArrowMaster>();
        currentFloorLightMode = FloorLightModes.Random;
    }

    float t = 0;
    public enum FloorLightModes
    {
        Off,
        Random,
        Circular
    }

    [SerializeField] FloorLightModes currentFloorLightMode;

    int curRand = 0;
    // Update is called once per frame
    void Update()
    {
        switch(currentFloorLightMode)
        {
            case FloorLightModes.Off:
                t = 0;
                break;
            case FloorLightModes.Random:
                LightRandomDouble();
                break;
            case FloorLightModes.Circular:
                LightCircular(circleLightTime, circleLightStep);
                break;
        }
    }
    
    public void LightAllOnce(float timeToTake = 1)
    {
        for (int i = 0; i < lpMats.Length; i++)
        {
            lpMats[i].enabled = true;
            lpMats[i].StartLightLerp(timeToTake);//USE ON ENABLE ONLY IF U WANT MORE RANDOMIZED LIGHT EFFECTS? (ITS A BUG)
        }
    }


    float circleLightTime = 0.4f;
    float circleLightStep = 0.03f;
    int indexCircular = 0;
    void LightCircular(float timeToTake = 0.25f,float stepTime = 0.05f)
    {
        t += Time.deltaTime;
        if(t >= stepTime)
        {
            if(indexCircular >= lpMats.Length)
            {
                indexCircular = 0;
            }

            for (; indexCircular < lpMats.Length;)
            {
                lpMats[indexCircular].enabled = true;
                lpMats[indexCircular].StartLightLerp(timeToTake);//USE ON ENABLE ONLY IF U WANT MORE RANDOMIZED LIGHT EFFECTS? (ITS A BUG)
                //2nd/double spiral below
                int secondSpiralIndex = indexCircular + (lpMats.Length / 2);
                if(secondSpiralIndex >= lpMats.Length)//wraparound handler
                {
                    secondSpiralIndex -= lpMats.Length;
                }
                lpMats[secondSpiralIndex].enabled = true;
                lpMats[secondSpiralIndex].StartLightLerp(timeToTake);
                break;
            }
            indexCircular++;
            t = 0;
        }
    }

    void LightRandomDouble()
    {
        t += Time.deltaTime;
        if (t >= 1)
        {
            for (int i = 0; i < 3; i++)
            {   //0 + (i * 6) = LOWERBOUND ARRAY LIGHT BRACKET THAT IS DIVISIBLE BY 3
                //6 + (i * 6) = UPPERBOUND ARRAY LIGHT BRACKET THAT IS DIVISIBLE BY 3
                //(i * 6) = lpmats.len / 3 = 6, 6 elements is the # of lights we go through in the total 3 loops 3*6 = 18
                curRand = Random.Range(0 + (i * 6), 6 + (i * 6));
                lpMats[curRand].enabled = true;
                lpMats[curRand].StartLightLerp();
                //Second Opposite Light Constant
                curRand += 3;
                if (curRand >= 6 + (i * 6))//wraparound handler
                {
                    curRand -= 6 + (i * 6);
                    curRand += 0 + (i * 6);
                }
                lpMats[curRand].enabled = true;
                lpMats[curRand].StartLightLerp();
            }
            t = 0;
            /*
            //Test Lights here
            int r = Random.Range(1, lpMats.Length);
            //steps = enable lp mat script and set alpha to 1
            for (int i = 0; i < r; i++)
            {
                lpMats[i].enabled = true;
                lpMats[i].StartLightLerp();//USE ON ENABLE ONLY IF U WANT MORE RANDOMIZED LIGHT EFFECTS? (ITS A BUG)
            }
            */
        }
    }

    public void ControlCircularLight(float timeToTake = 1, float stepTime = 0.25f)
    {
        circleLightTime = timeToTake;
        circleLightStep = stepTime;
    }

    public void ChangeCurrentLightMode(FloorLightModes targetLightMode)
    {
        currentFloorLightMode = targetLightMode;
    }

    public void BeginArrowLerp(float time = 0.5f)
    {
        arrowControl.enabled = true;
        arrowControl.StartArrowLerp(time);
    }

    public void TargetCustomLerps(float newSize, float newStr, float newBloom, float time = 0.5f)
    {
        BeginArrowLerp(time);
        arrowControl.TargetLerps(newSize, newStr, newBloom);
        arrowControl.isCustomLerped = true;
    }

    public void StartRandomLerps()
    {
        arrowControl.enabled = true;
        arrowControl.StartRandomControl();
    }

    public void DisableRandomLerps()
    {
        arrowControl.randomLerp = false;
        arrowControl.isCustomLerped = false;
    }
}
