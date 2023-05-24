using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuakeMovement : MonoBehaviour
{
    [Header("SKIP")]
    [SerializeField] Vector3 wishDir;
    CharacterController cc;

    [SerializeField] float rotX, rotY, sens, angle;

    Camera cam;

    [Header("MAIN PARAMS")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float moveAccel = 10f;
    [SerializeField] float moveDecel = 7f;
    [SerializeField] float floorFriction = 6f;
    [SerializeField] Vector3 playerVelocity;

    [SerializeField] float CheckPlayerFriction;
    [SerializeField] float CheckPlayerSpeed;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    Vector3 localForward;
    Vector3 localRight;

    

    void VectorManagement()
    {
        localForward = new Vector3(Mathf.Sin(rotX * Mathf.Deg2Rad), 0, Mathf.Cos(rotX * Mathf.Deg2Rad));
        localRight = new Vector3(Mathf.Cos(rotX * Mathf.Deg2Rad), 0, -Mathf.Sin(rotX * Mathf.Deg2Rad));

        wishDir = (Input.GetAxis("Horizontal") * localRight + Input.GetAxis("Vertical") * localForward);
        if(wishDir.magnitude > 1)
        {
            wishDir.Normalize();
        }
    }

    void QuakeMove()
    {
        /*
        if(cc.isGrounded) //cc is grounded is trash
        { UpdateGroundedVelocity(); }
        else
        { UpdateAirVelocity(); }
        */

        UpdateGroundedVelocity();

        cc.Move(playerVelocity * Time.deltaTime);
    }

    void UpdateGroundedVelocity()
    {
        Debug.Log("Grounded!");

        ApplyFriction();

        float wishSpeed = wishDir.magnitude;
        wishSpeed *= moveSpeed;

        SV_ACCELERATION(wishSpeed, moveAccel);
        CheckPlayerSpeed = playerVelocity.magnitude;
    }

    void SV_ACCELERATION(float wishspeed, float accel)
    {
        float addspeed, accelspeed, currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishDir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;
        accelspeed = accel * Time.deltaTime * wishspeed;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        playerVelocity.x += accelspeed * wishDir.x;
        playerVelocity.z += accelspeed * wishDir.z;
    }

    void ApplyFriction()
    {
        Vector3 vel = playerVelocity;
        float speed, newspeed, control;

        speed = vel.magnitude;
        if (speed < 0)
                return;

        vel.y = 0.0f;
        speed = vel.magnitude;

        //Only if the player is on the ground then apply friction
        /*
        if (cc.isGrounded)
        {
            control = speed < moveDecel ? moveDecel : speed;
            drop = control * floorFriction * Time.deltaTime;
        }
        */

        control = speed < moveDecel ? moveDecel : speed;
        newspeed = speed - Time.deltaTime * control * floorFriction;


        CheckPlayerFriction = newspeed;


        if (newspeed < 0)
        {
            newspeed = 0;
        }
            
        if (speed > 0)
            newspeed /= speed;
        

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }

    void UpdateAirVelocity()
    {
        Debug.LogWarning("IN AIR!!!");
        //cc.Move(wishDir * 5 * Time.deltaTime);
    }
    

    void DebugMovementVectors()
    {
        Debug.DrawRay(transform.position, wishDir * moveSpeed, Color.blue);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 3, Color.magenta);
        Debug.DrawRay(transform.position + Vector3.up, cc.velocity, Color.red);
        debugCCVelocity = cc.velocity;
    }

    void MouseHandler()
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
}
