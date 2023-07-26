using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float jumpForce = 200.0f;
    public float mouseSensitivity = 2.0f;

    private CharacterController characterController;
    private Camera playerCamera;
    private bool isSprinting;
    private float verticalSpeed = 0f;
    private float rotationX = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool wasGrounded = characterController.isGrounded;
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        float moveSpeed = isSprinting ? sprintSpeed : walkSpeed;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection *= moveSpeed;

        if (characterController.isGrounded)
        {
            // Apply friction to stop sliding on slopes
            verticalSpeed = -0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                verticalSpeed = jumpForce;
            }
        }
        else
        {
            verticalSpeed += Physics.gravity.y * Time.deltaTime;
        }

        moveDirection.y = verticalSpeed;

        characterController.Move(moveDirection * Time.deltaTime);

        // First-person looking
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX += mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Rotate the player's body left/right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up/down (inverted due to convention)
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}