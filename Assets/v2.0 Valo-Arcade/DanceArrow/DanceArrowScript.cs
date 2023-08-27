using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceArrowScript : MonoBehaviour
{
    public float speed = 1;
    public float acceleration = 1;
    public bool isVertical = false;
    public static float allSpeed = 1;
    public ParticleSystem trailingOrbs;
    [SerializeField] GameObject parentPFX;
    [SerializeField] Transform arrowObj;
    // Update is called once per frame

    float originalStartingSpeed = -5;
    float originalStartingAcc = 10;
    float originalStartingScale = 0.25f;

    IEnumerator currRoutine;

    private void Start()
    {
        originalStartingSpeed = speed;
        originalStartingAcc = acceleration;
        if (isVertical)
        {
            trailingOrbs.gameObject.SetActive(false);
            parentPFX.gameObject.SetActive(false);
        }
    }

    float t = 0;
    void Update()
    {

        if (!isVertical)
        {
            t += Time.deltaTime * 1;
            //speed += Mathf.Lerp(acceleration/2,acceleration*2, t) * Time.deltaTime;
            speed += acceleration * 1f * Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (trailingOrbs.transform.parent == null)
            {
                Debug.Log("ARE WE INSIDE THE DANCE PARTICLE PARENT?");
                trailingOrbs.transform.SetParent(transform);
                trailingOrbs.transform.position = transform.position;
                trailingOrbs.transform.localRotation = Quaternion.identity;
                trailingOrbs.Play();
            }
        }
        else
        {
            allSpeed += acceleration * Time.deltaTime;
            transform.position += transform.forward * allSpeed * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if(!isVertical)
        {
            speed = originalStartingSpeed;
            acceleration = originalStartingAcc;
            t = 0;
            //t = 0;
        }
    }

    public void StartRotScaleRoutine()
    {
        CheckStartCouroutine(currRoutine);
    }

    void CheckStartCouroutine(IEnumerator routine)
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        routine = routineRotScale();

        arrowObj.localScale = Vector3.one * originalStartingScale;
        t = 0;

        StartCoroutine(routine);
    }

    IEnumerator routineRotScale()
    {
        arrowObj.localScale = Vector3.one * (originalStartingScale + (t));
        arrowObj.Rotate(0, 0, 150 * (Time.deltaTime*3));
        yield return new WaitForEndOfFrame();
        StartCoroutine(routineRotScale());
    }
}
