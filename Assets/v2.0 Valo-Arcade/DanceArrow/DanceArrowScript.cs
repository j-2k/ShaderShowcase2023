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
    // Update is called once per frame

    float originalStartingSpeed = 0;

    IEnumerator currRoutine;

    private void Start()
    {
        originalStartingSpeed = speed;
        if (isVertical)
        {
            trailingOrbs.gameObject.SetActive(false);
            parentPFX.gameObject.SetActive(false);
        }
    }

    //float t = 0;
    void Update()
    {
        if(!isVertical)
        {
            //t += Time.deltaTime;
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
        StartCoroutine(routine);
    }

    IEnumerator routineRotScale()
    {
        yield return new WaitForEndOfFrame();
    }
}
