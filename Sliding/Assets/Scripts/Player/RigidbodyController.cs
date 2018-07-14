using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour {

    //Public
    public SkatePosition skate;
    public LayerMask slideMask;
    public Transform flyingPivot;
    public Transform activeSkillContainer;
    public float moveSpeed = 5;
    public float airSpeed = 15;
    public float jumpForce = 10;
    public float slidePower = 5;
    public float slideDrag = 0;
    public float gravityPower = 9;
    public float airDrag = 0.2f;
    public float forwardRayDist = 1;
    public float timeBeforeDying = 6;
    public bool canJump = true;
    public bool canUseSkill = true;
    public bool gravityEnabled = true;
    public bool airAccelEnabled = false;
    public bool disableDeathByAltitude = false;
    [HideInInspector]
    public ActiveBase active;

    //Private
    [HideInInspector]
    public new Rigidbody rigidbody;
    [HideInInspector]
    public Animator anim;
    CapsuleCollider col;

    PlayerSounds playerSounds;

    [HideInInspector]
    public Vector3 gravity;
    Vector3 slideDirection;
    Vector3 airDirection;
    Vector3 hitNormal;
    Vector3 externalForce;

    [HideInInspector]
    public float gravityForce;
    float slideForce;
    float airForce;

    bool grounded;
    bool lastFrameGrounded;
    bool lastFrameFlying;

    float deathTimer = 0;

    // Use this for initialization
    void Awake () {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<CapsuleCollider>();
        playerSounds = GetComponent<PlayerSounds>();
	}
	
	// Update is called once per frame
	void Update () {
        SetGroundCondition();

        if (grounded)
        {
            if (lastFrameFlying)
            {
                slideForce = Vector3.ProjectOnPlane(airDirection, hitNormal).magnitude;
                airForce = 0;
                airDirection = Vector3.zero;
                gravityForce = 0;
                anim.SetFloat("left", Random.Range(0.06f, 1));
                anim.SetBool("grounded", true);
                anim.SetFloat("back", 0);
                skate.transform.parent = transform;
                skate.BounceSkate();
                col.height = 2;
                flyingPivot.localEulerAngles = Vector3.zero;
                gravity = Vector3.zero;
                playerSounds.PlayLandingSound();
                playerSounds.ChangeSkatePitchGround();
            }
            slideForce += slidePower * Time.deltaTime;
            slideForce /= 1 + slideDrag * Time.deltaTime;

            slideDirection *= slideForce;

            //gravity = -transform.up;

            //Mouse/Touch input
            Vector3 rotation = Vector3.zero;
            rotation.y = InputManager.GetHorizontalInput() * Time.deltaTime * moveSpeed;
            transform.Rotate(rotation, Space.Self);
            anim.transform.localEulerAngles = new Vector3(anim.transform.localEulerAngles.x, InputManager.GetHorizontalInput() * 20, -InputManager.GetHorizontalInput() * 20);


            //Keyboard input
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0), Space.Self);
            //anim.transform.localEulerAngles = new Vector3(anim.transform.localEulerAngles.x, Input.GetAxis("Horizontal") * 20, -Input.GetAxis("Horizontal") * 20);

            if (InputManager.GetJumpInput() && canJump)
            {
                anim.SetFloat("back", Random.Range(0.06f, 1));

                if (anim.GetFloat("back") > 0.5)
                    playerSounds.PlayBackFlipSound();
                else playerSounds.PlayFlipSound();

                gravityForce = jumpForce;
                gravity = transform.up * gravityForce;
            }

            if (Input.GetButtonDown("Jump") && canJump)
            {
                anim.SetFloat("back", Random.value);

                if (anim.GetFloat("back") > 0.5)
                    playerSounds.PlayBackFlipSound();
                else playerSounds.PlayFlipSound();

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
                anim.transform.localEulerAngles = Vector3.zero;
                anim.SetBool("grounded", false);
                col.height = 0.5f;
                skate.SetSkateBelowFeet();
                deathTimer = 0;
                playerSounds.ChangeSkatePitchAir();
            }

            if (gravityEnabled)
            {
                gravityForce -= gravityPower * Time.deltaTime;
            }
            else
            {
                gravityForce = 0;
            }

            if (airAccelEnabled)
            {
                airForce += slidePower * Time.deltaTime;
            }

            airDirection = airForce * transform.forward;

            //Mouse/Touch input
            Vector3 rotation = Vector3.zero;
            rotation.y = InputManager.GetHorizontalInput() * Time.deltaTime * airSpeed;
            transform.Rotate(rotation, Space.Self);
            flyingPivot.localEulerAngles = new Vector3(flyingPivot.localEulerAngles.x, flyingPivot.localEulerAngles.y, -InputManager.GetHorizontalInput());

            //Keyboard input
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * airSpeed, 0), Space.Self);
            //flyingPivot.localEulerAngles = new Vector3(flyingPivot.localEulerAngles.x, flyingPivot.localEulerAngles.y, -Input.GetAxis("Horizontal") * 20);


            if (InputManager.GetJumpInput() && !active.used && canUseSkill)
            {
                active.Use();
            }

            else if (InputManager.GetJumpInput() && active.used && canUseSkill)
            {
                active.Disable();
            }


            if (Input.GetButtonDown("Jump") && !active.used && canUseSkill)
            {
                active.Use();
            }

            else if (Input.GetButtonDown("Jump") && active.used && canUseSkill)
            {
                active.Disable();
            }

            gravity = transform.up * gravityForce;
            gravityForce /= 1 + airDrag * Time.deltaTime;
            airForce /= 1 + airDrag * Time.deltaTime;

            if (!disableDeathByAltitude && !Physics.Raycast(transform.position, -transform.up))
                deathTimer += Time.deltaTime;

            if (deathTimer > timeBeforeDying)
            {
                GameManager.Instance.isDead = true;
            }
        }

        rigidbody.velocity = slideDirection + airDirection + gravity + externalForce;

        if(externalForce.sqrMagnitude > 0.5f)
        {
            externalForce = Vector3.Lerp(externalForce, Vector3.zero, Time.deltaTime * 2);
        }
        else
        {
            externalForce = Vector3.zero;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            AddForce(new Vector3(0, 30, 0), true,false);
        }

        lastFrameGrounded = grounded;
        lastFrameFlying = !grounded;
        
	}

    void SetGroundCondition()
    {
        RaycastHit downHit;
        RaycastHit frontHit;

        bool down = Physics.Raycast(transform.position, -transform.up, out downHit, 1.3f, slideMask);
        bool up = Physics.Raycast(transform.position + transform.forward * forwardRayDist, -transform.up, out frontHit, 2f, slideMask);

        Debug.DrawRay(transform.position, -transform.up * (1.3f), Color.red);
        Debug.DrawRay(transform.position + transform.forward * forwardRayDist, -transform.up * 2, Color.blue);

        if (down && up)
        {
            slideDirection = (frontHit.point - downHit.point).normalized;
            grounded = true;
            hitNormal = downHit.normal;
            transform.rotation = Quaternion.LookRotation(slideDirection, hitNormal);
            Debug.DrawRay(downHit.point, slideDirection * 5, Color.green);

            if (lastFrameFlying)
            {
                transform.position += transform.up * (1.29f - downHit.distance);
            }
        }
        else
        {
            grounded = false;
        }

    }

    public void AddForce(Vector3 force, bool relative, bool overrideVelocity)
    {
        externalForce += relative ? transform.TransformDirection(force) : force;
        if (overrideVelocity)
        {
            rigidbody.velocity = externalForce;
            slideForce = 0;
            airForce = 0;
        }
        else
            rigidbody.velocity += externalForce;
    }
}
