using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameARV2 : MonoBehaviour
{

    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button CreateRoomButton;
    [SerializeField] private Button JoinRoomButton;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controls;
    [SerializeField] private SpawnPrefab spawnpre;
    [SerializeField] private string hostIpAddress = "192.168.1.XX";
    private bool isHost;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //_sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerOnsharedSpaceManagerStateChanged;
        //Debug.Log("sharedspacemanager = " + _sharedSpaceManager);
        StartGameButton.onClick.AddListener(StartGame);
        CreateRoomButton.onClick.AddListener(CreateGameHost);
        JoinRoomButton.onClick.AddListener(JoinGameClient);
        
        StartGameButton.interactable = false;
    }

    private void OnDestroy()
    {
        //_sharedSpaceManager.sharedSpaceManagerStateChanged -= SharedSpaceManagerOnsharedSpaceManagerStateChanged;
    }


    //private void SharedSpaceManagerOnsharedSpaceManagerStateChanged(SharedSpaceManager.SharedSpaceManagerStateChangeEventArgs obj)
    //{
    //    if (obj.Tracking)
    //    {
    //        Debug.Log("trackingobj");
    //        StartGameButton.interactable = true;
    //        CreateRoomButton.interactable = false;
    //        JoinRoomButton.interactable = false;
    //    }
    //    Debug.Log("not tracking obj");
    //}

    public void StartGame()
    {
        //OnStartGame?.Invoke();
        
    var discovery = NetworkManager.Singleton.GetComponent<NetworkDiscovery>();

    if (isHost)
    {
        discovery.StartHosting();
        OnNetworkReady();
    }
    else
    {
        // The Client now injects the IP into the Transport before starting
        discovery.JoinGame(hostIpAddress);
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }
    }


    private void OnNetworkReady()
    {
        // Safety check for your singleton
    if (PlayerDataManager.Instance != null && 
        PlayerDataManager.Instance.GetHasPlayerPlaced(NetworkManager.Singleton.LocalClientId))
    {
        return;
    }

    menu.SetActive(false);
    controls.SetActive(true);
    
    if (spawnpre != null)
    {
        spawnpre.Spawn();
        Debug.Log("Spawn command sent to SpawnPrefab script");
    }
    else
    {
        Debug.LogError("SpawnPrefab reference is missing in the Inspector!");
    }
    }

    private void HandleClientConnected(ulong id)
    {
        // Make sure we only trigger this for our own local join
        if (id == NetworkManager.Singleton.LocalClientId)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            OnNetworkReady();
        }
    }

    //void StartSharedSpace()
    //{
//
    //    if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.MockColocalization)
    //    {
    //        var mockTrackingArgs = ISharedSpaceTrackingOptions.CreateMockTrackingOptions();
    //        var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
    //            roomName,
    //            MAX_AMOUNT_CLIENTS_ROOM,
    //            "MockColocalizationDemo"
    //        );
    //        
    //        _sharedSpaceManager.StartSharedSpace(mockTrackingArgs,roomArgs);
    //        return;
    //    }
    //    
    //    if (_sharedSpaceManager.GetColocalizationType() == SharedSpaceManager.ColocalizationType.ImageTrackingColocalization)
    //    {
    //        var imageTrackingOptions = ISharedSpaceTrackingOptions.CreateImageTrackingOptions(
    //            _targetImage, _targetImageSize
    //            );
    //        
    //        var roomArgs = ISharedSpaceRoomOptions.CreateLightshipRoomOptions(
    //            roomName,
    //            MAX_AMOUNT_CLIENTS_ROOM,
    //            "ImageColocalization"
    //        );
    //        
    //        _sharedSpaceManager.StartSharedSpace(imageTrackingOptions,roomArgs);
    //        Debug.Log("Start shared space");
    //        return;
    //    }
    //    
    //    
    //    
    //}
    

    void CreateGameHost()
    {
        Debug.Log("Creating host");
        isHost = true;
        StartGameButton.interactable = true;
        CreateRoomButton.interactable = false;
        JoinRoomButton.interactable = false;
        //StartSharedSpace();
    }

    void JoinGameClient()
    {
        Debug.Log("Join button clicked: Setting isHost to false");
        isHost = false;
        StartGameButton.interactable = true;
        CreateRoomButton.interactable = false;
        JoinRoomButton.interactable = false;
        //StartSharedSpace();
    }
    
    
}