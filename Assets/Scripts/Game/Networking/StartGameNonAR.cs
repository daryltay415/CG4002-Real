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
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject controls;
    [SerializeField] private SpawnPrefab spawnpre;
    
    
    void Start()
    {
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

    void StartGame()
    {
        menu.SetActive(false);
        spawnpre.Spawn();
        controls.SetActive(true);
    }

}