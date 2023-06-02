using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quake1Move : MonoBehaviour
{
    [SerializeField] Vector3 wishDir;
    [SerializeField] Vector3 playerVelocity;
    [SerializeField] float ground_accelerate;
    [SerializeField] float max_velocity_ground;
    [SerializeField] float air_accelerate;
    [SerializeField] float max_velocity_air;
    [SerializeField] float friction;
    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] float sens = 1;

    // Start is called before the first frame update
    void Start()
    {
        MainStartFunction();
    }

    // Update is called once per frame
    void Update()
    {
        MainFunctions();
        QuakeMovementManager();
    }

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
        cc.Move(playerVelocity * Time.deltaTime );
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

        if(currSpeed != 0)
        {
            //drop in speed, amt to reduce speed by friction
            float drop = currSpeed * friction * Time.deltaTime;
            scaledVelocity = currVelocity * MathF.Max(currSpeed - drop, 0)/ currSpeed;//care brackets
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
            currUUPS.text = (Mathf.Round(cc.velocity.magnitude * 100) * 0.01f).ToString();
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
}
