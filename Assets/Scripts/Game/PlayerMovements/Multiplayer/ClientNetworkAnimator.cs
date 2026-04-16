using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;
/// <summary>
/// Manages the synchronization of the animation for both players
/// </summary>
public class ClientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}