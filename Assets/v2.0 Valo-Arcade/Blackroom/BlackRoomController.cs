using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRoomController : MonoBehaviour
{
    Animator blackroomAnimator;
    [SerializeField]Animator cypherAnim;
    [SerializeField] GameObject noiseBall;

    public bool startThrow = false;
    public bool startNoiseBall = false;

    bool oneRun = false;

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

        if(startNoiseBall)
        {

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
}
