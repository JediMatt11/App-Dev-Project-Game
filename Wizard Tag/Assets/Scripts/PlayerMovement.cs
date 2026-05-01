using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 10f;
    public float sprintSpeed;
    public Vector3 moveDirection;
    public float horizontal;
    public float vertical;
    public KeyCode sprintKey;
    public bool isSprinting;
    public float tempSpeed;
    public float dashForce;
    public float dashDuration = 0.2f;
    public float baseDashCooldown;
    public float dashCooldown = 1.0f;
    public float jumpForce = 5f;
    public float gravityVal;
    public float groundedGravity;
    public float jumpedTime;
    public bool isJumpDashing = false;
    public float baseJumpDashForce = 35f;
    public float jumpDashCooldownTime = 0.2f;
    public KeyCode airDashKey;
    public bool isSliding = false;
    public bool slidingCantMove = false;
    public bool canSlide = true;
    public Vector3 slideMoveVector;
    public float slideTime = 0.35f;
    public float slideCooldown = 1f;
    public float slideCooldownTime;
    public float slideSpeed = 35f;
    public bool justSlid;

    public bool isSlideJumping = false;
    public float checkForSlideJumpCooldown = 0.08f;
    public bool canCheckForSlideJumpGround = false;
    public float baseSlideJumpForce = 30f;
    public Camera playerCamera;

    public CharacterController characterController;
    public float groundedCheckDistance = 0.1f;
    public Vector3 velocity;
    public bool isDashing = false;
    public bool canJump = true;
    public Animator animator;
    
    public AudioSource audioSource;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Movement();
        MovementInput();
        Jump();
        AnimationProperties();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        if (dashCooldown > 0f)
        {
            dashCooldown -= Time.fixedDeltaTime;
        }
        if (slideCooldownTime > 0f)
        {
            slideCooldownTime -= Time.fixedDeltaTime;
        }
    }

    public void MovementInput()
    {
        if (!isDashing && !slidingCantMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(airDashKey) && !isGrounded())
        {
            if (dashCooldown <= 0f)
            {
                isDashing = true;
                StartCoroutine(Dash());
                dashCooldown = baseDashCooldown;
            }
        }
        if (isGrounded())
        {
            dashCooldown = 0f;
        }

        if (Input.GetKey(sprintKey) && !isSliding && isGrounded())
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    public void Movement()
    {
        Slide();
        moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;
        float currentSpeed = baseSpeed * (isJumpDashing ? baseJumpDashForce : isSlideJumping ? baseSlideJumpForce : isSliding ? slideSpeed : isDashing ? dashForce : isSprinting ? sprintSpeed : 1f);
        moveDirection *= currentSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }


    public void Jump()
    {
        if (isGrounded() && velocity.y <= 0f)
        {
            canJump = true;
            isJumpDashing = false;
            if (canCheckForSlideJumpGround)
            {
                isSlideJumping = false;
            }
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            canJump = false;
            jumpedTime = Time.time;
            audioSource.Play();
        }
        if (Time.time - jumpedTime <= jumpDashCooldownTime)
        {
            jumpDashCooldown();
        }
        velocity.x = 0;
        velocity.z = 0;
        characterController.Move(velocity * Time.deltaTime);
    }

    public IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    public void jumpDashCooldown()
    {        
        if (isDashing)
        {
            isJumpDashing = true;
            tempSpeed = baseJumpDashForce * baseSpeed;
        }
    }

    public void Slide()
    {

        if (Input.GetKeyDown(KeyCode.C) && !isSliding && isSprinting && isGrounded() && canSlide) //&& (vertical * transform.forward).magnitude > 0
        {
            justSlid = true;
            isSliding = true;
            canSlide = false;
            StartCoroutine(slideTimer());
        }

        if (isSliding)
        {
            slideMoveVector = transform.right * horizontal + transform.forward;
            slideMoveVector.Normalize();
            if (!canSlide && Input.GetKeyDown(KeyCode.Space))
            {
                isSlideJumping = true;
                tempSpeed = baseSlideJumpForce * baseSpeed;
                StartCoroutine(slideJumpCooldown());
            }
        }
    }

    public IEnumerator slideTimer()
    {
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
        yield return new WaitForSeconds(slideCooldown - slideTime);
        canSlide = true;
    }


    public IEnumerator slideJumpCooldown()
    {
        canCheckForSlideJumpGround = false;
        yield return new WaitForSeconds(checkForSlideJumpCooldown);
        canCheckForSlideJumpGround = true;
    }
    void ApplyGravity()
    {
        if (!isGrounded())
        {
            velocity.y += gravityVal * Time.fixedDeltaTime;
        }
        else if (canJump)
        {
            velocity.y = groundedGravity;
        }
    }

    public bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, characterController.height / 2 + groundedCheckDistance);
    }

    public void AnimationProperties()
    {
        if (moveDirection.magnitude == 0 && isGrounded())
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        else if (isSprinting && isGrounded())
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
        }
        if (isSliding && justSlid)
        {
            justSlid = false;
            animator.SetTrigger("Slide");
        }
    }

}
