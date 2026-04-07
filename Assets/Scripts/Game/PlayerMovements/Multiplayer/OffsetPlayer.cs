using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class OffsetPlayer : NetworkBehaviour {
    public override void OnNetworkSpawn()
    {
        // Access the child that holds the actual graphics
        Transform visualChild = transform.GetChild(0);

        if (IsOwner)
        {
            // On your phone, keep the ninja close (First Person)
            // Your zoffset (0.8) + 0 = 0.8m from camera
            visualChild.localPosition = Vector3.zero;
        }
        else
        {
            // On the opponent's phone, push the ninja forward
            // Your zoffset (0.8) + 2.0 = 2.8m from your phone
            visualChild.localPosition = new Vector3(0, 0, 1f);
        }
    }
}
