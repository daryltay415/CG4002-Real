
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
/// <summary>
/// This class spawns the player's sprite
/// </summary>
public class SpawnPrefab : NetworkBehaviour
{
    public GameObject prefabToSpawn;

    public void Spawn() {
        SpawnPlayerServerRPC(Vector3.zero, Quaternion.identity, NetworkManager.Singleton.LocalClientId);
        
    }

    // Spawns the player and add them into the playerdatamanager
    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRPC(Vector3 position, Quaternion rotation, ulong callerID)
    { 
        Debug.Log("Hello there");
        GameObject prefab = Instantiate(prefabToSpawn, position, rotation);
        NetworkObject characterNetworkObject = prefab.GetComponent<NetworkObject>();
        characterNetworkObject.SpawnAsPlayerObject(callerID);
        PlayerDataManager.Instance.AddPlacedPlayer(callerID);
    }



}
