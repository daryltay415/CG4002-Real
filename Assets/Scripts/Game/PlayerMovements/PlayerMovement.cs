
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isAttackingHash = 0;

    // attack variables
    bool isAttackPressed = false;

    //getter and setter
    //public PlayerBaseState currentState { get { return _currentState; } set { _currentState = value; } }
    //public Animator _animator{get{ return animator; }}

    // Attack variables
    //public int _isAttackingHash {get{ return isAttackingHash; }set{ isAttackingHash = value; }}
    //public bool _isAttackPressed {get{ return isAttackPressed; }}
    //public int _stillAttacking {get{ return stillAttacking; }set{ stillAttacking = value; }}

    //public CharacterController _characterController {get{ return characterController; }}
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isAttackingHash = Animator.StringToHash("Attack");
        playerInput.CharacterControls.Jab.performed += onAttack;
        playerInput.CharacterControls.Jab.canceled += onAttack;
        
    }

    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }

    void handleAttackAnimation()
    {
        if (isAttackPressed)
        {
            animator.SetInteger(isAttackingHash, 1);
        }
        else
        {
            animator.SetInteger(isAttackingHash, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleAttackAnimation();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
