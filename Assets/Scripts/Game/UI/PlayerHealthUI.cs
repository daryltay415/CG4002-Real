using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
/// <summary>
/// This class manages the player health and the health UI
/// </summary>
public class PlayerHealthUI : NetworkBehaviour
{
    [SerializeField] private TMP_Text HealthText; // The health points UI

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        PlayerDataManager.Instance.OnPlayerHealthChanged += InstanceOnOnPlayerHealthChangedServerRpc;
        InstanceOnOnPlayerHealthChangedServerRpc(GetComponentInParent<NetworkObject>().OwnerClientId);

    }
    
    // When the player's health is changed, the health UI will be updated
    [ServerRpc(RequireOwnership = false)]
    private void InstanceOnOnPlayerHealthChangedServerRpc(ulong id)
    {
        if (GetComponentInParent<NetworkObject>().OwnerClientId == id)
        {
            SetHealthTextClientRpc(id);
        }
    }

    private void Update()
    {
        if (_mainCamera)
        {
            HealthText.transform.LookAt(_mainCamera.transform);
        }
    }

    [ClientRpc]
    void SetHealthTextClientRpc(ulong id)
    {
        HealthText.text = PlayerDataManager.Instance.GetPlayerHealth(id).ToString();
    }

    public override void OnNetworkDespawn()
    {
        PlayerDataManager.Instance.OnPlayerHealthChanged -= InstanceOnOnPlayerHealthChangedServerRpc;
    }
}
