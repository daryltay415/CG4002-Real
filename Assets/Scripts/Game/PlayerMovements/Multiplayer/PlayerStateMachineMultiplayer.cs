using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class PlayerStateMachineMultiplayer : NetworkBehaviour
{

    // Camera variables
    private Camera mainCamera;
    private Vector3 lastPosition;
    bool camIsMoving = false;
    float tolerance = 0.1f;

    public LayerMask hitLayer;
    public GameObject lefthand;
    public GameObject righthand;
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;
    NetworkAnimator networkAnimator;
    NetworkObject networkobj;
    // Guard variables
    bool isGuarding;
    bool isMoving;

    int isWalkingHash;
    int isAttackingHash;
    int isGuardingHash;
    int isDamagedHash;

    //Dmg variables
    public int takingDmg = 0;


    // Attack variables
    public float punchRange = 1.0f;
    bool isAttackPressed = false;
    PlayerBaseStateMultiplayer _currentState;
    PlayerStateFactoryMultiplayer states;
    public int stillAttacking;

    public static event Action<(ulong from, ulong to)> OnHitPlayer; 
    //getter and setter
    public PlayerBaseStateMultiplayer currentState { get { return _currentState; } set { _currentState = value; } }
    public Animator _animator{get{ return animator; }}
    public CharacterController _characterController {get{ return characterController; }}
    public bool _isMovingPressed {get{ return isMoving; }}
    public bool _isGuardingPressed {get{return isGuarding;}}
    public int _isWalkingHash {get{ return isWalkingHash; } set { isWalkingHash = value; }}
    public int _isGuardingHash {get{return isGuardingHash; } set { isGuardingHash = value;}}
    public int _isDamagedHash {get{return isDamagedHash;} set { isDamagedHash = value;}}
    public bool _camIsMoving {get{return camIsMoving;}}
    public int _takingDmg {get{return takingDmg;} set {takingDmg = value;}}

    // Attack variables
    public int _isAttackingHash {get{ return isAttackingHash; }set{ isAttackingHash = value; }}
    public bool _isAttackPressed {get{ return isAttackPressed; }}
    public int _stillAttacking {get{ return stillAttacking; }set{ stillAttacking = value; }}
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            mainCamera = Camera.main;
            playerInput = new PlayerInput();
            playerInput.CharacterControls.Enable();
            //childHitbox = GetComponentInChildren<PlayerHitbox>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            networkAnimator = GetComponent<NetworkAnimator>();
            networkAnimator.Animator = animator;
            // Set up states
            states = new PlayerStateFactoryMultiplayer(this);
            currentState = states.Idle();
            currentState.EnterState();
            isAttackingHash = Animator.StringToHash("Attack");
            isWalkingHash = Animator.StringToHash("Walk");
            isGuardingHash = Animator.StringToHash("Guard");
            isDamagedHash = Animator.StringToHash("Damage");
            //playerInput.CharacterControls.Move.started += onMovementInput;
            //playerInput.CharacterControls.Move.canceled += onMovementInput;
            //playerInput.CharacterControls.Move.performed += onMovementInput;
            playerInput.CharacterControls.Jab.performed += onAttack;
            playerInput.CharacterControls.Jab.canceled += onAttack;
            playerInput.CharacterControls.Guard.performed += onGuard;
            playerInput.CharacterControls.Guard.canceled += onGuard;
        }
        //PlayerDataManager.Instance.OnPlayerDead += playerDead;
        networkobj = GetComponent<NetworkObject>();
    }


    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }

    void onGuard(InputAction.CallbackContext context)
    {
        isGuarding = context.ReadValueAsButton();
        PlayerDataManager.Instance.PlayerGuardStateServerRpc(networkobj.OwnerClientId,isGuarding);
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
            playerInput.CharacterControls.Guard.performed -= onGuard;
            playerInput.CharacterControls.Guard.canceled -= onGuard;
        }
        //PlayerDataManager.Instance.OnPlayerDead -= playerDead;
    }

    //void playerDead(ulong id)
    //{
    //    if(networkobj.OwnerClientId == id)
    //    {
    //        // set player is dead variable to true;
    //        Debug.Log("Player" + id + "has died");
    //    }
    //}

    public void RaycastPunch(int handChoice)
    {
        GameObject hand;
        switch (handChoice)
        {
            case 1:
                hand = righthand;
                break;
            default:
                hand = lefthand;
                break;
        }
        RaycastHit hit;
        // Project a ray from the hand forward
        Vector3 direction = transform.forward;
        
        // Visualize the ray in the scene view for debugging
        Debug.DrawRay(hand.transform.position, direction * punchRange, Color.red, 0.5f);

        if (Physics.Raycast(hand.transform.position, direction, out hit, punchRange, hitLayer))
        {
            Debug.Log("Actual hit: " + hit.collider.name);
            CollisionOnObject(hit);
        }
    }

    public void DistancePunch(int handChoice)
    {
        if (!IsOwner) return;

        // Use the hand's localPosition relative to the SharedArRoot
        GameObject hand;
        switch (handChoice)
        {
            case 1:
                hand = righthand;
                break;
            default:
                hand = lefthand;
                break;
        }
        Vector3 myHandLocalPos = transform.parent.InverseTransformPoint(hand.transform.position);
        // We need the local position of the hand relative to the IMAGE (the parent of the player)

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId == networkobj.OwnerClientId) continue;

            // Get the enemy's position relative to the SAME Batman image
            Vector3 enemyLocalPos = client.PlayerObject.transform.localPosition;

            // 1. Calculate Horizontal distance in the shared space
            float horizontalDist = Vector2.Distance(
                new Vector2(myHandLocalPos.x, myHandLocalPos.z), 
                new Vector2(enemyLocalPos.x, enemyLocalPos.z)
            );

            // 2. Vertical check (so you can't hit them if they duck/jump too far)
            float verticalDiff = Mathf.Abs(myHandLocalPos.y - enemyLocalPos.y);

            if (horizontalDist <= 0.2f && verticalDiff <= 0.6f)
            {
                // The hit is mathematically "true" in the shared coordinate system
                (ulong, ulong) fromPlayerToEnemey = new(networkobj.OwnerClientId, client.ClientId);
                OnHitPlayer?.Invoke(fromPlayerToEnemey);
                break;
            }
        }
    }

    public void CollisionOnObject(RaycastHit collision)
    {
        if (IsServer)
        {
            if (collision.transform.TryGetComponent(out NetworkObject networkObject))
            {
                if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player") && networkObject.OwnerClientId != networkobj.OwnerClientId)
                {
                    Debug.Log("hand has Collision to player");
                    (ulong, ulong) fromPlayerToEnemey = new(networkobj.OwnerClientId, networkObject.OwnerClientId);
                    OnHitPlayer?.Invoke(fromPlayerToEnemey);
                    return;
                }
            }
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
