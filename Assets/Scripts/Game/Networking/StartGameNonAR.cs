using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
    public MsgVisualiser msgVisualiser;
    private int topicToSub = 1;
    
    
    void Start()
    {
        player1Button.onClick.AddListener(onPlayer1ButtonClicked);
        player2Button.onClick.AddListener(onPlayer2ButtonClicked);
        startHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            StartGameButton.interactable = true;
        });
        
        startClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            StartGameButton.interactable = true;
        });

        StartGameButton.onClick.AddListener(StartGame);
        
        StartGameButton.interactable = false;
        
    }

    private void onPlayer1ButtonClicked()
    {
        player1Button.GetComponent<Image>().color = Color.green;
        player2Button.GetComponent<Image>().color = Color.white;
        topicToSub = 1;
        msgVisualiser.TopicToSub = topicToSub;
        Debug.Log("Im player 1");
    }

    private void onPlayer2ButtonClicked()
    {
        player1Button.GetComponent<Image>().color = Color.white;
        player2Button.GetComponent<Image>().color = Color.green;
        topicToSub = 2;
        msgVisualiser.TopicToSub = topicToSub;
        Debug.Log("Im player 2");
    }

    void StartGame()
    {
        //menu.SetActive(false);
        uimanager.ShowPlayerControls();
        spawnpre.Spawn(topicToSub);
        //specialMeter.SetActive(true);
        //controls.SetActive(true);
    }

}