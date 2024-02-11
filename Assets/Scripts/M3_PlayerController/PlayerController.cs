using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Move Settings")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float fastMoveSpeed = 7f; // Hýzlý gitme hýzý
    [SerializeField] private float jumpForce = 2f;

    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;

    private Vector2 _movementInput;
    private bool _isJumping;
    private bool _isSprinting; // Sprint durumu

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Jump"].performed += context => _isJumping = true;
        _playerInput.actions["Movement"].performed += context => _movementInput = context.ReadValue<Vector2>();
        _playerInput.actions["Movement"].canceled += context => _movementInput = Vector2.zero;

        _playerInput.actions["Sprint"].performed += context => _isSprinting = true;
        _playerInput.actions["Sprint"].canceled += context => _isSprinting = false;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (_isJumping)
        {
            Jump();
            _isJumping = false;
        }
    }

    private void MovePlayer()
    {
        float moveSpeed = _isSprinting ? fastMoveSpeed : normalMoveSpeed;
        Vector3 movement = new Vector3(_movementInput.x, 0, _movementInput.y) * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.World);
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

   
}
