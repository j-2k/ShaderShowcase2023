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



    Vector3 GetRandomDirection(float angle, bool isRandom = false)
    {
        if(isRandom)
        {
            float randomRadian = Random.Range(0, Mathf.PI*2);
            Debug.Log(randomRadian + "RRAND RADIAN1");
            Vector3 direction = new Vector3(Mathf.Sin(randomRadian), 0, Mathf.Cos(randomRadian));
            if(Vector3.Angle(dir, direction) <= 90)
            {
                Debug.Log("Angle is < 90, Gen New");
                direction.x *= -1;
                direction.z *= -1;
                Debug.Log(randomRadian + "RRAND RADIAN2");
            }

            //Debug.Log(direction);
            return direction;

        }
        else
        {
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
            Debug.Log(direction);
            return direction;
        }
    }

    void RotARay()
    {
        if (Time.time >= a)
        {
            dir = GetRandomDirection(angleTheta, true) * 3;
            a = Time.time + 2;
        }
        Debug.DrawRay(transformPos, dir, Color.green);
    }
    float a;
    // Update is called once per frame
    void Update()
    {
        RotARay();

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
                if (MainDanceArrow == null)
                {
                    MainDanceArrow = GameObject.Instantiate(danceArrowFab, GenRandom8GridPos(transform),
                        Quaternion.LookRotation(transform.position));
                }
                else
                {
                    MainDanceArrow.transform.position = GenRandom8GridPos(transform);
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



    Vector3 GenRandom8GridPos(Transform center, bool isWorldSpace = false)
    {
        //Ditching this idea Random Direction Vec will look better
        return Vector3.zero + Vector3.up * 1f;
    }
}
