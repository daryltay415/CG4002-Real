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
    public int maxHealth = 20;

    public override void OnNetworkSpawn()
    {
        PlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnLocalPlayerHealthChanged;
        Debug.Log("Setting networkmanager: " + NetworkManager.Singleton.LocalClientId);
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
        HealthLevel.fillAmount = PlayerDataManager.Instance.GetPlayerHealth(id)/maxHealth;
    }

    public override void OnNetworkDespawn()
    {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }

    private void OnDisable() {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnLocalPlayerHealthChanged;
    }
}
