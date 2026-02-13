using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.SharedAR.Colocalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAR : MonoBehaviour
{
    //[SerializeField] private SharedSpaceManager _sharedSpaceManager;
    //private const int MAX_AMOUNT_CLIENTS_ROOM = 2;
//
    //[SerializeField] private Texture2D _targetImage;
    //[SerializeField] private float _targetImageSize;
    //private string roomName = "TestRoom";
//
    //[SerializeField] private Button StartGameButton;
    //[SerializeField] private Button CreateRoomButton;
    //[SerializeField] private Button JoinRoomButton;
    //[SerializeField] private GameObject menu;
    //[SerializeField] private GameObject controls;
    //[SerializeField] private SpawnPrefab spawnpre;
    //private bool isHost;

    void Start()
    {
        
    }


   //private void Awake()
   //{
   //    DontDestroyOnLoad(gameObject);
   //    _sharedSpaceManager.sharedSpaceManagerStateChanged += SharedSpaceManagerOnsharedSpaceManagerStateChanged;
   //    Debug.Log("sharedspacemanager = " + _sharedSpaceManager);
   //    StartGameButton.onClick.AddListener(StartGame);
   //    CreateRoomButton.onClick.AddListener(CreateGameHost);
   //    JoinRoomButton.onClick.AddListener(JoinGameClient);
   //    
   //    StartGameButton.interactable = false;
   //}

   //private void OnDestroy()
   //{
   //    _sharedSpaceManager.sharedSpaceManagerStateChanged -= SharedSpaceManagerOnsharedSpaceManagerStateChanged;
   //}


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

   //public void StartGame()
   //{
   //    //OnStartGame?.Invoke();
   //    
   //    if (isHost)
   //    {
   //        NetworkManager.Singleton.StartHost();
   //        Debug.Log("starting host");
   //        OnNetworkReady();
   //    }
   //    else
   //    {
   //        NetworkManager.Singleton.StartClient();
   //        Debug.Log("starting client");
   //        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
   //    }
   //    bool actuallyHost = NetworkManager.Singleton.IsHost;
   //    Debug.Log($"Am I the host? {actuallyHost}");
   //}


   //private void OnNetworkReady()
   //{
   //    if (PlayerDataManager.Instance.GetHasPlayerPlaced(NetworkManager.Singleton.LocalClientId))
   //    {
   //        return;
   //    }

   //    menu.SetActive(false);
   //    //spawnpre.Spawn(_sharedSpaceManager.SharedArOriginObject.transform);
   //    controls.SetActive(true);
   //}

   //private void HandleClientConnected(ulong id)
   //{
   //    // Make sure we only trigger this for our own local join
   //    if (id == NetworkManager.Singleton.LocalClientId)
   //    {
   //        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
   //        OnNetworkReady();
   //    }
   //}

   //void StartSharedSpace()
   //{

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
   //

   //void CreateGameHost()
   //{
   //    Debug.Log("Creating host");
   //    isHost = true;
   //    StartSharedSpace();
   //}

   //void JoinGameClient()
   //{
   //    Debug.Log("Join button clicked: Setting isHost to false");
   //    isHost = false;
   //    StartSharedSpace();
   //}
   //
   //
}