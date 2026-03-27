
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
/// <summary>
/// This class spawns the player's sprite
/// </summary>
public class SpawnPrefab : NetworkBehaviour
{
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    private GameObject prefabToSpawn;

    public void Spawn(int TopicToSub) {
        SpawnPlayerServerRPC(Vector3.zero, Quaternion.identity, NetworkManager.Singleton.LocalClientId, TopicToSub);
        
    }

    // Spawns the player and add them into the playerdatamanager
    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRPC(Vector3 position, Quaternion rotation, ulong callerID, int TopicToSub)
    { 
        Debug.Log("Hello there");
        if(TopicToSub == 1)
        {
            prefabToSpawn = player1Prefab;
        }
        else
        {
            prefabToSpawn = player2Prefab;
        }
        GameObject prefab = Instantiate(prefabToSpawn, position, rotation);
        NetworkObject characterNetworkObject = prefab.GetComponent<NetworkObject>();
        characterNetworkObject.SpawnAsPlayerObject(callerID);
        PlayerDataManager.Instance.AddPlacedPlayer(callerID);
    }



}
