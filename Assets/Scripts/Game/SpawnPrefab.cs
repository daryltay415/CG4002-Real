
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class SpawnPrefab : NetworkBehaviour
{
    public GameObject prefabToSpawn;
    //private Transform parentObjectTrans;

    public void Spawn() {
        //Vector3 spawnPos = new Vector3(0f,-0.842f,0.757f);
        SpawnPlayerServerRPC(Vector3.zero, Quaternion.identity, NetworkManager.Singleton.LocalClientId);
        
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnPlayerServerRPC(Vector3 position, Quaternion rotation, ulong callerID)
    { 
        Debug.Log("Hello there");
        //Instantiate prefab
        //GameObject prefab = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        GameObject prefab = Instantiate(prefabToSpawn, position, rotation);
        NetworkObject characterNetworkObject = prefab.GetComponent<NetworkObject>();
        characterNetworkObject.SpawnWithOwnership(callerID);
        PlayerDataManager.Instance.AddPlacedPlayer(callerID);
        //prefab.transform.SetParent(parentObjectTrans, true);
    }

    void Update()
    {
        //if (PlayerDataManager.Instance.GetHasPlayerPlaced(NetworkManager.Singleton.LocalClientId))
        //{
        //    return;
        //}
    }
}
