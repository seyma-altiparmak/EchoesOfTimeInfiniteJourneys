using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //03.04
    public static PlayerController Instance
    {
        get;
        private set;
    }

    public event EventHandler OnInteractAction;
    public event EventHandler<OnSelectGarbageChangeEventArgs> OnSelectGarbageChanged;

    public class OnSelectGarbageChangeEventArgs : EventArgs
    {
        public Garbage _garbage;
    }

    [Header("Player Move Settings")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float fastMoveSpeed = 7f; // H�zl� gitme h�z�
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask garbageLayerMask;

    private Rigidbody _rigidbody;
    private PlayerInput _playerInput;
    private Animator _animator;
    private Garbage _garbage;

    private Vector2 _movementInput;
    private bool _isJumping;
    private bool _isSprinting; // Sprint durumu
    private bool _isInteract;

    private Vector3 lastInteractDir;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Jump"].performed += context => _isJumping = true;
        _playerInput.actions["Movement"].performed += context => _movementInput = context.ReadValue<Vector2>();
        _playerInput.actions["Movement"].canceled += context => _movementInput = Vector2.zero;
        _playerInput.actions["Interact"].performed += Interact_performed;
        //_playerInput.actions["Interact"].canceled += context => _isInteract = false;
        _playerInput.actions["Sprint"].performed += context => _isSprinting = true;
        _playerInput.actions["Sprint"].canceled += context => _isSprinting = false;
    }

    private void Start()
    {
        OnInteractAction += GameInput_OnInteractAction;
    }
    private void FixedUpdate()
    {
        MovePlayer();
        HandleOInteractions();
        if (_isJumping)
        {
            Jump();
            _isJumping = false;
        }
    }

    private void HandleOInteractions()
    {
        Vector2 inputVector = _playerInput.actions["Movement"].ReadValue<Vector2>();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 4f;
        if(Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit , interactDistance,garbageLayerMask))
        {
           if( raycastHit.transform.TryGetComponent(out Garbage garbage))
            {
                //Garbage var m�?
                if(garbage != _garbage)
                {
                    SetSelectedGarbage(garbage); 
                }
               
            }
            else
            {
                SetSelectedGarbage(null);
            }
        }
        else
        {
            SetSelectedGarbage(null);
        }
        Debug.Log(_garbage);
    }

    private void MovePlayer()
    {
        float moveSpeed = _isSprinting ? fastMoveSpeed : normalMoveSpeed;
        Vector3 movement = new Vector3(_movementInput.x, 0, _movementInput.y);
        movement = movement.normalized * moveSpeed * Time.fixedDeltaTime;
        float playerSize = 5f;


        bool canMove = !Physics.Raycast(transform.position, movement, playerSize);
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }

        transform.Translate(movement, Space.World);
        
        //Animator controller
        if (moveSpeed == fastMoveSpeed) _animator.SetBool("isRun", true);
        else if(moveSpeed == normalMoveSpeed)
        {
            _animator.SetBool("isWalk", true);
            _animator.SetBool("isRun", false);
        }
        else
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", false);
        }
    }

    private void Jump()
    {
        StartCoroutine(JumpDelay());
    }

    IEnumerator JumpDelay()
    {
        RaycastHit hit;
        float raycastDistance = 0.2f; // Adjust this value to your needs
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.distance <= raycastDistance)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        yield return new WaitForSeconds(2f);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_garbage != null) _garbage.Interact();
    }
   
    private void SetSelectedGarbage(Garbage _garbage)
    {
        this._garbage = _garbage;

        OnSelectGarbageChanged?.Invoke(this, new OnSelectGarbageChangeEventArgs
        {
            _garbage = _garbage
        });
    }
}
