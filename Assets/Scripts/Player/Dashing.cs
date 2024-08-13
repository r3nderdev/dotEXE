using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    [SerializeField] private Slider slider;

    [Header("Dashing Values")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float maxDashYSpeed;
    [SerializeField] private float dashDuration;

    [Header("Camera Effects")]
    [SerializeField] private PlayerLook cam;
    [SerializeField] private float dashFov;

    [Header("Settings")]
    [SerializeField] private bool useCameraForward = true;
    [SerializeField] private bool allowAllDirections = true;
    [SerializeField] private bool disableGravity = false;
    [SerializeField] private bool resetVel = true;

    [Header("Cooldown")]
    [SerializeField] private float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    [SerializeField] private KeyCode dashKey = KeyCode.Mouse1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        slider.maxValue = dashCd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey))
            Dash();

        if(dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;

        if (dashCdTimer > 0)
        {
            slider.gameObject.SetActive(true);
            slider.value = dashCdTimer;
        }
        else
            slider.gameObject.SetActive(false);
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        cam.DoFow(dashFov);

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        if(resetVel)
            rb.velocity= Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0;

        cam.DoFow(90f);

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3();

        if (allowAllDirections)
        {
            direction = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        }
        else
        {
            direction = forwardT.forward;
        }

        if(verticalInput == 0 && horizontalInput == 0)
        {
            direction = forwardT.forward;
        }

        return direction.normalized;
    }
}
