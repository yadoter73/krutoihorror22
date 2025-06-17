using Cinemachine;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class MovementController
{

    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _airControlMultiplier = 0.5f;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _stepInterval;
    public float Gravity = -9.81f;

    private CharacterController _characterController;
    private PlayerController _playerController;

    private Transform _head;
    private float _speed;

    public void Initialize(CharacterController characterController, PlayerController playerController)
    {
        _characterController = characterController;
        _playerController = playerController;
        _head = Camera.main.transform;
    }
    public void Move(bool isGrounded)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = _head.right * horizontal + _head.forward * vertical;

        _speed = Input.GetButton("Run") ? _runSpeed : _walkSpeed;

        if (!isGrounded)
        {
            _speed *= _airControlMultiplier;
        }

        _characterController.Move(moveDirection * _speed * Time.deltaTime);

        bool isMoving = moveDirection.magnitude > 0.8f;



        if (isMoving && _playerController.StepCoroutine == null)
        {
            _playerController.StepCoroutine = _playerController.StartCoroutine(StepCoroutine());
        }
        else if (!isMoving && _playerController.StepCoroutine != null)
        {
            _playerController.StopCoroutine(_playerController.StepCoroutine);
            _playerController.StepCoroutine = null;
        }
    }

    private IEnumerator StepCoroutine()
    {
        while (true)
        {
            float stepInterval = _speed == _runSpeed ? _stepInterval * _walkSpeed / _runSpeed : _stepInterval;
            _impulseSource.GenerateImpulse(_speed == _runSpeed ? 1.1f : 1);

            float defaultVelocity = 0.15f;
            _impulseSource.m_DefaultVelocity.y = _speed == _runSpeed ? defaultVelocity * _walkSpeed / _runSpeed : defaultVelocity;
            _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = stepInterval;
            yield return new WaitForSeconds(stepInterval);
        }
    }
}
