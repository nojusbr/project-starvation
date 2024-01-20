using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    public SelectionManager resourceGathering;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckMovement();

        if (Input.GetKey(KeyCode.E))
        {
            if (!resourceGathering.IsChopping) return;
            else CheckChopping();
        }
    }

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
        animator.SetBool("IsChopping", false);
    }

    private void PlayAnimation(string animationName)
    {
        StopCurrentAnimation();
        animator.SetBool(animationName, true);
    }

    public void CheckChopping()
    {
        if (resourceGathering.IsChopping)
        {
            PlayAnimation("IsChopping");
            StartCoroutine(ChopEffectRoutine(1.5f));
        }
        else
        {
            StopCurrentAnimation();
        }
    }
    private IEnumerator ChopEffectRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
}
