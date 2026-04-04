using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This class manages the player health and the health UI
/// </summary>
public class PlayerHealthUILocal : NetworkBehaviour
{
    [SerializeField] private Image Healthbar; // The health points UI
    [SerializeField] private Image HealthLevel;
    private int maxHealth;

    

    public override void OnNetworkSpawn()
    {
        maxHealth = PlayerDataManager.Instance.LIFEPOINTS;
        PlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnLocalPlayerHealthChanged;
        Debug.Log("Setting networkmanager: " + NetworkManager.Singleton.LocalClientId);
        InstanceOnOnLocalPlayerHealthChanged(NetworkManager.Singleton.LocalClientId);
        Invoke(nameof(ForceInitialSync), 0.5f);
        //NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnectedHealthUI;

    }

    void ForceInitialSync()
    {
        Debug.Log("Forcing Sync: " + PlayerDataManager.Instance.GetPlayerHealth(NetworkManager.Singleton.LocalClientId));
        InstanceOnOnLocalPlayerHealthChanged(NetworkManager.Singleton.LocalClientId);
    }
    
    // When the player's health is changed, the health UI will be updated
    private void InstanceOnOnLocalPlayerHealthChanged(ulong id)
    {
        if(id == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("setting health");
            SetHealthTextLocal(id);
        }   
    }

    //void HandleClientConnectedHealthUI(ulong id)
    //{
    //    Debug.Log("Handling client connected");
    //    InstanceOnOnLocalPlayerHealthChanged(id);
    //    NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnectedHealthUI;
    //}

    void SetHealthTextLocal(ulong id)
    {
        HealthLevel.fillAmount = (float)PlayerDataManager.Instance.GetPlayerHealth(id)/maxHealth;
        Debug.Log("ratio: " + (float)PlayerDataManager.Instance.GetPlayerHealth(id)/maxHealth);
        Debug.Log("Playerhealth: " + PlayerDataManager.Instance.GetPlayerHealth(id));
        Debug.Log("maxHealth: " + maxHealth);
    }

    public override void OnNetworkDespawn()
    {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }

    private void OnDisable() {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }
}
