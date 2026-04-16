using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This class manages the player health and the health UI locally
/// </summary>
public class PlayerHealthUILocal : NetworkBehaviour
{
    [SerializeField] private Image Healthbar;
    [SerializeField] private Image HealthLevel;
    private int maxHealth;

    
    // Set the starting health for the player health bar
    public override void OnNetworkSpawn()
    {
        maxHealth = PlayerDataManager.Instance.LIFEPOINTS;
        PlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnLocalPlayerHealthChanged;
        Debug.Log("Setting networkmanager: " + NetworkManager.Singleton.LocalClientId);
        InstanceOnOnLocalPlayerHealthChanged(NetworkManager.Singleton.LocalClientId);
        Invoke(nameof(ForceInitialSync), 0.5f);

    }

    // Forces the client connected to have their health bar correctly set when they join the game
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


    void SetHealthTextLocal(ulong id)
    {
        HealthLevel.fillAmount = (float)PlayerDataManager.Instance.GetPlayerHealth(id)/maxHealth;
    }

    public override void OnNetworkDespawn()
    {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }

    private void OnDisable() {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }
}
