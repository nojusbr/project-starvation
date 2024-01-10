using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    public FirstPersonMovement movement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<FirstPersonMovement>();
    }

    private void Update()
    {
        CheckMovement();
    }

    //private void CheckDash()
    //{
    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    float verticalInput = Input.GetAxis("Vertical");
    //    bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

    //    if (isMoving)
    //    {
    //        StopCurrentAnimation();

    //        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
    //        {
    //            if (horizontalInput > 0.1f)
    //            {
    //                PlayAnimation("IsRollingLeft");
    //            }
    //            else if (horizontalInput < -0.1f)
    //            {
    //                PlayAnimation("IsRollingRight");
    //            }
    //        }
    //        else
    //        {
    //            if (verticalInput > 0.1f)
    //            {
    //                PlayAnimation("IsRollingFw");
    //                Debug.Log("ROLLING FORWARD");
    //            }
    //            else if (verticalInput < -0.1f)
    //            {
    //                PlayAnimation("IsRollingBckw");
    //                Debug.Log("ROLLING BACKWARDS");
    //            }
    //        }
    //    }
    //    else
    //    {
    //        StopCurrentAnimation();
    //    }
    //}

    private void CheckMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        if (isMoving)
        {
            StopCurrentAnimation();

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                if (horizontalInput > 0.1f)
                {
                    PlayAnimation("IsRunningLeft");
                }
                else if (horizontalInput < -0.1f)
                {
                    PlayAnimation("IsRunningRight");
                }
            }
            else
            {
                if (verticalInput > 0.1f)
                {
                    PlayAnimation("IsRunning");
                }
                else if (verticalInput < -0.1f)
                {
                    PlayAnimation("IsRunningBackwards");
                }
            }
        }
        else
        {
            StopCurrentAnimation();
        }
    }

    private void StopCurrentAnimation()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsRunningRight", false);
        animator.SetBool("IsRunningLeft", false);
        animator.SetBool("IsRunningBackwards", false);
    }

    private void PlayAnimation(string animationName)
    {
        StopCurrentAnimation();
        animator.SetBool(animationName, true);
    }
}
