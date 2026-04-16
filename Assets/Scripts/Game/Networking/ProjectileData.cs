using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
/// <summary>
/// This class manages the projectile data
/// </summary>
public class ProjectileData : NetworkBehaviour {

    private NetworkVariable<bool> isActiveSelf = new(true);
    private const int DAMAGE = 4;
    private const int MAX_FLY_TIME = 3;

    public override void OnNetworkSpawn()
    {
        DeactivateSelfDelay();
    }

    // Sets whether the projectile is active in the scene or not
    [ServerRpc(RequireOwnership = false)]
    public void SetProjectileIsActiveServerRpc(bool isActive)
    {
        if(!GetComponent<NetworkObject>()) return;
        
        
        isActiveSelf.Value = isActive;

        if (isActive == false)
        {
            GetComponent<NetworkObject>().Despawn();
        }
        else
        {
            GetComponent<NetworkObject>().Spawn();
        }
    }

    // Deactivates the projectile after a period of time
    public void DeactivateSelfDelay()
    {
        StartCoroutine(DeactivateSelfDelayCoroutine());
    }

    IEnumerator DeactivateSelfDelayCoroutine()
    {
        yield return new WaitForSeconds(MAX_FLY_TIME);
        SetProjectileIsActiveServerRpc(false);
    }
    
    // Checks for collision between the player and the projectile
    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            //if (collision.transform.TryGetComponent(out NetworkObject networkObject))
            if (collision.transform.TryGetComponent(out NetworkObject networkObject))
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && OwnerClientId != networkObject.OwnerClientId)
                {
                    ulong from = OwnerClientId;
                    ulong to = networkObject.OwnerClientId;
                    collision.gameObject.GetComponent<PlayerStateMachineMultiplayer>().ProjectileCollisionOnObject(from,to,DAMAGE);
                    SetProjectileIsActiveServerRpc(false);
                    return;
                }
            }
            else
            {
                SetProjectileIsActiveServerRpc(false);
            }
        }
    }
}
