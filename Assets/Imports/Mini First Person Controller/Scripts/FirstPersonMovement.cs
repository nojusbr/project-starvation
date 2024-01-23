using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;
    Rigidbody rb;

    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    public Transform capsuleMesh;
    public ParticleSystem dashEffect;
    public GameObject teleporterD;
    public GameObject teleporterR;
    public Grid grid;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;
    public bool isDashing;

    [Header("Cooldown")]
    public float dashCd;
    public float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.Space;


    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        Move();

        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }


        if (dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
            isDashing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TeleporterD"))
        {
            transform.position = teleporterD.transform.position;
        }
        if (collision.gameObject.CompareTag("TeleporterR"))
        {
            transform.position = teleporterR.transform.position;
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Move()
    {
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }


        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }

    public void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        Vector3 playerVelocity = rb.velocity.normalized;
        Vector3 forceToApply = playerVelocity * dashForce + orientation.up; 
        rb.AddForce(forceToApply, ForceMode.Impulse);
        dashEffect.Play();
        Invoke("ResetRotation", 1f);
        isDashing = true;
    }
}