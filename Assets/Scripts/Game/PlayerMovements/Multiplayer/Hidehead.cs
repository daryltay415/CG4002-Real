using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
/// <summary>
/// Hides the head of the player and shows the head of the opponent
/// </summary>
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
