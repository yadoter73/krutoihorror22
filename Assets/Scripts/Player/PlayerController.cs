using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;
    [SerializeField] private CrouchController _crouchController;
    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isGrounded;


    public Coroutine StepCoroutine;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _movementController.Initialize(_characterController, this);
        _crouchController.Initialize(_characterController);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;
        _movementController.Move(_isGrounded);
        _crouchController.HandleCrouch();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _movementController.Gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
