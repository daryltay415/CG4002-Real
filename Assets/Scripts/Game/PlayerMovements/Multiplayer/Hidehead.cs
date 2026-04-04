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
            head.SetActive(false);
        }
    }
}
