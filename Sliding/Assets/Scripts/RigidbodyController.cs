using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour {

    //Public
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

    Vector3 movement;
    Vector3 gravity;
    Vector3 slideDirection;
    Vector3 airForce;
    Vector3 hitNormal;

    float gravityForce;
    float slideForce;

    bool grounded;
    bool lastFrameGrounded;
    bool lastFrameFlying;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        SetGroundCondition();

        if (grounded)
        {
            if(lastFrameFlying)
            {
                slideForce = Vector3.ProjectOnPlane(airForce, hitNormal).magnitude;
                airForce = Vector3.zero;
                gravityForce = 0;                
                anim.SetFloat("left", Random.Range(0.06f,1));
                anim.SetBool("grounded", true);
                anim.SetFloat("back", 0);
            }

            slideForce += slidePower * Time.deltaTime;
            slideForce /= 1 + slideDrag * Time.deltaTime;
            
            slideDirection *= slideForce;

            //movement = Vector3.Cross(slideDirection.normalized, -transform.up).normalized * Input.GetAxis("Horizontal") * moveSpeed;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, transform.localEulerAngles.z);
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
                airForce = slideDirection;
                slideDirection = Vector3.zero;
                slideForce = 0;
                movement = Vector3.zero;
                anim.transform.localEulerAngles = Vector3.zero;
                anim.SetBool("grounded", false);
            }

            gravityForce -= gravityPower * Time.deltaTime;
            airForce += transform.right * Input.GetAxis("Horizontal") * airSpeed * Time.deltaTime;
            gravity = transform.up * gravityForce;

            gravityForce /= 1 + airDrag * Time.deltaTime;
            airForce /= 1 + airDrag * Time.deltaTime;

            flyingPivot.localEulerAngles = new Vector3(flyingPivot.localEulerAngles.x, flyingPivot.localEulerAngles.y, -Input.GetAxis("Horizontal") * 20);
        }

        rigidbody.velocity = movement + slideDirection + airForce + gravity;

        lastFrameGrounded = grounded;
        lastFrameFlying = !grounded;
	}

    void SetGroundCondition()
    {
        RaycastHit downHit;
        RaycastHit frontHit;

        bool down = Physics.Raycast(transform.position, -transform.up, out downHit, 1.1f, slideMask);
        bool up = Physics.Raycast(transform.position + transform.forward * forwardRayDist, -transform.up, out frontHit, 2f, slideMask);

        Debug.DrawRay(transform.position, -transform.up * 1.1f, Color.red);
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
