using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class Hidehead : NetworkBehaviour
{
    public GameObject head;
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            head.layer = LayerMask.NameToLayer("PlayerLocal");
            Debug.Log("Successfully hid my own head locally.");
            
        }
    }
}
