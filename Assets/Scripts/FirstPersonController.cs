using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class FirstPersonController : MonoBehaviour
{

    public float movementSpeed = 2;
    public float gravity = -9.8f;
    public float sensitivity = 0.2f;
    public float minLimit = -80f;
    public float maxLimit = 80;

    private float _currentRotationY;
    
    public Transform cameraTransform;
    private PlayerInputAction _inputAction;
    private CharacterController _characterController;

    private Vector2 _movement;
    private Vector2 _velocity;
    private Vector2 _look;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        _inputAction.Player.Enable();

        _inputAction.Player.Move.performed += SetMovement;
        _inputAction.Player.Move.canceled += obj => _movement = Vector2.zero;

        _inputAction.Player.Look.performed += SetLook;
        _inputAction.Player.Look.canceled += obj => _look = Vector2.zero;
    }

    private void SetLook(InputAction.CallbackContext obj)
    {
        _look = obj.ReadValue<Vector2>();
    }

    private void SetMovement(InputAction.CallbackContext obj)
    {
        _movement = obj.ReadValue<Vector2>();
    }

    private void Update()
    {
        Movement();
        Look();
    }

    private void Look()
    {
        Vector2 mouseNormalized = _look * sensitivity;

        _currentRotationY = Mathf.Clamp(_currentRotationY - mouseNormalized.y, minLimit, maxLimit);

        cameraTransform.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);
        transform.Rotate(Vector3.up * _look.x);
    }

    private void Movement()
    {
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;
        _characterController.Move(move * movementSpeed * Time.deltaTime);

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
