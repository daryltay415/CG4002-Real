using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class manages the initialisation of the game and the shared room AR
/// </summary>
public class StartGameAR : MonoBehaviour
{
    // Lightship AR variables
    [SerializeField] private SharedSpaceManager _sharedSpaceManager;
    private const int MAX_AMOUNT_CLIENTS_ROOM = 2;

    [SerializeField] private Texture2D _targetImage;
    [SerializeField] private float _targetImageSize;
    private string roomName = "TestRoom";

    // Game UI buttons and variables
    [SerializeField] private Button TutorialButton;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button CreateRoomButton;
    [SerializeField] private Button JoinRoomButton;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controls;
    [SerializeField] private UIManager uimanager;
    [SerializeField] private SpawnPrefab spawnpre;
    [SerializeField] private SpawnPB spawnBag;

    //Network variables
    private bool isHost;
    private bool isTutMode = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerOnsharedSpaceManagerStateChanged;
        Debug.Log("sharedspacemanager = " + _sharedSpaceManager);
        StartGameButton.onClick.AddListener(StartGame);
        CreateRoomButton.onClick.AddListener(CreateGameHost);
        JoinRoomButton.onClick.AddListener(JoinGameClient);
        TutorialButton.onClick.AddListener(StartTutorial);
        StartGameButton.interactable = false;
        
        //ImageForColocalization.OnTextureRendered += BlitImageForColocalizationOnTextureRendered;
    }

    private void OnDestroy()
    {
        _sharedSpaceManager.sharedSpaceManagerStateChanged -= SharedSpaceManagerOnsharedSpaceManagerStateChanged;
        //ImageForColocalization.OnTextureRendered -= BlitImageForColocalizationOnTextureRendered;
    }

    //private void BlitImageForColocalizationOnTextureRendered(Texture2D texture)
    //{
    //    SetTargetImage(texture);
    //    StartSharedSpace();
    //}

    //void SetTargetImage(Texture2D texture2D)
    //{
    //    _targetImage = texture2D;
    //}

    // Checks if the image in the Image tracking AR manager is in the camera's view. If it is, the start button is set active
    private void SharedSpaceManagerOnsharedSpaceManagerStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
    {
        if (obj.Tracking)
        {
            Debug.Log("trackingobj");
            StartGameButton.interactable = true;
            CreateRoomButton.interactable = false;
            JoinRoomButton.interactable = false;
            TutorialButton.interactable = false;
        }
        Debug.Log("not tracking obj");
    }

    // Intializes the host and client
    public void StartGame()
    {
        //OnStartGame?.Invoke();
        
        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("starting host");
            OnNetworkReady();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("starting client");
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }
        //if(isHost==false && NetworkManager.Singleton.LocalClientId == 0){
        //        Debug.Log("cool");
        //        return;
        //}
        bool actuallyHost = NetworkManager.Singleton.IsHost;
        Debug.Log($"Am I the host? {actuallyHost}");
        //StartCoroutine(WaitForConnectionAndSpawn());
        //menu.SetActive(false);
        //spawnpre.Spawn();
        //controls.SetActive(true);
    }

    // Checks if the player has a sprite placed. If not, it spawns a sprite for the player connected and set the control UI active
    private void OnNetworkReady()
    {
        if (PlayerDataManager.Instance.GetHasPlayerPlaced(NetworkManager.Singleton.LocalClientId))
        {
            return;
        }

        //menu.SetActive(false);
        //spawnpre.Spawn();
        //controls.SetActive(true);
        spawnpre.Spawn();
        if (isTutMode)
        {
            spawnBag.SpawnPunchingBag();
            uimanager.ShowTutorialControls();
        }
        else
        {
            uimanager.ShowPlayerControls();
        }
        Debug.Log("Tut mode? " + isTutMode);
        
    }

    // Checks if the client has joined the game before spawning their sprite
    private void HandleClientConnected(ulong id)
    {
        // Make sure we only trigger this for own local join
        if (id == NetworkManager.Singleton.LocalClientId)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            OnNetworkReady();
        }
    }

    // Starts the shared space AR room with all the clients having a shared AR origin. 
    // The shared AR origin is based on the image in the AR Image tracking manager.
    void StartSharedSpace()
    {
        //OnStartSharedSpace?.Invoke();

        if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
        {
            var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
                roomName,
                MAX_AMOUNT_CLIENTS_ROOM,
                "MockColocalizationDemo"
            );
            
            _sharedSpaceManager.StartSharedSpace(mockTrackingArgs,roomArgs);
            return;
        }
        
        if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization)
        {
            var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
                _targetImage, _targetImageSize
                );
            int noOfClients = MAX_AMOUNT_CLIENTS_ROOM;
            if (isTutMode)
            {
                noOfClients = 1;
            }   
            var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
            roomName,
            noOfClients,
            "ImageColocalization"
                );
        
            
            _sharedSpaceManager.StartSharedSpace(imageTrackingOptions,roomArgs);
            Debug.Log("Start shared space");
            return;
        }
    }

    // Creates the host and set this player as the host
    void CreateGameHost()
    {
        Debug.Log("Creating host");
        isHost = true;
        //OnStartSharedSpaceHost?.Invoke();
        StartSharedSpace();
    }

    // Joins the game as a client and set this player as the client
    void JoinGameClient()
    {
        Debug.Log("Join button clicked: Setting isHost to false");
        isHost = false;
        //OnJoinSharedSpaceClient?.Invoke();
        StartSharedSpace();
    }

    private void SpawnBag()
    {
        spawnBag.SpawnPunchingBag();
    }

    void StartTutorial()
    {
        Debug.Log("Creating tutorial");
        isHost = true;
        isTutMode = true;
        StartSharedSpace();
        
    }
    
    
}