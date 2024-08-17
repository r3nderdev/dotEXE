using UnityEngine;
using DG.Tweening;

public class PlayerLook : MonoBehaviour
{
    public static float mouseSensitivity = 50f;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform cameraPosition;

    private float xRotation;
    private float yRotation;

    private Camera cam;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    private void Update()
    {
        // Move camera position
        transform.position = cameraPosition.position;

        // Get raw mouse input
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Scale mouse input by sensitivity
        mouseX *= mouseSensitivity;
        mouseY *= mouseSensitivity;

        // Adjust rotation
        yRotation += mouseX * Time.deltaTime; // Use Time.deltaTime only for rotation application
        xRotation -= mouseY * Time.deltaTime; // Use Time.deltaTime only for rotation application

        // Clamp vertical rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFow(float endValue)
    {
        cam.DOFieldOfView(endValue, 0.25f);
    }
}