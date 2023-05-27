using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuakeMovement : MonoBehaviour
{
    [Header("SKIP")]
    [SerializeField] Vector3 wishDir;
    CharacterController cc;

    [SerializeField] float rotX, rotY, sens;

    Camera cam;

    [Header("MAIN PARAMS")]
    [SerializeField] float MAX_GROUND_SPEED = 5f;
    [SerializeField] float MAX_GROUND_ACCEL = 10f;
    [SerializeField] float SV_STOPSPEED = 7f;

    [SerializeField] float MAX_AIR_ACCEL = 10f;

    [SerializeField] float jumpSpeed = 10f;

    [SerializeField] float floorFriction = 6f;
    [SerializeField] float gravity = 10f;

    [SerializeField] Vector3 playerVelocity;

    [SerializeField] float CheckPlayerFriction;
    [SerializeField] float CheckPlayerSpeed;

    [SerializeField] bool isJumping;

    [SerializeField] TextMeshProUGUI currUUPS;
    [SerializeField] TextMeshProUGUI MAXUUPS;
    bool isUpdatingUUPS;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if(anim != null)
        {
            isAnimating = true;
        }
        if(currUUPS != null)
        {
            isUpdatingUUPS = true;
        }

        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseHandler();

        VectorManagement();

        QuakeMove();

        DebugMovementVectors();

        AnimatorManager();

        UpdateUUPS();
    }

    float offset;
    public void UpdateUUPS()
    {
        if (isUpdatingUUPS && offset < Time.timeSinceLevelLoad)
        {
            currUUPS.text = (Mathf.Round(cc.velocity.magnitude * 100) * 0.01f).ToString();
            offset = Time.timeSinceLevelLoad + 0.1f;
        }
    }


    Vector3 localForward;
    Vector3 localRight;

    

    void VectorManagement()
    {
        localForward = new Vector3(Mathf.Sin(rotX * Mathf.Deg2Rad), 0, Mathf.Cos(rotX * Mathf.Deg2Rad));
        localRight = new Vector3(Mathf.Cos(rotX * Mathf.Deg2Rad), 0, -Mathf.Sin(rotX * Mathf.Deg2Rad));
        /*
        wishDir = (Input.GetAxis("Horizontal") * localRight + Input.GetAxis("Vertical") * localForward);
        if (wishDir.magnitude > 1)
        {
            wishDir.Normalize();
        }
        */
        wishDir = (Input.GetAxisRaw("Horizontal") * localRight + Input.GetAxisRaw("Vertical") * localForward).normalized;
    }

    void QuakeMove()
    {
        isJumping = Input.GetButton("Jump");

        if(cc.isGrounded) //cc is grounded is trash
        { UpdateGroundedVelocity(); }
        else
        { UpdateAirVelocity(); }


        //UpdateGroundedVelocity();

        cc.Move(playerVelocity * Time.deltaTime);
    }

    void UpdateGroundedVelocity()
    {
        Debug.Log("Grounded!");

        ApplyFriction();

        float wishSpeed = wishDir.magnitude * MAX_GROUND_SPEED;

        SV_ACCELERATION(wishSpeed, MAX_GROUND_ACCEL);
        CheckPlayerSpeed = playerVelocity.magnitude;


        playerVelocity.y += -gravity * Time.deltaTime;

        if (isJumping)
        {
            playerVelocity.y = jumpSpeed;
            isJumping = false;
        }
    }
    [SerializeField] float currSpeedDot = 0;
    void SV_ACCELERATION(float maxSpeed, float maxAccel)
    {
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishDir);
        //currSpeedDot = currentspeed;
        addspeed = maxSpeed - currentspeed;

        if (addspeed <= 0)
        { return; }

        accelspeed = maxAccel * Time.deltaTime * maxSpeed;

        if (accelspeed > addspeed)
        { accelspeed = addspeed; }

        playerVelocity.x += accelspeed * wishDir.x;
        playerVelocity.z += accelspeed * wishDir.z;
    }

    void ApplyFriction()
    {
        Vector3 vel;
        float speed, newspeed, control, drop = 0;

        vel = playerVelocity;

        vel.y = 0.0f;
        speed = vel.magnitude;

        if (cc.isGrounded)
        {
            control = speed < SV_STOPSPEED ? SV_STOPSPEED : speed;
            drop = Time.deltaTime * control * floorFriction;
        }

        newspeed = speed - drop;
        CheckPlayerFriction = newspeed;

        if (newspeed < 0)
        {newspeed = 0;}

        if (speed > 0)
        {newspeed /= speed;}

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }
    
    void UpdateAirVelocity()
    {
        Debug.LogWarning("IN AIR!!!");

        float wishSpeed = wishDir.magnitude * MAX_GROUND_SPEED;

        SV_AIRACCELERATE(wishSpeed, MAX_AIR_ACCEL);

        playerVelocity.y -= gravity * Time.deltaTime;
    }
    

    void SV_AIRACCELERATE(float maxSpeed, float maxAirAccel)
    {
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishDir);
        addspeed = maxSpeed - currentspeed;
        currSpeedDot = currentspeed;
        if(addspeed <= 0)
        { return; }    

        accelspeed = maxAirAccel * maxSpeed * Time.deltaTime;
        if (accelspeed > addspeed)
            accelspeed = addspeed;


        playerVelocity += accelspeed * wishDir;
        //playerVelocity.x += accelspeed * wishDir.x;
        //playerVelocity.z += accelspeed * wishDir.z;
    }



    public void DebugMovementVectors()
    {
        Debug.DrawRay(transform.position, wishDir * MAX_GROUND_SPEED, Color.blue);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 3, Color.magenta);
        Debug.DrawRay(transform.position + Vector3.up, cc.velocity, Color.red);
        debugCCVelocity = cc.velocity;
    }

    public void MouseHandler()
    {
        rotX += Input.GetAxis("Mouse X") * sens;
        rotY += Input.GetAxis("Mouse Y") * sens;
        rotY = Mathf.Clamp(rotY, -90, 90);

        cam.transform.rotation = Quaternion.Euler(-rotY, rotX, 0);


        transform.eulerAngles = new Vector3(0, rotX, 0);
        //transform.rotation = Quaternion.Euler(0, rotX, 0);
    }

    Vector3 debugCCVelocity;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + debugCCVelocity + Vector3.up, 0.25f);
    }

    /* PASTED FROM ID SOFTWARE QUAKE / WINQUAKE / SV_USER.C LINE 190
    void SV_Accelerate (void)
    {
	    int			i;
	    float		addspeed, accelspeed, currentspeed;

	    currentspeed = DotProduct (velocity, wishdir);
	    addspeed = wishspeed - currentspeed;
	    if (addspeed <= 0)
	    	return;
	    accelspeed = sv_accelerate.value*host_frametime*wishspeed;
	    if (accelspeed > addspeed)
	    	accelspeed = addspeed;
	    
	    for (i=0 ; i<3 ; i++)
	    	velocity[i] += accelspeed*wishdir[i];	
    }
    */

    Animator anim;
    bool isAnimating;
    public void AnimatorManager()
    {
        if(isAnimating) 
        { 
            //can compare velocity but having some issues rn
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))    //hack but whatever
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
        }

    }

}
