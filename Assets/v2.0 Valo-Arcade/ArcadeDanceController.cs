using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArcadeDanceController : MonoBehaviour
{
    [SerializeField] GameObject danceArrowFab;
    [SerializeField] GameObject icoSphereFab;
    [SerializeField] DanceStages currentEnum;
    [SerializeField] ParticleSystem edSplash;
    [SerializeField] List<GameObject> centerModels;

    //float speed;
    [SerializeField] float directionMagnitude = 5; //def 5
    //[SerializeField] float acceleration = 12;//def 12
    [SerializeField] float edTimeOffset = 0.05f;//def 0.05f
    [SerializeField] float allSpeedAmount = 20;//def 20

    [Header("Horizontal Arrows:")]
    [SerializeField] float HA_Speed = 0f;
    [SerializeField] float HA_Accel = 1f;
    [SerializeField] float genOffset = 0.5f;

    [Header("Might wanna ignore these:")]
    [SerializeField] Material deformShaderMat;
    [SerializeField] List<GameObject> horizontalArrowsList;
    [SerializeField] List<DanceArrowScript> horizontalArrowsListScripts;
    [SerializeField] float angleTheta;
    [SerializeField] int maxEDDanceArrows;
    [SerializeField] List<GameObject> edDanceArrowsList;
    Vector3 transformPos;
    Vector3 dir;

    GameObject MainDanceArrow;
    GameObject MainIcoSphere;

    enum DanceStages {
        EMPTY,
        Generate,
        Waiting, //this approach for waiting is overkill (check implementation below) but whatever it works quickly.
        //Playing,
        Ending,
    }

    float originalEDTimeOffset = 0;
    float originalGenOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < centerModels.Count; i++)
        {
            centerModels[i].SetActive(false);
        }
        centerModels[0].SetActive(true);

        originalEDTimeOffset = edTimeOffset;
        originalGenOffset = genOffset;
        if (maxEDDanceArrows == 0)
        {
            maxEDDanceArrows = 20;
        }
        //speed = 1;
        transformPos = transform.position + Vector3.up * 1.0f;
        currentEnum = DanceStages.EMPTY;
        if(deformShaderMat == null)
        {
            deformShaderMat = transform.GetChild(0).Find("Surface").GetComponent<Renderer>().material;
        }
        

        DanceArrowScript.allSpeed = allSpeedAmount;

        //GENERATING FOR HORIZONTAL STAGE
        for (int i = 0; i < 5; i++)
        {
            //dir = GetRandomDirection() * directionMagnitude;
            //GameObject arrowH = GameObject.Instantiate(danceArrowFab, transformPos + dir,
            //    Quaternion.LookRotation(transformPos - (transformPos + dir)));
            GameObject arrowH = GameObject.Instantiate(danceArrowFab, Vector3.zero,
                Quaternion.identity);
            DanceArrowScript arrowScript = arrowH.GetComponent<DanceArrowScript>();
            arrowScript.speed = HA_Speed;
            arrowScript.acceleration = HA_Accel;

            arrowH.SetActive(false);
            horizontalArrowsList.Add(arrowH);
            horizontalArrowsListScripts.Add(arrowScript);
        }

        dir = Vector3.up * directionMagnitude;
        //GENERATING FOR ENDING STAGE
        for (int i = 0; i < maxEDDanceArrows; i++)
        {
            GameObject endingArrow = Instantiate(danceArrowFab, transformPos + dir,
            Quaternion.LookRotation(transformPos - (transformPos + dir)));
            endingArrow.transform.localScale = (Vector3.one * (((i * 0.1f) * 0.8f) + 0.4f));
            endingArrow.GetComponent<DanceArrowScript>().isVertical = true;
            
            endingArrow.SetActive(false);
            edDanceArrowsList.Add(endingArrow);
        }

        MainIcoSphere = Instantiate(icoSphereFab, transformPos + dir, Quaternion.identity);

    }



    // Update is called once per frame
    void Update()
    {
        InputHandling();
        transformPos = transform.position + Vector3.up * 1.0f;
        DebugDirectionRay();
        ShaderHandler();
        EnumDanceStates();

    }

    void InputHandling()
    {
        if(Input.GetKeyDown(KeyCode.Space) && currentEnum == DanceStages.EMPTY)
        {
            currentEnum = DanceStages.Generate;
        }
    }

    void DebugDirectionRay()
    {
        Debug.DrawRay(transformPos, dir, Color.green);
    }

    void ShaderHandler()
    {
        deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time * 2, 1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));
    }

    int stateIterator = 0;
    float maxTime = 0;
    int edArrowIndex = 0;

    float maxGenTime = 0;

    void EnumDanceStates()
    {
        switch (currentEnum)
        {
            case DanceStages.EMPTY:
                stateIterator = 0;
                ResetArrows();
                Debug.Log("EMPTY");
                break;


            case DanceStages.Generate:
                Debug.Log("Generate");
                //OldGen();
                if(Time.time > maxGenTime)
                {
                    if (stateIterator >= 5)
                    {
                        genOffset = originalGenOffset;
                        
                        currentEnum = DanceStages.Waiting;
                        break;
                    }
                    maxGenTime = Time.time + genOffset;
                    genOffset = genOffset * 0.9f - 0.012f;
                    //genOffset -= 0.09f;

                    //Reposition
                    dir = GetRandomDirection() * directionMagnitude;
                    horizontalArrowsList[stateIterator].transform.position = transformPos + dir;
                    horizontalArrowsList[stateIterator].transform.rotation = Quaternion.LookRotation(transformPos - (transformPos + dir));
                    horizontalArrowsList[stateIterator].SetActive(true);
                    horizontalArrowsListScripts[stateIterator].StartRotScaleRoutine();
                    stateIterator++;
                }

                DistanceCheck();
                break;

            case DanceStages.Waiting:
                Debug.Log("Waiting");
                //int checkListAmount = 0;
                DistanceCheck();

                bool shouldBreak = false;
                for (int i = 0; i < horizontalArrowsList.Count; i++)
                {
                    if (!horizontalArrowsList[i].activeSelf)
                    {
                        Debug.Log("Continue Waiting");
                        continue;
                    }
                    else
                    {
                        Debug.Log("Break Waiting");
                        shouldBreak = true;
                        break;
                    }
                    /*if (!horizontalArrowsList[i].activeSelf)
                    {
                        checkListAmount++;
                    }*/
                }

                if(!shouldBreak)
                {
                    currentEnum = DanceStages.Ending;
                }

                /*
                if (checkListAmount == horizontalArrowsList.Count)
                {
                    currentEnum = DanceStages.Ending;
                }*/
                break;

                /*
            case DanceStages.Playing:
                Debug.Log("Playing");
                //speed += acceleration * Time.deltaTime;
                //MainDanceArrow.transform.position += MainDanceArrow.transform.forward * speed * Time.deltaTime;
                if (Vector3.Distance(MainDanceArrow.transform.position, transformPos) < 0.5)
                {
                    MainDanceArrow.SetActive(false);
                    //speed = 1;
                    stateIterator++;
                    if (stateIterator >= 5)
                    {
                        currentEnum = DanceStages.Ending;
                    }
                    else
                    {
                        currentEnum = DanceStages.Generate;
                    }
                }
                break;
                */

            case DanceStages.Ending:
                Debug.Log("Ending");
                //create 20 arrows and try to send them all down (think of logic now implement later)

                //speed = acceleration;
                if (Time.time >= maxTime && edArrowIndex < maxEDDanceArrows)
                {
                    edDanceArrowsList[edArrowIndex].SetActive(true);

                    //rotatingdownarrows to face cam
                    Vector3 camDir = (Camera.main.transform.position - transform.position).normalized;
                    camDir.y = 0; //XZ plane only
                    float zRot = Vector3.SignedAngle(camDir, transform.right, transform.up);
                    edDanceArrowsList[edArrowIndex].transform.rotation = Quaternion.Euler(90, 0, zRot);
                    edArrowIndex++;
                    maxTime = Time.time + edTimeOffset;
                    //edTimeOffset -= 0.005f;//-= 0.003f;
                }

                for (int i = 0; i < maxEDDanceArrows; i++)
                {
                    if (!edDanceArrowsList[i].activeSelf){continue;}
                    Transform currArrow = edDanceArrowsList[i].transform;
                    //NEXT PROBLEM IS SPEED HAS TO BE UNIQUE FOR EVERY ARROW
                    //currArrow.position += currArrow.transform.forward * speed * Time.deltaTime;

                    if (Vector3.Distance(currArrow.position, transformPos) < 0.5)
                    {
                        //speed = 1;
                        if(i == 0)
                        {
                            edSplash.Play();
                        }
                        currArrow.gameObject.SetActive(false);
                        if(currArrow == edDanceArrowsList[maxEDDanceArrows-1].transform)
                        {
                            edArrowIndex = 0;
                            maxTime = 0;
                            //speed = 1;
                            areArrowsReset = true;
                            edTimeOffset = originalEDTimeOffset;
                            currentEnum = DanceStages.EMPTY;
                        }
                    }
                }
                break;


            default:
                Debug.Log("DEFAULT ENUM");
                break;
        }
    }

    Vector3 GetRandomDirection(bool isNormalized = true)
    {
        float randomRadian = Random.Range(0, Mathf.PI * 2);
        Vector3 direction = new Vector3(Mathf.Sin(randomRadian), 0, Mathf.Cos(randomRadian));
        if (Vector3.Angle(dir, direction) <= 90)
        {
            direction.x *= -1;
            direction.z *= -1;
        }

        if(isNormalized)
        {
            return Vector3.Normalize(direction);
        }
        else
        {
            return direction;
        }
    }

    Vector3 SetDirectionFromAngle(float angle)
    {
        Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
        Debug.Log(direction);
        return Vector3.Normalize(direction);
    }

    bool areArrowsReset = true;
    void ResetArrows()
    {
        if (areArrowsReset)
        {
            dir = Vector3.up * directionMagnitude*0.9f;
            for (int i = 0; i < edDanceArrowsList.Count; i++)
            {
                edDanceArrowsList[i].transform.position = transformPos + dir;
            }
            DanceArrowScript.allSpeed = allSpeedAmount;
            areArrowsReset = false;
        }
    }
    /*
    void OldGen()
    {
        dir = GetRandomDirection() * directionMagnitude;
        if (MainDanceArrow == null)
        {
            MainDanceArrow = GameObject.Instantiate(danceArrowFab, transformPos + dir,
                Quaternion.LookRotation(transformPos - (transformPos + dir)));
        }
        else
        {
            MainDanceArrow.transform.position = transformPos + dir;
            MainDanceArrow.transform.rotation = Quaternion.LookRotation(transformPos - (transformPos + dir));
            MainDanceArrow.SetActive(true);
        }
        currentEnum = DanceStages.Playing;
    }
    */

    void DistanceCheck()
    {
        for (int i = 0; i < horizontalArrowsList.Count; i++)
        {
            if (!horizontalArrowsList[i].activeSelf) { continue; }
            if (Vector3.Distance(horizontalArrowsList[i].transform.position, transformPos) < 0.5f)
            {
                horizontalArrowsListScripts[i].trailingOrbs.transform.SetParent(null);
                horizontalArrowsListScripts[i].trailingOrbs.Play();
                horizontalArrowsList[i].gameObject.SetActive(false);
            }
        }
    }
}
