using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachineMultiplayer : NetworkBehaviour
{

    // Camera variables
    private Camera mainCamera;
    private Vector3 lastPosition;
    bool camIsMoving = false;
    float tolerance = 0.1f;

    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    NetworkAnimator networkAnimator;
    bool isMoving;

    int isWalkingHash;
    int isAttackingHash;


    // Attack variables

    bool isAttackPressed = false;
    PlayerBaseStateMultiplayer _currentState;
    PlayerStateFactoryMultiplayer states;

    //getter and setter
    public PlayerBaseStateMultiplayer currentState { get { return _currentState; } set { _currentState = value; } }
    public Animator _animator{get{ return animator; }}
    public CharacterController _characterController {get{ return characterController; }}
    public bool _isMovingPressed {get{ return isMoving; }}
    public int _isWalkingHash {get{ return isWalkingHash; } set { isWalkingHash = value; }}
    public bool _camIsMoving {get{return camIsMoving;}}

    // Attack variables
    public int _isAttackingHash {get{ return isAttackingHash; }set{ isAttackingHash = value; }}
    public bool _isAttackPressed {get{ return isAttackPressed; }}
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            mainCamera = Camera.main;
            playerInput = new PlayerInput();
            playerInput.CharacterControls.Enable();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            networkAnimator = GetComponent<NetworkAnimator>();
            networkAnimator.Animator = animator;
            // Set up states
            states = new PlayerStateFactoryMultiplayer(this);
            _currentState = states.Idle();
            _currentState.EnterState();
            isAttackingHash = Animator.StringToHash("Attack");
            isWalkingHash = Animator.StringToHash("Walk");
            //playerInput.CharacterControls.Move.started += onMovementInput;
            //playerInput.CharacterControls.Move.canceled += onMovementInput;
            //playerInput.CharacterControls.Move.performed += onMovementInput;
            playerInput.CharacterControls.Jab.performed += onAttack;
            playerInput.CharacterControls.Jab.canceled += onAttack;
        }
    }


    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
        //if (isAttackPressed)
        //{
        //    animator.SetInteger(isAttackingHash,1);
        //}
        //else
        //{
        //    animator.SetInteger(isAttackingHash,0);
        //}
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        // change to adapt to camera movement
        //curMovementInput = context.ReadValue<Vector2>();
        //isMoving = curMovementInput.x != 0 || curMovementInput.y != 0;
    }

    void CameraStatus()
    {
        
        // Check if camera is moving
        Vector3 difference = mainCamera.transform.position - lastPosition;
        float sqrDistance = difference.sqrMagnitude;
        float sqrTolerance = tolerance*tolerance;
        //float distance = Vector3.Distance(mainCamera.transform.position, lastPosition);
        if (sqrDistance <= sqrTolerance)
        {
            camIsMoving = false;

        }
        else
        {
            camIsMoving = true;
            lastPosition = mainCamera.transform.position;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            CameraStatus();
            currentState.UpdateStates();
        }
    }


    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            playerInput.CharacterControls.Jab.performed -= onAttack;
            playerInput.CharacterControls.Jab.canceled -= onAttack;
        }
    }

    //void OnEnable()
    //{
    //    
    //    playerInput.CharacterControls.Enable();
    //}
//
    //void OnDisable()
    //{
    //    playerInput.CharacterControls.Disable();
    //}

}
