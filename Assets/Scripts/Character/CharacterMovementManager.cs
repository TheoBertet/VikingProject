using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    public float walkingSpeed = 1;
    public float runningSpeed = 2;
    public float camMaxRot = 60;
    public float camMinRot = -20;
    public float fallingVelocityValue = -2;
    public float jumpForce = 5;

    private Rigidbody rigidbody;
    private Animator animator;
    private float movingSpeed = 1;
    private Transform focusPoint;
    private CinemachineVirtualCamera characterCamera;
    private bool isFalling = false;
    private bool isLanded = true;
    private bool isJumping = false;

    private CapsuleCollider standingCollider;
    private CapsuleCollider jumpingCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody>();
        animator = transform.GetComponent<Animator>();
        focusPoint = transform.Find("FocusPoint").transform;
        characterCamera = transform.Find("CharacterCamera").GetComponent<CinemachineVirtualCamera>();

        standingCollider = transform.Find("Colliders").Find("StandingCollider").GetComponent<CapsuleCollider>();
        jumpingCollider = transform.Find("Colliders").Find("JumpingCollider").GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Falling();
        if (!isFalling && isLanded)
        {
            Move();
            Jump();
        }
        Rotate();
        Animate();
        RotateUpCam();
    }

    private void Move()
    {
        if (Input.GetAxis("Run") > 0)
        {
            movingSpeed = runningSpeed;
        }
        else
        {
            movingSpeed = walkingSpeed;
        }

        rigidbody.velocity = transform.up * rigidbody.velocity.y + (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")) * movingSpeed;
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X"),
            transform.rotation.eulerAngles.z);
    }

    private void Jump()
    {
        if(Input.GetAxis("Jump") != 0)
        {
            animator.SetBool("isJumping", true);
        }

    }

    private void RotateUpCam()
    {
        float movement = Input.GetAxis("Mouse Y");
        float angle = focusPoint.rotation.eulerAngles.x;
        angle = (angle > 180) ? angle - 360 : angle;

        if (movement < 0 && angle <= camMaxRot ||
            movement > 0 && angle >= camMinRot)
        {
            focusPoint.rotation = Quaternion.Euler(focusPoint.rotation.eulerAngles.x - movement,
                focusPoint.rotation.eulerAngles.y,
                focusPoint.rotation.eulerAngles.z);
        }
    }

    private void Falling()
    {
        if(rigidbody.velocity.y <= fallingVelocityValue)
        {
            isFalling = true;
            isLanded = false;
        }
        else
        {
            isFalling = false;
        }
    }

    private void Animate()
    {
        // FALLING
        if (isFalling)
        {
            animator.SetBool("isFalling", true);
            // Setting off other animations possibly still activated
            if(!isLanded)
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingBack", false);
                animator.SetBool("isStrafingRight", false);
                animator.SetBool("isStrafingLeft", false);
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            animator.SetBool("isFalling", false);

            // MOVING FORWARD
            if (Input.GetAxis("Vertical") > 0)
            {
                animator.SetBool("isMoving", true);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                animator.SetBool("isMovingBack", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingBack", false);
            }

            // RUNNING FORWARD
            if (Input.GetAxis("Run") > 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            // MOVING RIGHT/LEFT
            if (Input.GetAxis("Horizontal") > 0)
            {
                animator.SetBool("isStrafingRight", true);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                animator.SetBool("isStrafingLeft", true);
            }
            else
            {
                animator.SetBool("isStrafingRight", false);
                animator.SetBool("isStrafingLeft", false);
            }
        }
    }

    private void JumpCollider()
    {
        jumpingCollider.enabled = true;
        standingCollider.enabled = false;
    }

    private void StandingCollider()
    {
        jumpingCollider.enabled = false;
        standingCollider.enabled = true;
    }

    private void CharacterLanded()
    {
        isLanded = true;
    }

    private void StartJump()
    {
        rigidbody.AddForce(transform.up * jumpForce + transform.forward * jumpForce/2 * movingSpeed, ForceMode.Impulse);
        //JumpCollider();
        //rigidbody.velocity = rigidbody.velocity + transform.up * jumpForce;
    }

    private void EndJump()
    {
        //StandingCollider();
        animator.SetBool("isJumping", false);
    }
}
