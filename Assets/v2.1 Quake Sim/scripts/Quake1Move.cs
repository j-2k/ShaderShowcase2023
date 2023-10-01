using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Quake1Move : MonoBehaviour
{
    [SerializeField] Vector3 wishDir;           //Vector3.zero
    [SerializeField] Vector3 playerVelocity;    //Vector3.zero
    [SerializeField] float ground_accelerate;   //100
    [SerializeField] float max_velocity_ground; //10
    [SerializeField] float air_accelerate;      //200-400
    [SerializeField] float max_velocity_air;    //1-2
    [SerializeField] float friction;            //7
    [SerializeField] float jumpSpeed;           //12
    [SerializeField] float gravity;             //30
    [SerializeField] float sens = 1;            //1
    [SerializeField] float slideLerp = 0;

    public float currentSpeed {get{ return cc.velocity.magnitude; }}
    public Vector3 currentVelocityVector {get { return cc.velocity; }}

    public float speedCheck;

    // Start is called before the first frame update
    void Start()
    {
        MainStartFunction();
    }

    // Update is called once per frame
    void Update()
    {
        speedCheck = currentSpeed;
        MainFunctions();
        QuakeMainMovement();
        //QuakeMovementManager();
    }

    [SerializeField] bool isJumping;
    [SerializeField] bool isSliding;
    [SerializeField] bool isGrounded;

    void QuakeMainMovement()
    {
        isGrounded = cc.isGrounded;
        isJumping = Input.GetButton("Jump");
        if (cc.isGrounded)
        {
            if (isJumping)
            {
                playerVelocity.y = jumpSpeed;
                isJumping = false;
                isSliding = false;
            }
            else//REMOVE THIS ELSE STATEMENT IN CASE YOU WANT 1 FRAME OF FRICTION
            {
                isSliding = Input.GetKey(KeyCode.LeftShift);
                if (isSliding)
                {
                    playerVelocity = MoveSlide(0.1f, playerVelocity);//0.1f to make it difficult to gain speed unless your cheating with perfect 90 degree angles
                                                                     //mainly this is here to slow down no matter what you are doing in sliding state UNLESS going forward only.
                    //Gravity(); //gravity creating instant weird extreme downforce as soon as falling off a platform?
                }
                else
                {
                    playerVelocity = MoveGround(wishDir, playerVelocity);
                    Gravity();
                }
            }
        }
        else
        {
            Gravity();
            playerVelocity = MoveAir(wishDir, playerVelocity);
        }
        //Gravity();
        cc.Move(playerVelocity * Time.deltaTime);
    }

    Vector3 Accelerate(Vector3 currVelocity, float maxAccel, float maxVelocity)
    {
        float currentSpeedDot = Vector3.Dot(wishDir, currVelocity);

        float addSpeed = Mathf.Clamp(maxVelocity - currentSpeedDot, 0, maxAccel * Time.deltaTime);

        Vector3 finalVec = currVelocity + wishDir * addSpeed;

        return finalVec;
    }

    Vector3 OldAccelerate(Vector3 wishDirection, Vector3 currVelocity, float accelerate, float max_velocity)
    {
        float projVel = Vector3.Dot(wishDirection, currVelocity); // Vector projection of Current velocity onto wishDirection.
        float accelVel = accelerate * Time.deltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        Vector3 finalVec = currVelocity + wishDirection * accelVel;
        return finalVec;
    }

    Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        // Apply Friction
        float speed = prevVelocity.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * friction * Time.deltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }

        // ground_accelerate and max_velocity_ground are server-defined movement variables
        return Accelerate(prevVelocity, ground_accelerate, max_velocity_ground);
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
    {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(prevVelocity, air_accelerate, max_velocity_air);
    }

    private Vector3 MoveSlide(float fricRamp, Vector3 prevVelocity)
    {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(prevVelocity, air_accelerate, max_velocity_air * fricRamp);
    }

    void Gravity()
    {
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    public void TeleportPlayer(Vector3 destination, bool isONLYSPEEDRESET = false)
    {
        if(isONLYSPEEDRESET)
        {
            playerVelocity = Vector3.zero;
            return;
        }
        playerVelocity = Vector3.zero;
        cc.enabled = false;
        cc.transform.position = destination;
        cc.enabled = true;
    }

    //Ontriggerenter for teleporting would be better but torus cant be trigger because of convex flattening it
    //other solution was to use sphere triggers in the torus but yeah not a big fan of that rn 
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.tag == "bh_trigger")
        {
            TeleportPlayer(ExtraControls.Respawn1);
        }
        Debug.Log("OnControllerColliderHit");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bh_trigger")
        {
            TeleportPlayer(ExtraControls.Respawn1);
        }
        Debug.Log("OnTriggerEnter");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "bh_boundbox")
        {
            TeleportPlayer(ExtraControls.Respawn1);
        }
        Debug.Log("OnTriggerExit");
    }

    public bool isJumpingCheck() {return isJumping;}
    public bool isSlidingCheck() { return isSliding; }
    public bool isGroundedCheck() { return isGrounded; }






    #region DEBUGS & OTHERS
    CharacterController cc;
    [SerializeField] TextMeshProUGUI currUUPS;
    void MainStartFunction()
    {
        anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {isAnimating = true;}
        if (currUUPS != null)
        {isUpdatingUUPS = true;}

        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void MainFunctions()
    {
        MouseHandler();
        VectorManagement();
        DebugMovementVectors();
        AnimatorManager();
        UpdateUUPS();
    }
    Vector3 localForward;
    Vector3 localRight;
    void VectorManagement()
    {
        localForward = transform.forward;
        localRight = transform.right;
        /*
        wishDir = (Input.GetAxis("Horizontal") * localRight + Input.GetAxis("Vertical") * localForward);
        if (wishDir.magnitude > 1)
        {
            wishDir.Normalize();
        }
        */
        wishDir = (Input.GetAxisRaw("Horizontal") * localRight + Input.GetAxisRaw("Vertical") * localForward).normalized;
    }

    public void DebugMovementVectors()
    {
        Debug.DrawRay(transform.position + Vector3.up * 1, wishDir, Color.blue);
        Debug.DrawRay(cam.transform.position, cam.transform.forward * 1, Color.magenta);
        Debug.DrawRay(transform.position + Vector3.up * 2, cc.velocity, Color.red);
        debugCCVelocity = cc.velocity;
    }

    float offset;
    bool isUpdatingUUPS;
    public void UpdateUUPS()
    {
        if (isUpdatingUUPS && offset < Time.timeSinceLevelLoad)
        {
            currUUPS.text = Mathf.RoundToInt(cc.velocity.magnitude*10).ToString();//remove the " * 10 " to remove the fake upscaled speed display
            offset = Time.timeSinceLevelLoad + 0.1f;
        }
    }

    float rotX, rotY;
    Camera cam;
    public void MouseHandler()
    {
        rotX += Input.GetAxis("Mouse X") * sens;
        rotY += Input.GetAxis("Mouse Y") * sens;
        rotY = Mathf.Clamp(rotY, -90, 90);

        cam.transform.rotation = Quaternion.Euler(-rotY, rotX, 0);


        transform.eulerAngles = new Vector3(0, rotX, 0);
        //transform.rotation = Quaternion.Euler(0, rotX, 0);
    }
    Animator anim;
    bool isAnimating;
    float maxVelo = 10;
    [SerializeField] float lerpAmt = 0;
    public void AnimatorManager()
    {
        
        if (isAnimating)
        {
            if(isSliding)
            {
                anim.SetBool("isRunning", false);
                return;
            }
            Vector3 xzVelo = playerVelocity;
            xzVelo.y = 0;
            /*
            //can compare velocity but having some issues rn
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))    //hack but whatever
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
            */

            if (xzVelo.magnitude > 1f)
            {
                anim.SetBool("isRunning", true);
                anim.SetFloat("Multiply", Mathf.Lerp(0.1f,1, (xzVelo.magnitude) / maxVelo));
            }
            else
            {
                anim.SetBool("isRunning", false);
                
            }
            lerpAmt = Mathf.Lerp(0.1f, 1, (xzVelo.magnitude) / maxVelo);
        }

    }

    Vector3 debugCCVelocity;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + debugCCVelocity + Vector3.up * 2, 0.25f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Vector3.up * 1 + transform.position + wishDir, 0.25f);
    }
    #endregion

    #region Old
    /*
    [SerializeField] bool isJumping;
    [SerializeField] bool isGrounded;
    void QuakeMainMovement()
    {
        isGrounded = cc.isGrounded;
        isJumping = Input.GetButton("Jump");
        if (cc.isGrounded)
        {
            //playerVelocity = GroundMove();
            playerVelocity = MoveGround(wishDir, playerVelocity);
            Gravity();
            if (isJumping)
            {
                playerVelocity.y = jumpSpeed;
                isJumping = false;
            }
        }
        else
        {
            Gravity();
            playerVelocity = MoveAir(wishDir, playerVelocity);
        }

        cc.Move(playerVelocity * Time.deltaTime);
    }


    Vector3 GroundMove()
    {
        return wishDir * ground_accelerate;
    }

    Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity)
    {
        float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
        float accelVel = accelerate * Time.deltaTime; // Accelerated velocity in direction of movment

        // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
        if (projVel + accelVel > max_velocity)
            accelVel = max_velocity - projVel;

        return prevVelocity + accelDir * accelVel;
    }

    Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
    {
        // Apply Friction
        float speed = prevVelocity.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            float drop = speed * friction * Time.deltaTime;
            prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
        }

        // ground_accelerate and max_velocity_ground are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, ground_accelerate, max_velocity_ground);
    }

    private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
    {
        // air_accelerate and max_velocity_air are server-defined movement variables
        return Accelerate(accelDir, prevVelocity, air_accelerate, max_velocity_air);
    }
    */
    #endregion

    #region gd
    /*
    //DO GODOT THING
    [SerializeField] bool isGrounded;
    [SerializeField] bool isJumping;
    void QuakeMovementManager()
    {
        isGrounded = cc.isGrounded;
        isJumping = Input.GetButton("Jump");
        if (cc.isGrounded)
        {//DO GODOT THING
            GroundMove(cc.velocity);
            Gravity();
            if (isJumping)
            {
                playerVelocity.y = jumpSpeed;
                isJumping = false;
            }
        }
        else
        {//DO GODOT THING
            //playerVelocity = wishDir * max_velocity_air;
            AirMove(cc.velocity);
            Gravity();
        }
        cc.Move(playerVelocity * Time.deltaTime);
    }
    void Gravity()
    {
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    void AirMove(Vector3 currVelocity)
    {
        Vector3 newVeloXZ = currVelocity;
        newVeloXZ.y = 0;

        newVeloXZ = Accelerate(newVeloXZ, air_accelerate, max_velocity_air);

        newVeloXZ.y = currVelocity.y;
        playerVelocity = newVeloXZ;
    }

    void GroundMove(Vector3 currVelocity)
    {
        Vector3 newVeloXZ = currVelocity;
        newVeloXZ.y = 0;

        newVeloXZ = Friction(newVeloXZ);
        newVeloXZ = Accelerate(newVeloXZ, ground_accelerate, max_velocity_ground);

        newVeloXZ.y = currVelocity.y;
        playerVelocity = newVeloXZ;
    }

    Vector3 Friction(Vector3 currVelocity)
    {
        float currSpeed = currVelocity.magnitude;
        Vector3 scaledVelocity = Vector3.zero;

        if (currSpeed != 0)
        {
            //drop in speed, amt to reduce speed by friction
            float drop = currSpeed * friction * Time.deltaTime;
            scaledVelocity = currVelocity * MathF.Max(currSpeed - drop, 0) / currSpeed;//care brackets
        }

        return scaledVelocity;
    }

    Vector3 Accelerate(Vector3 currVelocity, float maxAccel, float maxVelocity)
    {
        float currentSpeedDot = Vector3.Dot(wishDir, currVelocity);

        float addSpeed = Mathf.Clamp(maxVelocity - currentSpeedDot, 0, maxAccel * Time.deltaTime);

        Vector3 finalVec = currVelocity + wishDir * addSpeed;

        return finalVec;
    }
    */
    #endregion
}
