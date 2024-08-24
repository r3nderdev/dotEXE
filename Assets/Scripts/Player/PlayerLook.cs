using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerLook : MonoBehaviour
{
    public static float sensitivity = 5f;

    [SerializeField] private InputAction lookAction;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform cameraPosition;

    private float xRotation;
    private float yRotation;

    private Camera cam;

    private void OnEnable()
    {
        lookAction.performed += OnLook;
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.performed -= OnLook;
        lookAction.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
    }

    private void Update()
    {
        transform.position = cameraPosition.position;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        float mouseX = input.x * sensitivity;
        float mouseY = input.y * sensitivity;

        // Camera Rotation
        yRotation += mouseX * Time.deltaTime;
        xRotation -= mouseY * Time.deltaTime;

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