using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ProjectileData : NetworkBehaviour {

    private NetworkVariable<bool> isActiveSelf = new(true);

    private const int MAX_FLY_TIME = 3;

    public override void OnNetworkSpawn()
    {
        DeactivateSelfDelay();
    }

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


    public void DeactivateSelfDelay()
    {
        StartCoroutine(DeactivateSelfDelayCoroutine());
    }

    IEnumerator DeactivateSelfDelayCoroutine()
    {
        yield return new WaitForSeconds(MAX_FLY_TIME);
        SetProjectileIsActiveServerRpc(false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.transform.TryGetComponent(out NetworkObject networkObject))
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && OwnerClientId != networkObject.OwnerClientId)
                {
                    ulong from = OwnerClientId;
                    ulong to = networkObject.OwnerClientId;
                    collision.gameObject.GetComponent<PlayerStateMachineMultiplayer>().ProjectileCollisionOnObject(from,to);
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
