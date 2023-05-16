using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArcadeDanceController : MonoBehaviour
{
    [SerializeField] Material deformShaderMat;
    [SerializeField] GameObject danceArrowFab;
    [SerializeField] DanceStages currentEnum;

    enum DanceStages {
        EMPTY,
        Idle,
        Playing,
        Ending,
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentEnum = DanceStages.EMPTY;
        deformShaderMat = transform.GetChild(0).Find("Surface").GetComponent<Renderer>().material;
    }

    GameObject MainDanceArrow;
    // Update is called once per frame
    void Update()
    {
        deformShaderMat.SetFloat("_Strength", Mathf.PingPong(Time.time*2,1));//(-0.5f,0.5f, Mathf.Abs(Mathf.Sin(Time.time))));

        switch (currentEnum)
        {
            case DanceStages.EMPTY:
                Debug.Log("EMPTY");
                break;
            case DanceStages.Idle:
                Debug.Log("Idle");
                MainDanceArrow = GameObject.Instantiate(danceArrowFab, transform.position - Vector3.forward * 3,
                    Quaternion.LookRotation(transform.position - danceArrowFab.transform.position));
                currentEnum = DanceStages.Playing;
                break;
            case DanceStages.Playing:
                Debug.Log("Playing");
                MainDanceArrow.transform.position += transform.forward * Time.deltaTime;
                if (Vector3.Distance(MainDanceArrow.transform.position, transform.position) < 0.5)
                {
                    MainDanceArrow.SetActive(false);
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
}
