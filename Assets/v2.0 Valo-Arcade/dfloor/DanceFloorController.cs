using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloorController : MonoBehaviour
{
    [SerializeField] GameObject floorLightsHolder;
    //[SerializeField] GameObject[] setLights;          //Added these here for debug, However moved them to start function to save memory -
    //[SerializeField] GameObject[] lightPairsHolder;   //when exiting start function & i dont need the refs anymore (uncomment these for debugging)



    class LightPairMats//maybe could have used a dictionary here but this seems nicer
    {
        public Material lightBase;
        public Material lightShaft;
    }
    LightPairMats[] lpMats;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(lpMats.Length);
        for (int i = 0; i < lightPairsHolder.Length; i++)
        {
            LightPairMats lpm = new LightPairMats();
            lpm.lightBase = lightPairsHolder[i].transform.GetChild(0).GetComponent<Renderer>().material;
            lpm.lightShaft = lightPairsHolder[i].transform.GetChild(1).GetComponent<Renderer>().material;
            lpMats[i] = lpm;
        }
        Debug.Log(lpMats[0].lightBase + " " +  lpMats[0].lightShaft);
    }


    float t = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= 0.5)
        {
            //Test Lights here
        }
    }
}
