using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour {

    //Public
    public SkatePosition skate;
    public LayerMask slideMask;
    public Transform flyingPivot;
    public float moveSpeed = 5;
    public float airSpeed = 15;
    public float jumpForce = 10;
    public float slidePower = 5;
    public float slideDrag = 0;
    public float gravityPower = 9;
    public float airDrag = 0.2f;
    public float forwardRayDist = 1;

    //Private
    new Rigidbody rigidbody;
    Animator anim;
    CapsuleCollider col;
    
    Vector3 movement;
    Vector3 gravity;
    Vector3 slideDirection;
    Vector3 airDirection;
    Vector3 hitNormal;

    float gravityForce;
    float slideForce;
    float airForce;

    bool grounded;
    bool lastFrameGrounded;
    bool lastFrameFlying;

    // Use this for initialization
    void Awake () {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        SetGroundCondition();

        if (grounded)
        {
            if(lastFrameFlying)
            {
                slideForce = Vector3.ProjectOnPlane(airDirection, hitNormal).magnitude;
                airForce = 0;
                airDirection = Vector3.zero;
                gravityForce = 0;                
                anim.SetFloat("left", Random.Range(0.06f,1));
                anim.SetBool("grounded", true);
                anim.SetFloat("back", 0);
                skate.transform.parent = transform;
                skate.BounceSkate();
                flyingPivot.localEulerAngles = Vector3.zero;
            }
            slideForce += slidePower * Time.deltaTime;
            slideForce /= 1 + slideDrag * Time.deltaTime;
            
            slideDirection *= slideForce;
            
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0), Space.Self);
            anim.transform.localEulerAngles = new Vector3(anim.transform.localEulerAngles.x, Input.GetAxis("Horizontal") * 20, -Input.GetAxis("Horizontal") * 20);
            gravity = -transform.up;           

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("jump");
                anim.SetFloat("back", Random.value);
                gravityForce = jumpForce;
                gravity = transform.up * gravityForce;
            }
        }
        else
        {
            if (lastFrameGrounded)
            {
                airForce = slideDirection.magnitude;
                slideDirection = Vector3.zero;
                slideForce = 0;
                movement = Vector3.zero;
                anim.transform.localEulerAngles = Vector3.zero;
                anim.SetBool("grounded", false);
                
                skate.SetSkateBelowFeet();
            }

            gravityForce -= gravityPower * Time.deltaTime;
            airDirection = airForce * transform.forward; 

            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * airSpeed, 0), Space.Self);

            gravity = transform.up * gravityForce;

            gravityForce /= 1 + airDrag * Time.deltaTime;
            airForce /= 1 + airDrag * Time.deltaTime;

            flyingPivot.localEulerAngles = new Vector3(flyingPivot.localEulerAngles.x, flyingPivot.localEulerAngles.y, -Input.GetAxis("Horizontal") * 20);

            Debug.DrawRay(transform.position, airDirection * 10, Color.cyan);
        }

        rigidbody.velocity = movement + slideDirection + airDirection + gravity;

        lastFrameGrounded = grounded;
        lastFrameFlying = !grounded;
        
	}

    void SetGroundCondition()
    {
        RaycastHit downHit;
        RaycastHit frontHit;

        bool down = Physics.Raycast(transform.position, -transform.up, out downHit, col.height/2 + 0.3f, slideMask);
        bool up = Physics.Raycast(transform.position + transform.forward * forwardRayDist, -transform.up, out frontHit, 2f, slideMask);

        Debug.DrawRay(transform.position, -transform.up * (col.height/2 + 0.3f), Color.red);
        Debug.DrawRay(transform.position + transform.forward * forwardRayDist, -transform.up * 2, Color.blue);

        if (down && up)
        {
            slideDirection = (frontHit.point - downHit.point).normalized;
            grounded = true;
            hitNormal = downHit.normal;
            transform.rotation = Quaternion.LookRotation(slideDirection, hitNormal);
            Debug.DrawRay(downHit.point, slideDirection * 5, Color.green);
        }
        else
        {
            grounded = false;
        }

    }

}
