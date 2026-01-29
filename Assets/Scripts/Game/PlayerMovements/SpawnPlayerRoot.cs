using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class SpawnPlayerRoot : NetworkBehaviour
{
    public GameObject playerRoot;
    public void SpawnPlayer()
    {
        SpawnPlayerServerRPC(Vector3.zero, Quaternion.identity, NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRPC(Vector3 position, Quaternion rotation, ulong callerID)
    { 
        //Instantiate prefab
        //GameObject prefab = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        GameObject prefab = Instantiate(playerRoot, position, rotation);
        NetworkObject characterNetworkObject = prefab.GetComponent<NetworkObject>();
        characterNetworkObject.SpawnWithOwnership(callerID);
    }
}
