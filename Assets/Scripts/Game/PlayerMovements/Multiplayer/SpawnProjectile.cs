using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnProjectile : NetworkBehaviour 
    
{
    public Transform spawnPoint;
    public GameObject projectileToSpawn;
    public float speed = 5f;
    private NetworkObject networkobj;
    // Start is called before the first frame update

    public override void OnNetworkSpawn()
    {
        networkobj = GetComponent<NetworkObject>();
    }

    [ServerRpc]
    void SpawnProjectileServerRpc()
    {
        GameObject projectile = Instantiate(projectileToSpawn, spawnPoint.position, spawnPoint.rotation);
        NetworkObject projectileNetworkObject = projectile.GetComponent<NetworkObject>();
        projectileNetworkObject.SpawnWithOwnership(networkobj.OwnerClientId);
        projectile.GetComponent<Rigidbody>().AddForce(spawnPoint.right * speed);
    }
}
