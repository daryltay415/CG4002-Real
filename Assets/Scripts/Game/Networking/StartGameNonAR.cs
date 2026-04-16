using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class manages the initialisation of the game in the non AR setting
/// </summary>
public class StartGameNonAR : NetworkBehaviour
{
    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject specialMeter;
    [SerializeField] private UIManager uimanager;
    [SerializeField] private SpawnPrefab spawnpre;
    [SerializeField] private Button tutorial;
    private bool isTutMode = false;
    public MsgVisualiser msgVisualiser;
    private int topicToSub = 1;
    private bool isHost;
    
    void Start()
    {
        player1Button.onClick.AddListener(onPlayer1ButtonClicked);
        player2Button.onClick.AddListener(onPlayer2ButtonClicked);
        startHost.onClick.AddListener(() =>
        {
            //NetworkManager.Singleton.StartHost();
            isHost = true;
            StartGameButton.interactable = true;
        });
        
        startClient.onClick.AddListener(() =>
        {
            isHost = false;
            //NetworkManager.Singleton.StartClient();
            StartGameButton.interactable = true;
        });

        StartGameButton.onClick.AddListener(StartGame);
        tutorial.onClick.AddListener(StartTutorial);
        StartGameButton.interactable = false;
        
    }

    private void onPlayer1ButtonClicked()
    {
        player1Button.GetComponent<Image>().color = Color.green;
        player2Button.GetComponent<Image>().color = Color.white;
        topicToSub = 1;
        msgVisualiser.TopicToSub = topicToSub;
        msgVisualiser.StartMQQTConnection();
        uimanager.ShowCreateGameCanvas();
        Debug.Log("Im player 1");
    }

    private void onPlayer2ButtonClicked()
    {
        player1Button.GetComponent<Image>().color = Color.white;
        player2Button.GetComponent<Image>().color = Color.green;
        topicToSub = 2;
        msgVisualiser.TopicToSub = topicToSub;
        msgVisualiser.StartMQQTConnection();
        uimanager.ShowCreateGameCanvas();
        Debug.Log("Im player 2");
    }

    public void StartGame()
    {
        MsgVisualiser.Instance.inGameplay = true;
        if (isHost)
        {
            NetworkManager.Singleton.StartHost();
            OnNetworkReady();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }
        bool actuallyHost = NetworkManager.Singleton.IsHost;
    }

    // Checks if the player has a sprite placed. If not, it spawns a sprite for the player connected and set the control UI active
    private void OnNetworkReady()
    {
        if (PlayerDataManager.Instance.GetHasPlayerPlaced(NetworkManager.Singleton.LocalClientId))
        {
            return;
        }

        spawnpre.Spawn(topicToSub);
        if (isTutMode)
        {
            uimanager.ShowTutorialControls();
        }
        else
        {
            uimanager.ShowPlayerControls();
        }
        
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

    void StartTutorial()
    {
        isTutMode = true;
        NetworkManager.Singleton.StartHost();
        StartGameButton.interactable = true;
    }

}