using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArcadeDanceController : MonoBehaviour
{
    [SerializeField] Material deformShaderMat;
    [SerializeField] GameObject danceArrowFab;
    [SerializeField] DanceStages currentEnum;
    float speed;
    [SerializeField] float angleTheta;
    [SerializeField] float directionMagnitude = 5;

    Vector3 transformPos;
    Vector3 dir;

    enum DanceStages {
        EMPTY,
        Generate,
        Playing,
        Ending,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 1;
        transformPos = transform.position + Vector3.up * 1.0f;
        currentEnum = DanceStages.EMPTY;
        deformShaderMat = transform.GetChild(0).Find("Surface").GetComponent<Renderer>().material;
    }

    GameObject MainDanceArrow;






    void DebugDirectionRay()
    {
        Debug.DrawRay(transformPos, dir, Color.green);
    }
    // Update is called once per frame
    void Update()
    {
        DebugDirectionRay();
        deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time*2,1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));
        transformPos = transform.position + Vector3.up * 1.0f;
        EnumDanceStates();
       
    }

    void EnumDanceStates()
    {
        switch (currentEnum)
        {
            case DanceStages.EMPTY:
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
                speed += 7 * Time.deltaTime;
                MainDanceArrow.transform.position += MainDanceArrow.transform.forward * speed * Time.deltaTime;
                if (Vector3.Distance(MainDanceArrow.transform.position, transformPos) < 0.5)
                {
                    MainDanceArrow.SetActive(false);
                    currentEnum = DanceStages.Generate;
                    speed = 1;
                }
                break;


            case DanceStages.Ending:
                Debug.Log("Ending");
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
}
