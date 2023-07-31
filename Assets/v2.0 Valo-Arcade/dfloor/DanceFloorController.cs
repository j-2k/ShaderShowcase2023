using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloorController : MonoBehaviour
{
    [SerializeField] GameObject floorLightsHolder;
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
    }


    float t = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= 1)
        {
            //Test Lights here
            int r = Random.Range(0, lpMats.Length);
            //steps = enable lp mat script and set alpha to 1
            for (int i = 0; i < r; i++)
            {
                lpMats[i].enabled = true;
                lpMats[i].StartLightLerp();//USE ON ENABLE ONLY IF U WANT MORE RANDOMIZED LIGHT EFFECTS? (ITS A BUG)
            }
            t = 0;
        }
    }


}
