
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class SpawnPrefab : NetworkBehaviour
{
    public GameObject prefabToSpawn;
    //private Transform parentObjectTrans;

    //public override void OnNetworkSpawn()
    //{
    //    if (IsServer || IsClient)
    //    {
    //        StartGameAR.OnStartGame += Spawn;
    //    }
    //}

    public void Spawn() {
        //Vector3 spawnPos = new Vector3(0f,-0.842f,0.757f);
        //if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsListening)
        //{
        //    Debug.LogError("Network not ready!");
        //    return;
        //}
        //
        //if (NetworkManager.Singleton.LocalClientId == 0)
        //{
        //    Debug.LogError("Client ID not assigned yet!");
        //    return;
        //}
        //Debug.Log("Spawning relative to: " + origin.name);
        //Debug.Log("bruH");
        //`Debug.Log("LocalclientID:" + NetworkManager.Singleton.LocalClientId);
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
        //var manager = Object.FindFirstObjectByType<Niantic.Lightship.SharedAR.Colocalization.SharedSpaceManager>();
        //if (manager != null && manager.SharedArOriginObject != null)
        //{
        //    // Set parent and ensure local position is used
        //    prefab.transform.SetParent(manager.SharedArOriginObject.transform, false);
        //    prefab.transform.localPosition = position;
        //    prefab.transform.localRotation = rotation;
        //}
        PlayerDataManager.Instance.AddPlacedPlayer(callerID);
        //prefab.transform.SetParent(parentObjectTrans, true);
    }

    //public override void OnNetworkDespawn()
    //{
    //    StartGameAR.OnStartGame -= Spawn;
    //}

}
