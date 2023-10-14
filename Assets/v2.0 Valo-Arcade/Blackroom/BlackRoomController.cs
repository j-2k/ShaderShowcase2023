using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRoomController : MonoBehaviour
{
    Animator blackroomAnimator;
    [SerializeField]Animator cypherAnim;
    [SerializeField] GameObject noiseBall;
    [SerializeField] GameObject handObject;
    [SerializeField] GameObject targetPos;

    public bool startThrow = false;
    public bool throwBall = false;

    bool oneRun = false;
    Vector3 frontCAM;
    // Start is called before the first frame update
    void Start()
    {
        blackroomAnimator = GetComponent<Animator>();
        blackroomAnimator.SetTrigger("StartRoom");

    }

   
    private void Update()
    {
        if(startThrow)
        {
            StartThrowAnimation();
        }

        if (!throwBall)
        {
            if (Vector3.Distance(noiseBall.transform.position, handObject.transform.position) <= 1.5f)
            {
                noiseBall.transform.SetParent(handObject.transform);
                Invoke(nameof(ThrowBallDelay), 1.4f);
            }
        }
        else
        {
            frontCAM = targetPos.transform.position + targetPos.transform.forward * 1.05f ;
            if (Vector3.Distance(noiseBall.transform.position, frontCAM) > 0.1f)
            {
                noiseBall.transform.SetParent(null);
                Vector3 dir = frontCAM - noiseBall.transform.position;
                noiseBall.transform.position += dir.normalized * 100 * Time.deltaTime;
            }
            else
            {
                this.enabled = false;
            }
        }

    }

    void StartThrowAnimation()
    {
        if(!oneRun)
        {
            cypherAnim.SetTrigger("Throw");
            oneRun = true;
        }
    }

    void ThrowBallDelay()
    {
        throwBall = true;
    }
}
