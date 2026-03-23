using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
/// <summary>
/// This class spawns the projectile
/// </summary>
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

    // Spawns the projectile at the fist of the player's sprite
    [ServerRpc]
    void SpawnProjectileServerRpc()
    {
        //for AR
        //Vector3 localPos = transform.parent.InverseTransformPoint(spawnPoint.position);
        //Quaternion localRot = Quaternion.Inverse(transform.parent.rotation) * spawnPoint.rotation;
        //GameObject projectile = Instantiate(projectileToSpawn, localPos, localRot);
        //NetworkObject projectileNetworkObject = projectile.GetComponent<NetworkObject>();
        //projectileNetworkObject.SpawnWithOwnership(networkobj.OwnerClientId);
        //Vector3 localRight = transform.parent.InverseTransformDirection(spawnPoint.right);
        //localRight.y=  0;
        //projectile.GetComponent<Rigidbody>().velocity = localRight * speed;
        //Debug.Log("Spawn projectile");

        //for Non AR
        GameObject projectile = Instantiate(projectileToSpawn, spawnPoint.position, spawnPoint.rotation);
        NetworkObject projectileNetworkObject = projectile.GetComponent<NetworkObject>();
        projectileNetworkObject.SpawnWithOwnership(networkobj.OwnerClientId);
        projectile.GetComponent<Rigidbody>().AddForce(spawnPoint.right * speed);
        Debug.Log("Spawn projectile");
    }
}
