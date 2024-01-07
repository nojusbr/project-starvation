using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    public KeyCode dashKey = KeyCode.Space;

    public bool canRun = true;
    public bool canDash = true;
    public bool IsRunning { get; private set; }
    public bool IsNotDashing { get; private set; }

    public float dashSpeed = 15f; // Adjust the speed of the dash as needed
    public float dashDuration = 1f; // Duration of the dash in seconds

    private bool isDashing = false;
    private float dashTimer = 0f;
    private Rigidbody rigidbody;

    public ParticleSystem dashEffect;

    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);
        IsNotDashing = canDash && Input.GetKey(dashKey);

        // Check if the player initiates a dash
        if (IsNotDashing && Input.GetKeyDown(dashKey) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
        }

        // Perform the dash
        if (isDashing)
        {
            // Reduce the timer for the dash duration
            dashTimer -= Time.deltaTime;
            dashEffect.Play();


            // Check if the dash duration has elapsed
            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashTimer = 0f;

            }

            // Calculate the velocity for dash movement
            Vector3 dashVelocity = transform.forward * dashSpeed;

            // Apply the dash movement
            rigidbody.velocity = dashVelocity;
        }
        else
        {
            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

            // Apply regular movement if not dashing
            rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        }
    }
}
