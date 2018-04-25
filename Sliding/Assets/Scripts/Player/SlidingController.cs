using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SlidingController : MonoBehaviour
{
    public float airAcceleration = 20; 
    public float gravityPower = 10;
    public float airDrag = 0.2f;
    public float slideMovementAcceleration = 40; 
    public float slideMovementDrag = 0.5f;
    public LayerMask slideMask; 
    public LayerMask groundMask;

    public bool slidingStrafeAccelEnabled = true; 

    float ccHeight;
    float ccWidth;
    float slideForce;
    float strafeMagnitude;
    float gravityForce;

    bool sliding;

    bool lastFrameFlying;
    bool lastFrameGrounded;
    bool lastFrameSliding;
    bool canCastRays;

    Vector3 gravity = Vector3.zero;
    Vector3 movement = Vector3.zero;
    Vector3 slideDirection = Vector3.zero;
    Vector3 airForce = Vector3.zero;

    CharacterController cc;
    Vector3 hitNormal;
    Vector3 sideHitNormal;

    public Vector3 forwardRayStart = Vector3.zero;
    public float forwardRayDist = 1;

    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        ccHeight = GetComponent<CharacterController>().bounds.extents.y;
        ccWidth = GetComponent<CharacterController>().bounds.extents.x;

        
    }



    // Update is called once per frame
    void Update()
    {
        CheckGroundCondition(); 

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input.Normalize();
        input = transform.TransformDirection(input); 


        if (sliding)
        {
            if (lastFrameFlying) 
            {
                slideForce = gravity.y; 
                gravity = Vector3.zero;
                movement = Vector3.zero;
                airForce = Vector3.zero;
            }

            slideForce -= gravityPower * Time.deltaTime; 
            slideDirection *= slideForce;


            //Debug.Log(slideDirection.normalized);



            Quaternion rot = new Quaternion();
            rot = Quaternion.FromToRotation(Vector3.up, hitNormal); 
            input = rot * input; 

            Vector3 slidePerpendicular = Vector3.Cross(slideDirection.normalized, hitNormal).normalized;
            float strafeDot = Vector3.Dot(slidePerpendicular, input); 

            if(slidingStrafeAccelEnabled) 
            {
                strafeMagnitude += slideMovementAcceleration * strafeDot * Time.deltaTime; 
                strafeMagnitude /= 1 + slideMovementDrag * Time.deltaTime; 
                movement = slidePerpendicular * strafeMagnitude; 
            }
            else
            {
                movement = slidePerpendicular * slideMovementAcceleration * strafeDot;
            }

            if(Physics.Raycast(transform.position, movement, ccWidth +0.1f))
            {
                strafeMagnitude = 0;
            }

        }
        else
        {
            
            if (lastFrameSliding)
            {
                airForce = slideDirection; 
                slideDirection = Vector3.zero;
            }            

            airForce += input * airAcceleration * Time.deltaTime; 
            airForce /= 1 + airDrag * Time.deltaTime;

            gravityForce -= gravityPower * Time.deltaTime;
            gravity = transform.up * gravityForce; 
        }

        cc.Move((movement + gravity + slideDirection + airForce) * Time.deltaTime); 

        lastFrameSliding = sliding;

        Debug.Log("  Sliding: " + sliding);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        sideHitNormal = hit.normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SlideDirectionTrigger")
        {
            forwardRayStart = other.transform.forward;
        }
    }


    void CheckGroundCondition()
    {
        RaycastHit downHit; 
        RaycastHit frontHit;
       
        if (Physics.Raycast(transform.position, -transform.up, out downHit, ccHeight + 0.5f, slideMask) && 
            Physics.Raycast(transform.position + transform.forward * forwardRayDist, -transform.up, out frontHit, ccHeight + 0.5f, slideMask)) 
        {
            slideDirection = downHit.point - frontHit.point;
            slideDirection.Normalize();

            Debug.DrawRay(transform.position, -transform.up, Color.red);
            Debug.DrawRay(transform.position + transform.forward * forwardRayDist, -transform.up, Color.blue);

            sliding = true;

            hitNormal = downHit.normal;
            transform.up = frontHit.normal;
        }        
        else 
        {
            sliding = false;
        }
    }
}