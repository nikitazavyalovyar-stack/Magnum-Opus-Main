using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 50f; // НИЗКАЯ чувствительность!
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;
    
    private Vector2 moveInput;
    private Vector3 velocity;
    private float xRotation = 0f;
    private bool isGrounded;
    
    private CharacterController characterController;
    private Camera playerCamera;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        isGrounded = characterController.isGrounded;
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // Движение
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
        
        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void OnLook(InputValue value)
    {
        Vector2 look = value.Get<Vector2>();
        
        // Debug - смотри в Console что приходит
        // Debug.Log($"Mouse input: X={look.x}, Y={look.y}");
        
        // Вращение персонажа (влево-вправо)
        float yaw = look.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(0, yaw, 0);
        
        // Вращение камеры (вверх-вниз)
        float pitch = -look.y * mouseSensitivity * Time.deltaTime;
        xRotation += pitch;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    void OnJump(InputValue value)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }
}