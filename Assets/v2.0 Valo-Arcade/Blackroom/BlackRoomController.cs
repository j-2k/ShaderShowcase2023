using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRoomController : MonoBehaviour
{
    Animator blackroomAnimator;
    [SerializeField]Animator cypherAnim;

    public bool startThrow = false;

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
            this.enabled = false;
        }    
    }

    void StartThrowAnimation()
    {
        cypherAnim.SetTrigger("Throw");
    }
}
