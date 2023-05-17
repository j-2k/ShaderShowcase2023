using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArcadeDanceController : MonoBehaviour
{
    [SerializeField] GameObject danceArrowFab;
    [SerializeField] DanceStages currentEnum;

    //float speed;
    [SerializeField] float directionMagnitude = 5; //def 5
    //[SerializeField] float acceleration = 12;//def 12
    [SerializeField] float edTimeOffset = 0.04f;//def 0.04f
    [SerializeField] float allSpeedAmount = 30;//def 30

    [Header("Might wanna ignore these:")]
    [SerializeField] Material deformShaderMat;
    [SerializeField] float angleTheta;
    [SerializeField] int maxEDDanceArrows;
    [SerializeField] List<GameObject> edDanceArrowsList;
    Vector3 transformPos;
    Vector3 dir;

    GameObject MainDanceArrow;

    enum DanceStages {
        EMPTY,
        Generate,
        Playing,
        Ending,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(maxEDDanceArrows == 0)
        {
            maxEDDanceArrows = 20;
        }
        //speed = 1;
        transformPos = transform.position + Vector3.up * 1.0f;
        currentEnum = DanceStages.EMPTY;
        deformShaderMat = transform.GetChild(0).Find("Surface").GetComponent<Renderer>().material;
        dir = Vector3.up * directionMagnitude;

        DanceArrowScript.allSpeed = allSpeedAmount;

        //GENERATING FOR ENDING STAGE
        for (int i = 0; i < maxEDDanceArrows; i++)
        {
            GameObject endingArrow = Instantiate(danceArrowFab, transformPos + dir,
            Quaternion.LookRotation(transformPos - (transformPos + dir)));
            endingArrow.transform.localScale = (Vector3.one * (((i * 0.1f) * 0.8f) + 1f));
            endingArrow.GetComponent<DanceArrowScript>().isUniversal = true;
            
            endingArrow.SetActive(false);
            edDanceArrowsList.Add(endingArrow);
        }
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
                break;


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


            case DanceStages.Ending:
                Debug.Log("Ending");
                //dir = Vector3.up * directionMagnitude;
                //MainDanceArrow.transform.position = transformPos + dir;
                //MainDanceArrow.transform.rotation = Quaternion.LookRotation(transformPos - MainDanceArrow.transform.position);
                //MainDanceArrow.SetActive(true);
                //create 20 arrows and try to send them all down (think of logic now implement later)

                //speed = acceleration;
                Time.timeScale = 0.5f;
                if (Time.time >= maxTime && edArrowIndex < maxEDDanceArrows)
                {
                    edDanceArrowsList[edArrowIndex].SetActive(true);
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
                        currArrow.gameObject.SetActive(false);
                        if(currArrow == edDanceArrowsList[maxEDDanceArrows-1].transform)
                        {
                            edArrowIndex = 0;
                            maxTime = 0;
                            //speed = 1;
                            areArrowsReset = true;
                            edTimeOffset = 0.04f;
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
            dir = Vector3.up * directionMagnitude*1f;
            for (int i = 0; i < edDanceArrowsList.Count; i++)
            {
                edDanceArrowsList[i].transform.position = transformPos + dir;
            }
            DanceArrowScript.allSpeed = allSpeedAmount;
            areArrowsReset = false;
        }
    }
}
