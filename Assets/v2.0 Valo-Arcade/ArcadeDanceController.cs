using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArcadeDanceController : MonoBehaviour
{//Main Controller for the whole Kill Effect
    [SerializeField] GameObject danceArrowFab;
    [SerializeField] GameObject icoSphereFab;
    [SerializeField] DanceStages currentEnum;
    [SerializeField] ParticleSystem edSplash;
    [SerializeField] ParticleSystem ed2Splash;
    [SerializeField] List<GameObject> centerModels;
    [SerializeField] DanceFloorController danceFloorController;

    //float speed;
    [SerializeField] float directionMagnitude = 5; //def 5
    //[SerializeField] float acceleration = 12;//def 12
    [SerializeField] float edTimeOffset = 0.05f;//def 0.05f
    [SerializeField] float allSpeedAmount = 20;//def 20

    [Header("Horizontal Arrows:")]
    //[SerializeField] float HA_Speed = 0f;
    //[SerializeField] float HA_Accel = 1f;
    [SerializeField] float genOffset = 0.5f;

    [Header("Might wanna ignore these:")]
    [SerializeField] Material deformShaderMat;//_FresnelExponent (Insta 1, Lerp to 5)//_Strength (Insta 3 or 2, lerp to 0.5 or 0)
    [SerializeField] List<GameObject> horizontalArrowsList;
    [SerializeField] List<DanceArrowScript> horizontalArrowsListScripts;
    [SerializeField] float angleTheta;
    [SerializeField] int maxEDDanceArrows;
    [SerializeField] List<GameObject> edDanceArrowsList;
    Vector3 transformPos;
    Vector3 dir;

    GameObject MainDanceArrow;
    TimerBasedController MainIcoSphereController;
    [SerializeField] AudioSource testToAudio;
    

    enum DanceStages {
        EMPTY,
        Generate,
        Waiting, //this approach for waiting is overkill (check implementation below) but whatever it works quickly.
        //Playing,
        Ending,
        Ending2
    }

    Animator deathAnim;
    bool isDeadStatus = false;
    public void DeathAnimController(bool isStarting = true)
    {
        isDeadStatus = isStarting;
        deathAnim.SetBool("isDead", isDeadStatus);
        MainIcoSphereController.isDead = isDeadStatus;
        Invoke(nameof(StartDance), 0.75f);//instead of animation event im just going to do this cuz its fast.

        for (int i = 0; i < maxEDDanceArrows; i++)
        {
            dir = Vector3.up * directionMagnitude;
            edDanceArrowsList[i].transform.position = transformPos + dir;
        }
        MainIcoSphereController.transform.position = transformPos + dir;
        MainIcoSphereController.UpdatePositions();
    }

    float originalEDTimeOffset = 0;
    float originalGenOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        deathAnim = centerModels[0].GetComponent<Animator>();
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
        transformPos = transform.position + Vector3.up * 1f;
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
            //arrowScript.speed = HA_Speed;
            //arrowScript.acceleration = HA_Accel;

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

        MainIcoSphereController = Instantiate(icoSphereFab, transformPos + dir, Quaternion.identity).GetComponent<TimerBasedController>();
        MainIcoSphereController.isBouncing = true;
        danceFloorController.StartRandomLerps();
    }


    [SerializeField] KeyCode startAnimKey;
    // Update is called once per frame
    void Update()
    {
        InputHandling();
        transformPos = transform.position + Vector3.up * 2.0f;
        DebugDirectionRay();
        ShaderHandler();
        EnumDanceStates();
    }

    public bool isStartingAnim = false;

    void InputHandling()
    {
        if ((Input.GetKeyDown(startAnimKey) || isStartingAnim) && !isDeadStatus)
        {
            isStartingAnim = false;
            DeathAnimController(true);
        }
       
    }

    public void StartDance()
    {
        if (currentEnum == DanceStages.EMPTY && isDeadStatus)
        {
            currentEnum = DanceStages.Generate;
            danceFloorController.ChangeCurrentLightMode(DanceFloorController.FloorLightModes.Off);
            danceFloorController.DisableRandomLerps();
            if (!centerModels[0].activeSelf)
            {
                centerModels[0].SetActive(true);
                centerModels[centerModels.Count - 1].gameObject.SetActive(false);
            }

            if (testToAudio != null)
            {
                testToAudio.Play();
            }
        }
    }


    void DebugDirectionRay()
    {
        Debug.DrawRay(transformPos, dir, Color.green);
    }

    [SerializeField] float hardRotValueParent;//garbage implementation but im lazy to do the proper method

    float shaderT;
    float shaderSpeedT;
    void ShaderHandler()
    {
        if (currentEnum == DanceStages.Ending)
        {
            if (shaderT < 1)
            {
                //algo idea is for the spam visual use ping pong + add to base 
                shaderT += Time.deltaTime * shaderSpeedT;

                //deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time * 2, 1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));
                float multiplier = Mathf.Lerp(15, 30, shaderT * 3);
                deformShaderMat.SetFloat("_Strength", 0.5f + Mathf.PingPong(shaderT* multiplier, 2));
                deformShaderMat.SetFloat("_FresnelExponent", Mathf.Lerp(5, 0.2f,  shaderT));
                //deformShaderMat.SetFloat("_UpPow", Mathf.Lerp(0, -0.2f, shaderT));
                float baseMP = shaderT * 2 + (Mathf.PingPong(shaderT, 1)*2);
                deformShaderMat.SetFloat("_UpPow", Mathf.Lerp(0, -0.2f, baseMP));
            }
        }
        else
        {
            if (shaderT < 1)
            {
                shaderT += Time.deltaTime * shaderSpeedT * 2;

                //deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time * 2, 1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));
                deformShaderMat.SetFloat("_Strength", Mathf.Lerp(3, 0.1f, shaderT));
                deformShaderMat.SetFloat("_FresnelExponent", Mathf.Lerp(0.5f, 5, shaderT));
                deformShaderMat.SetFloat("_UpPow", Mathf.Lerp(-0.2f, 0, shaderT));
            }
        }
        /*if (shaderT < 1)
        {
            shaderT += Time.deltaTime * shaderSpeedT;

            //deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time * 2, 1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));
            deformShaderMat.SetFloat("_Strength", Mathf.Lerp(2, 0.1f, shaderT));
            deformShaderMat.SetFloat("_FresnelExponent", Mathf.Lerp(0.5f, 5, shaderT));
            deformShaderMat.SetFloat("_UpPow", Mathf.Lerp(-0.2f, 0, shaderT));
        }*/


    }

    int stateIterator = 0;
    float maxTime = 0;
    int edArrowIndex = 0;

    float maxGenTime = 0;

    float tempAccel = 0;

    float edTime = 0;
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
                        tempAccel = -3;
                        
                        currentEnum = DanceStages.Waiting;
                        break;
                    }
                    maxGenTime = Time.time + genOffset;
                    //genOffset = genOffset * 0.9f - 0.012f;
                    //genOffset -= 0.09f;
                    HardGenOffsets();

                    //Reposition
                    dir = GetRandomDirection() * directionMagnitude;
                    horizontalArrowsList[stateIterator].transform.position = transformPos + dir;
                    horizontalArrowsList[stateIterator].transform.rotation = Quaternion.LookRotation(transformPos - (transformPos + dir));
                    horizontalArrowsList[stateIterator].SetActive(true);
                    horizontalArrowsListScripts[stateIterator].StartRotScaleRoutine();
                    //tempAccel += 3;
                    horizontalArrowsListScripts[stateIterator].acceleration += tempAccel;
                    
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
                    danceFloorController.ChangeCurrentLightMode(DanceFloorController.FloorLightModes.Circular);
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
                    edDanceArrowsList[edArrowIndex].transform.rotation = Quaternion.Euler(90, 0, zRot + hardRotValueParent);
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
                            MainIcoSphereController.isMultiBouncing = true;
                            MainIcoSphereController.StopParticleEmitting();
                        }
                        danceFloorController.BeginArrowLerp(0.15f);
                        currArrow.gameObject.SetActive(false);
                        if (currArrow == edDanceArrowsList[maxEDDanceArrows - 2].transform)
                        {
                            MainIcoSphereController.isExploding = true;
                        }
                        else if (currArrow == edDanceArrowsList[maxEDDanceArrows - 1].transform)
                        {
                            ed2Splash.Play();
                            edArrowIndex = 0;
                            maxTime = 0;
                            //speed = 1;
                            areArrowsReset = true;
                            edTimeOffset = originalEDTimeOffset;
                            edTime = Time.time + 2;
                            currentEnum = DanceStages.Ending2;
                            centerModelIndex++;
                            CycleNextCenterModel(centerModelIndex, 0.5f);
                            danceFloorController.ChangeCurrentLightMode(DanceFloorController.FloorLightModes.Random);
                            danceFloorController.TargetCustomLerps(2, 0, 6, 2);
                            isStartingAnim = false;
                        }
                    }
                }
                break;

            case DanceStages.Ending2:
                if (Time.time >= edTime)
                {
                    MainIcoSphereController.isExploding = false;
                    MainIcoSphereController.isBouncing = true;
                    DeathAnimController(false);
                    MainIcoSphereController.StartUpsizing();
                    danceFloorController.StartRandomLerps();
                    currentEnum = DanceStages.EMPTY;
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
                MainIcoSphereController.ControlBounce(1, (i + 1) * 2 + 2);
                danceFloorController.LightAllOnce(1/((i * 0.5f) +1f));
                danceFloorController.BeginArrowLerp();
                /*shaderT = 0;
                shaderSpeedT = 1;
                if (i<2)
                {
                    shaderSpeedT = 1;
                }
                else
                {
                    shaderSpeedT = i - 1;
                }*/
                centerModelIndex = i;
                CycleNextCenterModel(centerModelIndex,1);
            }
        }
    }

    [SerializeField] int centerModelIndex = 0;
    void CycleNextCenterModel(int i,float shaderTimeSpeed)
    {
        shaderT = 0;
        shaderSpeedT = shaderTimeSpeed;
        centerModels[i].gameObject.SetActive(false);
        if (centerModels[i + 1] != null)
        {
            centerModels[i + 1].gameObject.SetActive(true);
        }
    }

    void HardGenOffsets()
    {
        if(stateIterator == 0)
        {
            genOffset = 1;
            tempAccel = 5f;
        }
        else if(stateIterator == 1)
        {
            genOffset = 0.3f;
            tempAccel = 10f;
        }
        else if(stateIterator == 2)
        {
            genOffset = 0.3f;
            tempAccel = 15f;
        }
        else if(stateIterator == 3)
        {
            genOffset = 0.2f;
            tempAccel = 16f;
        }
        else if (stateIterator == 4)
        {
            genOffset = 0.1f;
            tempAccel = 17f;
        }
    }

}
